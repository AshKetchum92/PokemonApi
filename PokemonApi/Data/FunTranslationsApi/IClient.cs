namespace PokemonApi.Data.FunTranslationsApi
{
    public interface IClient
    {
        /// <summary>
        /// Translate from English to Shakespeare
        /// </summary>
        Result TranslateToShakespeare(string text);

        /// <summary>
        /// Translate from English to Yoda
        /// </summary>
        Result TranslateToYoda(string text);
    }
}