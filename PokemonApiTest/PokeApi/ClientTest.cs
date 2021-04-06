using System.Linq;
using System.Net;
using FluentAssertions;
using NUnit.Framework;
using PokemonApi.PokeApi;

namespace PokemonApiTest.PokeApi
{
    public class ClientTest
    {
        private const string ExpectedPikachuFlavorText = "When several of\nthese POKéMON\ngather, their\u000celectricity could\nbuild and cause\nlightning storms.";
        private const string ExpectedHoOhFlavorText = "Legends claim this\nPOKéMON flies the\nworld's skies con\u00AD\u000ctinuously on its\nmagnificent seven-\ncolored wings.";
        private Client _pokeApiClient;

        [SetUp]
        public void Setup()
        {
            _pokeApiClient = new Client();
        }

        [Test]
        public void IfGivenNameIsInvalid_ShouldReturnHttpNotFound()
        {
            const string invalidPokemonName = "NotAValidPokemonName";

            ((HttpWebResponse)this.Invoking(t => _pokeApiClient.GetPokemonSpecies(invalidPokemonName))
                .Should().ThrowExactly<WebException>()
                .And.Response).StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public void IfGivenNameIsPikachu_ShouldReturnPikachuInfo()
        {
            var pokemonInfo = _pokeApiClient.GetPokemonSpecies("pikachu");

            pokemonInfo.Name.Should().Be("pikachu");
            pokemonInfo.IsLegendary.Should().BeFalse();
            pokemonInfo.Habitat.Name.Should().Be("forest");
            GetFirstEnglishFlavorText(pokemonInfo).Should().Be(ExpectedPikachuFlavorText);
        }

        [Test]
        public void IfGivenNameIsHoOh_ShouldReturnHoOhInfo()
        {
            var pokemonInfo = _pokeApiClient.GetPokemonSpecies("ho-oh");

            pokemonInfo.Name.Should().Be("ho-oh");
            pokemonInfo.IsLegendary.Should().BeTrue();
            pokemonInfo.Habitat.Name.Should().Be("rare");
            GetFirstEnglishFlavorText(pokemonInfo).Should().Be(ExpectedHoOhFlavorText);
        }

        #region Utility Methods

        private static string GetFirstEnglishFlavorText(PokemonInfo pokemonInfo)
        {
            return pokemonInfo.FlavorTextEntries.First(f => f.Language.Name == "en").FlavorText;
        }

        #endregion
    }
}