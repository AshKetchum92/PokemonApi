using System;
using System.Net;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using PokemonApi.Data;
using PokemonApi.Services;
using Pokemon = PokemonApi.Services.Pokemon;
using PokemonDataLayer = PokemonApi.Data.Pokemon;

namespace PokemonApiTest.Services
{
    [TestFixture]
    public class PokemonServiceTest
    {
        private IPokemonRepository _pokemonRepository;
        private PokemonService _pokemonService;

        [SetUp]
        public void SetUp()
        {
            _pokemonRepository = A.Fake<IPokemonRepository>();
            _pokemonService = new PokemonService(_pokemonRepository);
        }

        [Test]
        public void IfRepositoryThrowsArgumentOutOfRangeException_GetPokemon_ShouldReturnHttpNotFound()
        {
            const string pokemonName = "pikachu";
            A.CallTo(() => _pokemonRepository.Get(A<string>._)).Throws<ArgumentOutOfRangeException>();

            var actionResult = _pokemonService.Get(pokemonName);

            var result = CastActionResultOrFail<NotFoundResult, Pokemon>(actionResult);
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public void IfRepositoryThrowsAnUnexpectedException_GetPokemon_ShouldReturnHttpInternalServerError()
        {
            const string pokemonName = "pikachu";
            A.CallTo(() => _pokemonRepository.Get(A<string>._)).Throws<Exception>();

            var actionResult = _pokemonService.Get(pokemonName);

            var result = CastActionResultOrFail<StatusCodeResult, Pokemon>(actionResult);
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test]
        public void HappyPath()
        {
            const string pokemonName = "pikachu";
            var expectedPokemon = APokemon();
            A.CallTo(() => _pokemonRepository.Get(pokemonName)).Returns(expectedPokemon);

            var pokemon = _pokemonService.Get(pokemonName);

            pokemon.Value.Should().BeEquivalentTo(expectedPokemon);
        }

        #region Utility Methods

        private static IPokemon APokemon()
        {
            return new PokemonDataLayer
            {
                Name = "michu",
                Description = "A description",
                Habitat = "An habitat",
                IsLegendary = true
            };
        }

        private static T CastActionResultOrFail<T, T2>(ActionResult<T2> actionResult) where T : ActionResult
        {
            actionResult.Result.Should().BeAssignableTo<T>();
            return (T)actionResult.Result;
        }

        #endregion
    }
}