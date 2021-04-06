using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PokemonApi.Services
{
    [DataContract]
    public class Pokemon
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("isLegendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("habitat")]
        public string Habitat { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}