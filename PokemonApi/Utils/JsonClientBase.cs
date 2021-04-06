using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace PokemonApi.Utils
{
    public abstract class JsonClientBase
    {
        private readonly Uri _baseUrl;

        protected JsonClientBase(string baseUrl)
        {
            _baseUrl = new Uri(baseUrl);
        }

        protected TResponse Get<TResponse>(string relativeUrl)
        {
            var request = CreateGetRequest(relativeUrl);

            var response = (HttpWebResponse)request.GetResponse();
            return Deserialize<TResponse>(response);
        }

        protected TResponse Post<TBody, TResponse>(string relativeUrl, TBody body)
        {
            var request = WebRequest.Create($"{_baseUrl}/{relativeUrl}");
            request.Method = "POST";
            AddRequestBody(request, body);

            var response = (HttpWebResponse)request.GetResponse();
            return Deserialize<TResponse>(response);
        }

        #region Utility Methods

        private WebRequest CreateGetRequest(string relativeUrl)
        {
            var request = WebRequest.Create($"{_baseUrl}/{relativeUrl}");
            request.Method = "GET";

            return request;
        }

        private static T Deserialize<T>(WebResponse response)
        {
            using var streamReader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException());
            return JsonSerializer.Deserialize<T>(streamReader.ReadToEnd());
        }

        private static void AddRequestBody<T>(WebRequest request, T body)
        {
            var serializedBody = JsonSerializer.SerializeToUtf8Bytes(body);
            request.ContentType = "application/json";
            request.ContentLength = serializedBody.Length;
            using var stream = request.GetRequestStream();
            stream.Write(serializedBody, 0, serializedBody.Length);
        }

        #endregion
    }
}