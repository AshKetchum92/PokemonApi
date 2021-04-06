using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PokemonApi.PokeApi
{
    [DataContract]
    public class Habitat
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}