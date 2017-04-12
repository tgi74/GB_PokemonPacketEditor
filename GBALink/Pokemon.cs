namespace PokemonPacketCorruptor
{
    public class Pokemon
    {
        public string Name = "";
        public string TrainerName = "";
        public byte Id;
        public short Hp;
        public PokemonStatus Status = PokemonStatus.Normal;
        public PokemonType Type1 = PokemonType.Normal;
        public PokemonType Type2 = PokemonType.Normal;
        public PokemonAttack Move1 = PokemonAttack.None;
        public PokemonAttack Move2 = PokemonAttack.None;
        public PokemonAttack Move3 = PokemonAttack.None;
        public PokemonAttack Move4 = PokemonAttack.None;
        public short TrainerId;
        public int Experience;
        public short HpEv;
        public short AttackEv;
        public short DefenseEv;
        public short SpeedEv;
        public short SpecialEv;
        public byte AttackDefenseIv;
        public byte SpeedSpecialIv;
        public byte PP1;
        public byte PP2;
        public byte PP3;
        public byte PP4;
        public byte Level;
        public short MaxHp;
        public short Attack;
        public short Defense;
        public short Speed;
        public short Special;

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