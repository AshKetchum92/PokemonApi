using System;
using System.Net;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using PokemonApi.Data;
using PokemonApi.Services;
using Pokemon = PokemonApi.Services.Pokemon;

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

        #region GetPokemon

        [Test]
        public void IfRepositoryThrowsArgumentOutOfRangeException_GetPokemon_ShouldReturnHttpNotFound()
        {
            const string pokemonName = "pikachu";
            A.CallTo(() => _pokemonRepository.Get(A<string>._)).Throws<ArgumentOutOfRangeException>();

            var actionResult = _pokemonService.GetPokemon(pokemonName);

            var result = CastActionResultOrFail<NotFoundResult, Pokemon>(actionResult);
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public void IfRepositoryThrowsAnUnexpectedException_GetPokemon_ShouldReturnHttpInternalServerError()
        {
            const string pokemonName = "pikachu";
            A.CallTo(() => _pokemonRepository.Get(A<string>._)).Throws<Exception>();

            var actionResult = _pokemonService.GetPokemon(pokemonName);

            var result = CastActionResultOrFail<StatusCodeResult, Pokemon>(actionResult);
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test]
        public void GetPokemon_ShouldReturnThePokemonInfo()
        {
            const string pokemonName = "pikachu";
            var expectedPokemon = APokemon();
            A.CallTo(() => _pokemonRepository.Get(pokemonName)).Returns(expectedPokemon);

            var pokemon = _pokemonService.GetPokemon(pokemonName);

            pokemon.Value.Should().BeEquivalentTo(expectedPokemon, options => options.Excluding(p => p.TranslatedDescription));
        }

        #endregion

        #region GetPokemonTranslated

        [Test]
        public void IfRepositoryThrowsArgumentOutOfRangeException_GetPokemonTranslated_ShouldReturnHttpNotFound()
        {
            const string pokemonName = "pikachu";
            A.CallTo(() => _pokemonRepository.Get(A<string>._)).Throws<ArgumentOutOfRangeException>();

            var actionResult = _pokemonService.GetPokemonTranslated(pokemonName);

            var result = CastActionResultOrFail<NotFoundResult, Pokemon>(actionResult);
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public void IfRepositoryThrowsAnUnexpectedException_GetPokemonTranslated_ShouldReturnHttpInternalServerError()
        {
            const string pokemonName = "pikachu";
            A.CallTo(() => _pokemonRepository.Get(A<string>._)).Throws<Exception>();

            var actionResult = _pokemonService.GetPokemonTranslated(pokemonName);

            var result = CastActionResultOrFail<StatusCodeResult, Pokemon>(actionResult);
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test]
        public void IfRepositoryThrowsAnUnexpectedExceptionWhileGettingTranslatedDescription_GetPokemonTranslated_ShouldReturnThePokemonInfoWithTheStandardDescription()
        {
            const string pokemonName = "pikachu";
            var expectedPokemon = APokemon();
            A.CallTo(() => _pokemonRepository.Get(pokemonName)).Returns(expectedPokemon);
            A.CallTo(() => expectedPokemon.TranslatedDescription).Throws<Exception>();

            var pokemon = _pokemonService.GetPokemonTranslated(pokemonName);

            pokemon.Value.Should().BeEquivalentTo(expectedPokemon, options => options.Excluding(p => p.TranslatedDescription));
        }

        [Test]
        public void IfNoExceptionOccurs_GetPokemonTranslated_ShouldReturnThePokemonInfoWithTheTranslatedDescription()
        {
            const string pokemonName = "pikachu";
            const string expectedDescription = "a translated description";
            var expectedPokemon = APokemon();
            A.CallTo(() => _pokemonRepository.Get(pokemonName)).Returns(expectedPokemon);
            A.CallTo(() => expectedPokemon.TranslatedDescription).Returns(expectedDescription);

            var pokemon = _pokemonService.GetPokemonTranslated(pokemonName);

            pokemon.Value.Should().BeEquivalentTo(expectedPokemon, options => options.Excluding(p => p.TranslatedDescription).Excluding(p => p.Description));
            pokemon.Value.Description.Should().Be(expectedDescription);
        }

        #endregion

        #region Utility Methods

        private static IPokemon APokemon()
        {
            var pokemon = A.Fake<IPokemon>();
            A.CallTo(() => pokemon.Name).Returns("michu");
            A.CallTo(() => pokemon.Description).Returns("A description");
            A.CallTo(() => pokemon.Habitat).Returns("An habitat");
            A.CallTo(() => pokemon.IsLegendary).Returns(true);

            return pokemon;
        }

        private static T CastActionResultOrFail<T, T2>(ActionResult<T2> actionResult) where T : ActionResult
        {
            actionResult.Result.Should().BeAssignableTo<T>();
            return (T)actionResult.Result;
        }

        #endregion
    }
}