namespace PokemonPacketCorruptor
{
    internal static class TrainerBotProfiles
    {
        internal static Trainer Trainer1 = new Trainer
        {
            Name = "PokeAi1",
            PokemonAmount = 1,
            Pokemons = new Pokemon[] {
            new Pokemon{
                Name = "TheTest",
                TrainerName = "Tgi",
                Id = 1,
                Hp = 200,
                Status = PokemonStatus.Normal,
                Type1 = PokemonType.Bug,
                Type2 = PokemonType.Normal,
                Move1 = PokemonAttack.Bide,
                Move2 = PokemonAttack.Bide,
                Move3 = PokemonAttack.Bide,
                Move4 = PokemonAttack.Bide,
                TrainerId = 1,
                Experience = 50,
                HpEv = 15,
                AttackEv = 15,
                DefenseEv = 15,
                SpeedEv = 15,
                SpecialEv = 15,
                AttackDefenseIv = 15,
                SpeedSpecialIv = 15,
                PP1 = 5,
                PP2 = 5,
                PP3 = 5,
                PP4 = 5,
                Level = 60,
                MaxHp = 200,
                Attack = 15,
                Defense = 15,
                Speed = 15,
                Special = 15,
            },
            new Pokemon(),
            new Pokemon(),
            new Pokemon(),
            new Pokemon(),
            new Pokemon()
        }
        };
    }
}