using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PokemonApi.Data.FunTranslationsApi
{
    [DataContract]
    public class Contents
    {
        [JsonPropertyName("translated")]
        public string Translated { get; set; }
    }
}