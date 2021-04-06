namespace PokemonApi.Data
{
    public interface IPokemon
    {
        public string Name { get; }

        public string Description { get; }
        
        public string Habitat { get; }
        
        public bool IsLegendary { get; }

        public string TranslatedDescription { get; }
    }
}