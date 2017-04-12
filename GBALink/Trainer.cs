namespace PokemonPacketCorruptor
{
    public class Trainer
    {
        public string Name;
        public byte PokemonAmount;
        public Pokemon[] Pokemons = { new Pokemon(), new Pokemon(), new Pokemon(), new Pokemon(), new Pokemon(), new Pokemon() };
        public byte CurrentPokemonIndex;
    }
}