using PokemonApi.Utils;

namespace PokemonApi.Data.FunTranslationsApi
{
    public class Client : JsonClientBase, IClient
    {
        public Client() : base("https://api.funtranslations.com/translate")
        {

        }

        public Result TranslateToShakespeare(string text)
        {
            return Post<Request, Result>("shakespeare.json", new Request
            {
                Text = text
            });
        }

        public Result TranslateToYoda(string text)
        {
            return Post<Request, Result>("yoda.json", new Request
            {
                Text = text
            });
        }
    }
}