namespace PokemonApi.PokeApi
{
    public class Client : JsonClientBase
    {
        public Client() : base("https://pokeapi.co/api/v2/")
        {

        }

        public PokemonInfo GetPokemonSpecies(string pokemonName)
        {
            return Get<PokemonInfo>($"pokemon-species/{pokemonName}/");
        }
    }
}