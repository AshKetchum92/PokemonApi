using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PokemonApi.Data.FunTranslationsApi
{
    [DataContract]
    public class Request
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}