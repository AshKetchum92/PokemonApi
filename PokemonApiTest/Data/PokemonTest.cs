using Bogus;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using PokemonApi.Data;
using PokemonApi.Data.FunTranslationsApi;

namespace PokemonApiTest.Data
{
    [TestFixture]
    public class PokemonTest
    {
        private IClient _funTranslationsClient;
        private Pokemon _pokemon;

        [SetUp]
        public void Setup()
        {
            _funTranslationsClient = A.Fake<IClient>();
            _pokemon = new Pokemon(_funTranslationsClient);
        }

        [TestCase("cave", false)]
        [TestCase("forest", true)]
        [TestCase("cave", true)]
        public void IfPokemonHabitatIsCaveOrItIsALegendaryPokemon_TranslatedDescription_ShouldApplyYodaTranslation(string habitat, bool isLegendary)
        {
            var description = new Faker().Lorem.Sentence();
            var expectedTranslation = new Faker().Lorem.Sentence();
            A.CallTo(() => _funTranslationsClient.TranslateToShakespeare(description)).Returns(new Result { Contents = new Contents { Translated = expectedTranslation } });
            _pokemon.Habitat = habitat;
            _pokemon.IsLegendary = isLegendary;
            _pokemon.Description = description;

            var translatedDescription = _pokemon.TranslatedDescription;

            translatedDescription.Should().Be(expectedTranslation);
        }

        [Test]
        public void IfPokemonHabitatIsNotCaveAndItIsNotALegendaryPokemon_TranslatedDescription_ShouldApplyYodaTranslation()
        {
            var description = new Faker().Lorem.Sentence();
            var expectedTranslation = new Faker().Lorem.Sentence();
            A.CallTo(() => _funTranslationsClient.TranslateToYoda(description)).Returns(new Result { Contents = new Contents { Translated = expectedTranslation } });
            _pokemon.Habitat = "forest";
            _pokemon.IsLegendary = false;
            _pokemon.Description = description;

            var translatedDescription = _pokemon.TranslatedDescription;

            translatedDescription.Should().Be(expectedTranslation);
        }
    }
}