using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PokemonApi.PokeApi
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