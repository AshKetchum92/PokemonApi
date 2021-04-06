using System;
using System.Net;
using PokemonApi.Utils;

namespace PokemonApi.Data.PokeApi
{
    public class Client : JsonClientBase, IClient
    {
        public Client() : base("https://pokeapi.co/api/v2/")
        {

        }

        public PokemonInfo GetPokemonSpecies(string name)
        {
            try
            {
                return Get<PokemonInfo>($"pokemon-species/{name}/");
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
                {
                    throw new ArgumentOutOfRangeException(nameof(name));
                }

                throw;
            }
        }
    }
}