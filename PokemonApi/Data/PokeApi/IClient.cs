using System;

namespace PokemonApi.Data.PokeApi
{
    public interface IClient
    {
        /// <summary>
        /// Returns the pokemon species info of the given pokemon name
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">If given <param name="name"></param> does not belong to any pokemon species</exception>
        PokemonInfo GetPokemonSpecies(string name);
    }
}