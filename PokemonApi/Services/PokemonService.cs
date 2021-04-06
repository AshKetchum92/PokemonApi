using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokemonApi.Data;

namespace PokemonApi.Services
{
    [ApiController]
    [Route("pokemon")]
    public class PokemonService : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly ILogger<PokemonService> _logger;

        public PokemonService(IPokemonRepository pokemonRepository, ILogger<PokemonService> logger)
        {
            _pokemonRepository = pokemonRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route("{name}")]
        public ActionResult<Pokemon> GetPokemon(string name)
        {
            try
            {
                var sw = AStartedStopwatch();
                _logger.LogInformation($"{nameof(GetPokemon)} invoked, pokemon name: {name}");

                var result = ToPokemon(_pokemonRepository.Get(name));

                _logger.LogInformation($"{nameof(GetPokemon)} completed, elapsed time: {sw.ElapsedMilliseconds}ms");

                return result;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "No pokemon found with the given name");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving pokemon information");
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("translated/{name}")]
        public ActionResult<Pokemon> GetPokemonTranslated(string name)
        {
            try
            {
                var sw = AStartedStopwatch();
                _logger.LogInformation($"{nameof(GetPokemonTranslated)} invoked, pokemon name: {name}");

                var result = ToTranslatedPokemon(_pokemonRepository.Get(name));

                _logger.LogInformation($"{nameof(GetPokemonTranslated)} completed, elapsed time: {sw.ElapsedMilliseconds}ms");

                return result;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "No pokemon found with the given name");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving pokemon information");
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

        private Pokemon ToTranslatedPokemon(IPokemon pokemon)
        {
            var result = ToPokemon(pokemon);
            try
            {
                result.Description = pokemon.TranslatedDescription;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "An unexpected error occurred while retrieving translated pokemon description");
                // ignore
            }

            return result;
        }

        private static Stopwatch AStartedStopwatch()
        {
            var sw = new Stopwatch();
            sw.Start();

            return sw;
        }

        #endregion
    }
}