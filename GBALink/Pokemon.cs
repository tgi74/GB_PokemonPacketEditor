namespace PokemonPacketCorruptor
{
    internal class Pokemon
    {
        internal string Name = "";
        internal string TrainerName = "";
        internal byte Id;
        internal short Hp;
        internal PokemonStatus Status = PokemonStatus.Normal;
        internal PokemonType Type1 = PokemonType.Normal;
        internal PokemonType Type2 = PokemonType.Normal;
        internal PokemonAttack Move1 = PokemonAttack.None;
        internal PokemonAttack Move2 = PokemonAttack.None;
        internal PokemonAttack Move3 = PokemonAttack.None;
        internal PokemonAttack Move4 = PokemonAttack.None;
        internal short TrainerId;
        internal int Experience;
        internal short HpEv;
        internal short AttackEv;
        internal short DefenseEv;
        internal short SpeedEv;
        internal short SpecialEv;
        internal byte AttackDefenseIv;
        internal byte SpeedSpecialIv;
        internal byte PP1;
        internal byte PP2;
        internal byte PP3;
        internal byte PP4;
        internal byte Level;
        internal short MaxHp;
        internal short Attack;
        internal short Defense;
        internal short Speed;
        internal short Special;

        internal PokemonAttack MoveFromId(byte id)
        {
            switch (id)
            {
                case 0:
                    return Move1;

                case 1:
                    return Move2;

                case 2:
                    return Move3;

                case 3:
                    return Move4;
            }

            return PokemonAttack.None;
        }
    }
}