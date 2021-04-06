using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PokemonApi.PokeApi
{
    [DataContract]
    public class Language
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}