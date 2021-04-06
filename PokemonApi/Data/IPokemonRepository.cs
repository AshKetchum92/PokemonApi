using System;

namespace PokemonApi.Data
{
    public interface IPokemonRepository
    {
        /// <summary>
        /// Returns the pokemon info of the given pokemon name
        /// </summary>
        /// <exception cref="ArgumentNullException">If given <param name="name"></param> is null</exception>
        /// <exception cref="ArgumentException">If given <param name="name"></param> is empty</exception>
        /// <exception cref="ArgumentOutOfRangeException">If given <param name="name"></param> does not belong to any pokemon</exception>
        IPokemon Get(string name);
    }
}