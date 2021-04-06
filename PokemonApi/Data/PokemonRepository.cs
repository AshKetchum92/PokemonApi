using System;
using System.Linq;
using PokemonApi.Data.PokeApi;

namespace PokemonApi.Data
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly IClient _pokeApiClient;

        public PokemonRepository(IClient pokeApiClient)
        {
            _pokeApiClient = pokeApiClient;
        }

        public IPokemon Get(string name)
        {
            ValidateOrFail(name);

            var pokemonSpecies = _pokeApiClient.GetPokemonSpecies(name);
            return ToPokemon(pokemonSpecies);
        }

        #region Utility Methods

        private static void ValidateOrFail(string name)
        {
            switch (name)
            {
                case null:
                    throw new ArgumentNullException(nameof(name));
                case "":
                    throw new ArgumentException(string.Empty, nameof(name));
            }
        }

        private static IPokemon ToPokemon(PokemonInfo pokemonSpecies)
        {
            return new Pokemon(new FunTranslationsApi.Client())
            {
                Name = pokemonSpecies.Name,
                IsLegendary = pokemonSpecies.IsLegendary,
                Habitat = pokemonSpecies.Habitat?.Name,
                Description = GetFirstEnglishFlavorText(pokemonSpecies)
            };
        }

        private static string GetFirstEnglishFlavorText(PokemonInfo pokemonSpecies)
        {
            return pokemonSpecies.FlavorTextEntries?.FirstOrDefault(f => f.Language.Name == "en")?.FlavorText;
        }

        #endregion
    }
}