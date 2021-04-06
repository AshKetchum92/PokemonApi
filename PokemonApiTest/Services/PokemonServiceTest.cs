using System;
using System.Net;
using Bogus;
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
            var pokemonName = new Faker().Lorem.Word();
            A.CallTo(() => _pokemonRepository.Get(A<string>._)).Throws<ArgumentOutOfRangeException>();

            var actionResult = _pokemonService.GetPokemon(pokemonName);

            var result = CastActionResultOrFail<NotFoundResult, Pokemon>(actionResult);
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public void IfRepositoryThrowsAnUnexpectedException_GetPokemon_ShouldReturnHttpInternalServerError()
        {
            var pokemonName = new Faker().Lorem.Word();
            A.CallTo(() => _pokemonRepository.Get(A<string>._)).Throws<Exception>();

            var actionResult = _pokemonService.GetPokemon(pokemonName);

            var result = CastActionResultOrFail<StatusCodeResult, Pokemon>(actionResult);
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test]
        public void GetPokemon_ShouldReturnThePokemonInfo()
        {
            var pokemonName = new Faker().Lorem.Word();
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
            var pokemonName = new Faker().Lorem.Word();
            A.CallTo(() => _pokemonRepository.Get(A<string>._)).Throws<ArgumentOutOfRangeException>();

            var actionResult = _pokemonService.GetPokemonTranslated(pokemonName);

            var result = CastActionResultOrFail<NotFoundResult, Pokemon>(actionResult);
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public void IfRepositoryThrowsAnUnexpectedException_GetPokemonTranslated_ShouldReturnHttpInternalServerError()
        {
            var pokemonName = new Faker().Lorem.Word();
            A.CallTo(() => _pokemonRepository.Get(A<string>._)).Throws<Exception>();

            var actionResult = _pokemonService.GetPokemonTranslated(pokemonName);

            var result = CastActionResultOrFail<StatusCodeResult, Pokemon>(actionResult);
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test]
        public void IfRepositoryThrowsAnUnexpectedExceptionWhileGettingTranslatedDescription_GetPokemonTranslated_ShouldReturnThePokemonInfoWithTheStandardDescription()
        {
            var pokemonName = new Faker().Lorem.Word();
            var expectedPokemon = APokemon();
            A.CallTo(() => _pokemonRepository.Get(pokemonName)).Returns(expectedPokemon);
            A.CallTo(() => expectedPokemon.TranslatedDescription).Throws<Exception>();

            var pokemon = _pokemonService.GetPokemonTranslated(pokemonName);

            pokemon.Value.Should().BeEquivalentTo(expectedPokemon, options => options.Excluding(p => p.TranslatedDescription));
        }

        [Test]
        public void IfNoExceptionOccurs_GetPokemonTranslated_ShouldReturnThePokemonInfoWithTheTranslatedDescription()
        {
            var pokemonName = new Faker().Lorem.Word();
            var expectedDescription = new Faker().Lorem.Sentence();
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
            A.CallTo(() => pokemon.Name).Returns(new Faker().Lorem.Word());
            A.CallTo(() => pokemon.Description).Returns(new Faker().Lorem.Text());
            A.CallTo(() => pokemon.Habitat).Returns(new Faker().Lorem.Word());
            A.CallTo(() => pokemon.IsLegendary).Returns(new Faker().Random.Bool());

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