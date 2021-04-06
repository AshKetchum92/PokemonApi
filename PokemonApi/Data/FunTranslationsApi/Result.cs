using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PokemonApi.Data.FunTranslationsApi
{
    [DataContract]
    public class Result
    {
        [JsonPropertyName("contents")] 
        public Contents Contents { get; set; }
    }
}