using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PokemonApi.Data.PokeApi
{
    [DataContract]
    public class FlavorTextEntry
    {
        [JsonPropertyName("flavor_text")]
        public string FlavorText { get; set; }

        [JsonPropertyName("language")]
        public Language Language { get; set; }
    }
}