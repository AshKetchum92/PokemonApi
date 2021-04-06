using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PokemonApi.Data.PokeApi
{
    [DataContract]
    public class Habitat
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}