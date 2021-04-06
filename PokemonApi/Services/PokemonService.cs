using System;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Data;

namespace PokemonApi.Services
{
    [ApiController]
    [Route("pokemon")]
    public class PokemonService : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;

        public PokemonService(IPokemonRepository pokemonRepository)
        {
            _pokemonRepository = pokemonRepository;
        }

        [HttpGet]
        [Route("{name}")]
        public ActionResult<Pokemon> GetPokemon(string name)
        {
            try
            {
                return ToPokemon(_pokemonRepository.Get(name));
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("translated/{name}")]
        public ActionResult<Pokemon> GetPokemonTranslated(string name)
        {
            try
            {
                return ToTranslatedPokemon(_pokemonRepository.Get(name));
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        #region Utility Methods

        private static Pokemon ToPokemon(IPokemon pokemon)
        {
            return new Pokemon
            {
                Name = pokemon.Name,
                IsLegendary = pokemon.IsLegendary,
                Habitat = pokemon.Habitat,
                Description = pokemon.Description
            };
        }

        private static Pokemon ToTranslatedPokemon(IPokemon pokemon)
        {
            var result = ToPokemon(pokemon);
            try
            {
                result.Description = pokemon.TranslatedDescription;
            }
            catch
            {
                // ignore
            }

            return result;
        }

        #endregion
    }
}