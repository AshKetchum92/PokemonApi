using System;
using System.Collections.Generic;
using Bogus;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using PokemonApi.Data;
using PokemonApi.Data.PokeApi;

namespace PokemonApiTest.Data
{
    [TestFixture]
    public class PokemonRepositoryTest
    {
        private IClient _pokeApiClient;
        private IPokemonRepository _pokemonRepository;

        [SetUp]
        public void SetUp()
        {
            _pokeApiClient = A.Fake<IClient>();
            _pokemonRepository = new PokemonRepository(_pokeApiClient);
        }

        [Test]
        public void IfGivenNameIsNull_Get_ShouldThrowException()
        {
            this.Invoking(t => _pokemonRepository.Get(null))
                .Should().ThrowExactly<ArgumentNullException>()
                .And.ParamName.Should().Be("name");
        }

        [Test]
        public void IfGivenNameIsEmpty_Get_ShouldThrowException()
        {
            this.Invoking(t => _pokemonRepository.Get(string.Empty))
                .Should().ThrowExactly<ArgumentException>()
                .And.ParamName.Should().Be("name");
        }

        [Test]
        public void IfExceptionIsReturnedByTheClient_Get_ShouldReThrowIt()
        {
            var exception = new Exception("An unexpected exception");
            A.CallTo(() => _pokeApiClient.GetPokemonSpecies(A<string>._)).Throws(exception);

            this.Invoking(t => _pokemonRepository.Get("pokemon"))
                .Should().Throw<Exception>().
                Where(e => e == exception);
        }

        #region Name

        [Test]
        public void Get_ShouldSetIt()
        {
            var pokemonName = new Faker().Lorem.Word();
            var expectedPokemonName = new Faker().Lorem.Word();
            A.CallTo(() => _pokeApiClient.GetPokemonSpecies(pokemonName)).Returns(new PokemonInfo
            {
                Name = expectedPokemonName
            });

            var pokemon = _pokemonRepository.Get(pokemonName);

            pokemon.Name.Should().Be(expectedPokemonName);
        }

        #endregion

        #region IsLegendary

        [Test]
        public void Get_ShouldSetIt([Values] bool expectedIsLegendary)
        {
            var pokemonName = new Faker().Lorem.Word();
            A.CallTo(() => _pokeApiClient.GetPokemonSpecies(pokemonName)).Returns(new PokemonInfo
            {
                IsLegendary = expectedIsLegendary
            });

            var pokemon = _pokemonRepository.Get(pokemonName);

            pokemon.IsLegendary.Should().Be(expectedIsLegendary);
        }

        #endregion

        #region Habitat

        [Test]
        public void IfNoHabitatIsReturnedByTheClient_Get_ShouldSetNull()
        {
            var pokemonName = new Faker().Lorem.Word();
            A.CallTo(() => _pokeApiClient.GetPokemonSpecies(pokemonName)).Returns(new PokemonInfo
            {
                Habitat = null
            });

            var pokemon = _pokemonRepository.Get(pokemonName);

            pokemon.Habitat.Should().BeNull();
        }

        [Test]
        public void IfHabitatIsReturnedByTheClient_Get_ShouldSetIt()
        {
            var pokemonName = new Faker().Lorem.Word();
            var expectedHabitat = new Faker().Lorem.Word();
            A.CallTo(() => _pokeApiClient.GetPokemonSpecies(pokemonName)).Returns(new PokemonInfo
            {
                Habitat = new Habitat
                {
                    Name = expectedHabitat
                }
            });

            var pokemon = _pokemonRepository.Get(pokemonName);

            pokemon.Habitat.Should().Be(expectedHabitat);
        }

        #endregion

        #region Description

        [Test]
        public void IfFlavorTextEntriesIsNull_Get_ShouldSetNull()
        {
            var pokemonName = new Faker().Lorem.Word();
            A.CallTo(() => _pokeApiClient.GetPokemonSpecies(pokemonName)).Returns(new PokemonInfo
            {
                FlavorTextEntries = null
            });

            var pokemon = _pokemonRepository.Get(pokemonName);

            pokemon.Description.Should().BeNull();
        }

        [Test]
        public void IfFlavorTextEntriesIsEmpty_Get_ShouldSetNull()
        {
            var pokemonName = new Faker().Lorem.Word();
            A.CallTo(() => _pokeApiClient.GetPokemonSpecies(pokemonName)).Returns(new PokemonInfo
            {
                FlavorTextEntries = new List<FlavorTextEntry>()
            });

            var pokemon = _pokemonRepository.Get(pokemonName);

            pokemon.Description.Should().BeNull();
        }

        [Test]
        public void IfFlavorTextContainsOnlyNonEnglishEntries_Get_ShouldSetNull()
        {
            var pokemonName = new Faker().Lorem.Word();
            A.CallTo(() => _pokeApiClient.GetPokemonSpecies(pokemonName)).Returns(new PokemonInfo
            {
                FlavorTextEntries = new List<FlavorTextEntry>
                {
                    AFlavorTextEntry("es", "Whatever"),
                    AFlavorTextEntry("de", "Whatever")
                }
            });

            var pokemon = _pokemonRepository.Get(pokemonName);

            pokemon.Description.Should().BeNull();
        }

        [Test]
        public void IfFlavorTextContainsAtLeastAnEnglishEntries_Get_ShouldSetItToTheFirstEnglishFlavorText()
        {
            var pokemonName = new Faker().Lorem.Word(); 
            var expectedDescription = new Faker().Lorem.Sentence();
            A.CallTo(() => _pokeApiClient.GetPokemonSpecies(pokemonName)).Returns(new PokemonInfo
            {
                FlavorTextEntries = new List<FlavorTextEntry>
                {
                    AFlavorTextEntry("es", "Whatever"),
                    AFlavorTextEntry("en", expectedDescription),
                    AFlavorTextEntry("en", "Whatever")
                }
            });

            var pokemon = _pokemonRepository.Get(pokemonName);

            pokemon.Description.Should().Be(expectedDescription);
        }

        #endregion

        #region Utility Methods

        private static FlavorTextEntry AFlavorTextEntry(string language, string flavorText)
        {
            return new FlavorTextEntry
            {
                Language = new Language
                {
                    Name = language
                },
                FlavorText = flavorText
            };
        }

        #endregion
    }
}