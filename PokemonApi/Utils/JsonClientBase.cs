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

        #endregion
    }
}