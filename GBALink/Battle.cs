using GBALink;
using System;

namespace PokemonPacketCorruptor
{
    internal class Battle
    {
        internal BattleState State = BattleState.Wait;
        private short offset;

        internal byte[] Random = new byte[9];

        internal Trainer Trainer = new Trainer();
        internal Trainer Opponent = new Trainer();

        internal byte Process(GBAConnection con, byte data)
        {
            if (State == BattleState.TrainerData)
            {
                // Random Handling
                if (offset >= 34 && offset <= 42) // 0x110 - 0x150
                {
                    int s = offset - 34;

                    Random[s] += data;
                }

                // Name Handling
                if (offset >= 52 && offset <= 62) // 0x1A0 - 0x1F0
                {
                    int loc = offset - 52;
                    char c = CharacterHelper.ToChar(data);

                    if (c != '\0')
                        Opponent.Name += c;

                    if (Trainer.Name.Length > loc)
                        data = CharacterHelper.ToPokemonChar(Trainer.Name[loc]);
                    else if (Trainer.Name.Length == loc)
                        data = 0x50;
                    else
                        data = 0x00;
                }

                // Pokemon Team Handling
                if (offset >= 63 && offset <= 71) // 0x1F8 - 0x238
                {
                    int loc = offset - 63;

                    if (loc == 0)
                    {
                        Opponent.PokemonAmount = data;
                        data = Trainer.PokemonAmount;
                    }
                    else if (loc < 6)
                    {
                        Opponent.Pokemons[loc - 1].Id = data;
                        data = Trainer.Pokemons[loc - 1].Id;
                    }
                }

                // Pokemon infos
                if (offset >= 71 && offset <= 334) // 0x238 - 0xA70
                {
                    int pk = (int)Math.Floor((offset - 71) / 44d);

                    int s = (int)((offset - 71) % 44d);
                    switch (s)
                    {
                        case 0:
                            Opponent.Pokemons[pk].Id += data;
                            data = Trainer.Pokemons[pk].Id;
                            break;

                        case 1:
                            Opponent.Pokemons[pk].Hp += (short)(data * 0xFF);
                            data = (byte)(Math.Floor(Trainer.Pokemons[pk].Hp / 255d) * 0xFF);
                            break;

                        case 2:
                            Opponent.Pokemons[pk].Hp += data;
                            data = (byte)(Trainer.Pokemons[pk].Hp > 255 ? Trainer.Pokemons[pk].Hp - Math.Floor(Trainer.Pokemons[pk].Hp / 255d) * 0xFF : Trainer.Pokemons[pk].Hp);
                            break;

                        case 4:
                            Opponent.Pokemons[pk].Status = (PokemonStatus)data;
                            data = (byte)Trainer.Pokemons[pk].Status;
                            break;

                        case 5:
                            Opponent.Pokemons[pk].Type1 = (PokemonType)data;
                            data = (byte)Trainer.Pokemons[pk].Type1;
                            break;

                        case 6:
                            Opponent.Pokemons[pk].Type2 = (PokemonType)data;
                            data = (byte)Trainer.Pokemons[pk].Type2;
                            break;

                        case 8:
                            Opponent.Pokemons[pk].Move1 = (PokemonAttack)data;
                            data = (byte)Trainer.Pokemons[pk].Move1;
                            break;

                        case 9:
                            Opponent.Pokemons[pk].Move2 = (PokemonAttack)data;
                            data = (byte)Trainer.Pokemons[pk].Move2;
                            break;

                        case 10:
                            Opponent.Pokemons[pk].Move3 = (PokemonAttack)data;
                            data = (byte)Trainer.Pokemons[pk].Move3;
                            break;

                        case 11:
                            Opponent.Pokemons[pk].Move4 = (PokemonAttack)data;
                            data = (byte)Trainer.Pokemons[pk].Move4;
                            break;

                        case 12:
                        case 13:
                            Opponent.Pokemons[pk].TrainerId += data;

                            if (Trainer.Pokemons[pk].TrainerId > 0xFF)
                            {
                                if (s == 12)
                                    data = 0xFF;
                                else if (s == 13)
                                    data = (byte)(Trainer.Pokemons[pk].TrainerId - 0xFF);
                            }
                            else
                                data = (byte)Trainer.Pokemons[pk].TrainerId;
                            break;

                        case 14:
                        case 15:
                        case 16:
                            Opponent.Pokemons[pk].Experience += data;

                            if (Trainer.Pokemons[pk].Experience > 0xFF)
                            {
                                if (s == 14 || s == 15)
                                    data = 0xFF;
                                else if (s == 16)
                                    data = (byte)(Trainer.Pokemons[pk].Experience - 0xFF);
                            }
                            else
                                data = (byte)Trainer.Pokemons[pk].Experience;
                            break;

                        case 17:
                        case 18:
                            Opponent.Pokemons[pk].HpEv += data;

                            if (Trainer.Pokemons[pk].HpEv > 0xFF)
                            {
                                if (s == 17)
                                    data = 0xFF;
                                else if (s == 18)
                                    data = (byte)(Trainer.Pokemons[pk].HpEv - 0xFF);
                            }
                            else
                                data = (byte)Trainer.Pokemons[pk].HpEv;
                            break;

                        case 19:
                        case 20:
                            Opponent.Pokemons[pk].AttackEv += data;

                            if (Trainer.Pokemons[pk].AttackEv > 0xFF)
                            {
                                if (s == 19)
                                    data = 0xFF;
                                else if (s == 20)
                                    data = (byte)(Trainer.Pokemons[pk].AttackEv - 0xFF);
                            }
                            else
                                data = (byte)Trainer.Pokemons[pk].AttackEv;
                            break;

                        case 21:
                        case 22:
                            Opponent.Pokemons[pk].DefenseEv += data;

                            if (Trainer.Pokemons[pk].DefenseEv > 0xFF)
                            {
                                if (s == 22)
                                    data = 0xFF;
                                else if (s == 22)
                                    data = (byte)(Trainer.Pokemons[pk].DefenseEv - 0xFF);
                            }
                            else
                                data = (byte)Trainer.Pokemons[pk].DefenseEv;
                            break;

                        case 23:
                        case 24:
                            Opponent.Pokemons[pk].SpeedEv += data;

                            if (Trainer.Pokemons[pk].SpeedEv > 0xFF)
                            {
                                if (s == 23)
                                    data = 0xFF;
                                else if (s == 24)
                                    data = (byte)(Trainer.Pokemons[pk].SpeedEv - 0xFF);
                            }
                            else
                                data = (byte)Trainer.Pokemons[pk].SpeedEv;
                            break;

                        case 25:
                        case 26:
                            Opponent.Pokemons[pk].SpecialEv += data;

                            if (Trainer.Pokemons[pk].SpecialEv > 0xFF)
                            {
                                if (s == 25)
                                    data = 0xFF;
                                else if (s == 26)
                                    data = (byte)(Trainer.Pokemons[pk].SpecialEv - 0xFF);
                            }
                            else
                                data = (byte)Trainer.Pokemons[pk].SpecialEv;
                            break;

                        case 27:
                            Opponent.Pokemons[pk].AttackDefenseIv = data;
                            data = Trainer.Pokemons[pk].AttackDefenseIv;
                            break;

                        case 28:
                            Opponent.Pokemons[pk].SpeedSpecialIv = data;
                            data = Trainer.Pokemons[pk].SpeedSpecialIv;
                            break;

                        case 29:
                            Opponent.Pokemons[pk].PP1 = data;
                            data = Trainer.Pokemons[pk].PP1;
                            break;

                        case 30:
                            Opponent.Pokemons[pk].PP2 = data;
                            data = Trainer.Pokemons[pk].PP2;
                            break;

                        case 31:
                            Opponent.Pokemons[pk].PP3 = data;
                            data = Trainer.Pokemons[pk].PP3;
                            break;

                        case 32:
                            Opponent.Pokemons[pk].PP4 = data;
                            data = Trainer.Pokemons[pk].PP4;
                            break;

                        case 33:
                            Opponent.Pokemons[pk].Level = data;
                            data = Trainer.Pokemons[pk].Level;
                            break;

                        case 34:
                        case 35:
                            Opponent.Pokemons[pk].MaxHp += data;

                            if (Trainer.Pokemons[pk].MaxHp > 0xFF)
                            {
                                if (s == 34)
                                    data = 0xFF;
                                else if (s == 35)
                                    data = (byte)(Trainer.Pokemons[pk].MaxHp - 0xFF);
                            }
                            else
                                data = (byte)Trainer.Pokemons[pk].MaxHp;
                            break;

                        case 36:
                        case 37:
                            Opponent.Pokemons[pk].Attack += data;

                            if (Trainer.Pokemons[pk].Attack > 0xFF)
                            {
                                if (s == 34)
                                    data = 0xFF;
                                else if (s == 35)
                                    data = (byte)(Trainer.Pokemons[pk].Attack - 0xFF);
                            }
                            else
                                data = (byte)Trainer.Pokemons[pk].Attack;
                            break;

                        case 38:
                        case 39:
                            Opponent.Pokemons[pk].Defense += data;

                            if (Trainer.Pokemons[pk].Defense > 0xFF)
                            {
                                if (s == 34)
                                    data = 0xFF;
                                else if (s == 35)
                                    data = (byte)(Trainer.Pokemons[pk].Defense - 0xFF);
                            }
                            else
                                data = (byte)Trainer.Pokemons[pk].Defense;
                            break;

                        case 40:
                        case 41:
                            Opponent.Pokemons[pk].Speed += data;

                            if (Trainer.Pokemons[pk].Speed > 0xFF)
                            {
                                if (s == 34)
                                    data = 0xFF;
                                else if (s == 35)
                                    data = (byte)(Trainer.Pokemons[pk].Speed - 0xFF);
                            }
                            else
                                data = (byte)Trainer.Pokemons[pk].Speed;
                            break;

                        case 42:
                        case 43:
                            Opponent.Pokemons[pk].Special += data;

                            if (Trainer.Pokemons[pk].Special > 0xFF)
                            {
                                if (s == 34)
                                    data = 0xFF;
                                else if (s == 35)
                                    data = (byte)(Trainer.Pokemons[pk].Special - 0xFF);
                            }
                            else
                                data = (byte)Trainer.Pokemons[pk].Special;
                            break;
                    }
                }

                // Pokemon TrainerName Handling
                if (offset >= 335 && offset <= 395) // 0xA78 - 0xC58
                {
                    int loc = (offset - 335) % 10;
                    int s = (int)Math.Floor((offset - 335) / 10d);
                    char c = CharacterHelper.ToChar(data);

                    if (c != '\0')
                        Opponent.Pokemons[s].TrainerName += c;

                    if (Trainer.Name.Length > loc)
                        data = CharacterHelper.ToPokemonChar(Trainer.Name[loc]);
                    else if (Trainer.Name.Length == loc)
                        data = 0x50;
                    else
                        data = 0x00;
                }

                // Pokemon Name Handling
                if (offset >= 401 && offset < 461) // 0xC88 - 0xE68
                {
                    int loc = (offset - 401) % 10;
                    int s = (int)Math.Floor((offset - 401) / 10d);
                    char c = CharacterHelper.ToChar(data);

                    if (c != '\0')
                        Opponent.Pokemons[s].Name += c;

                    if (Trainer.Pokemons[s].Name.Length > loc)
                        data = CharacterHelper.ToPokemonChar(Trainer.Pokemons[s].Name[loc]);
                    else if (Trainer.Pokemons[s].Name.Length == loc)
                        data = 0x50;
                    else
                        data = 0x00;
                }

                offset++;

                if (offset == 671) // 0x14F0
                    Apply();
            }
            else if (State == BattleState.Fight)
            {
                data = ProcessData(data);
            }
            else if (State == BattleState.Wait)
            {
                // Not ready handler
                if (data == 0x60)
                {
                    return (byte)(AcceptTrainerData() ? 0x60 : 0xFE);
                }
                if (data == 0xFE)
                {
                    Console.WriteLine($"[#] [VS{Opponent.Name}] Downloading TrainerData...");
                    State = BattleState.TrainerData;
                }
            }

            return data;
        }

        protected virtual byte ProcessData(byte data)
        {
            return data;
        }

        protected virtual bool AcceptTrainerData()
        {
            return true;
        }

        internal void Apply()
        {
            Console.WriteLine($"[#] [VS{Opponent.Name}] Calculating...");

            Console.WriteLine($"[VS{Opponent.Name}] has a team of {string.Join(", ", Opponent.Pokemons[0].Name, Opponent.Pokemons[1].Name, Opponent.Pokemons[2].Name, Opponent.Pokemons[3].Name)}");
            State = BattleState.Fight;
        }
    }

    internal enum BattleState : byte
    {
        Wait,
        TrainerData,
        Fight
    }
}