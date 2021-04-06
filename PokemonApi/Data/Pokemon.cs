using PokemonApi.Data.FunTranslationsApi;

namespace PokemonApi.Data
{
    public class Pokemon : IPokemon
    {
        private readonly IClient _funTranslationsClient;

        public Pokemon(IClient funTranslationsClient)
        {
            _funTranslationsClient = funTranslationsClient;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Habitat { get; set; }

        public bool IsLegendary { get; set; }

        public string TranslatedDescription => GetTranslatedDescription();

        #region Utility Methods

        private string GetTranslatedDescription()
        {
            if (Habitat == "cave" || IsLegendary)
            {
                return _funTranslationsClient.TranslateToShakespeare(Description).Contents.Translated;
            }

            return _funTranslationsClient.TranslateToYoda(Description).Contents.Translated;
        }

        #endregion
    }
}