using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PokemonApi.Data.PokeApi
{
    [DataContract]
    public class PokemonInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("habitat")]
        public Habitat Habitat { get; set; }

        [JsonPropertyName("flavor_text_entries")]
        public IEnumerable<FlavorTextEntry> FlavorTextEntries { get; set; }
    }
}