using FluentAssertions;
using NUnit.Framework;
using PokemonApi.Data.FunTranslationsApi;

namespace PokemonApiTest.Data.FunTranslationsApi
{
    [TestFixture]
    public class ClientTest
    {
        private Client _funTranslationsClient;

        [SetUp]
        public void Setup()
        {
            _funTranslationsClient = new Client();
        }

        [Test]
        public void TranslateToShakespeare_ShouldTranslateFromEnglishToShakespeare()
        {
            var result = _funTranslationsClient.TranslateToShakespeare("You gave Mr. Tim a hearty meal, but unfortunately what he ate made him die.");

            result.Contents.Translated.Should().Be("Thee did giveth mr. Tim a hearty meal,  but unfortunately what he did doth englut did maketh him kicketh the bucket.");
        }

        [Test]
        public void TranslateToYoda_ShouldTranslateFromEnglishToYoda()
        {
            var result = _funTranslationsClient.TranslateToYoda("Master Obiwan has lost a planet.");

            result.Contents.Translated.Should().Be("Lost a planet,  master obiwan has.");
        }
    }
}