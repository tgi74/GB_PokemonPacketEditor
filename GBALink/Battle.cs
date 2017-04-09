using GBALink;
using System;
using System.Collections.Generic;

namespace PokemonPacketCorruptor
{
    internal class Battle
    {
        private bool isPullingTrainerData = true;
        private short offset;
        private readonly List<byte[]> sectionBuffer = new List<byte[]>();

        internal byte[] Random = new byte[9];

        internal Trainer Bot = TrainerBotProfiles.Trainer1;
        internal Trainer Opponent = new Trainer();
        internal AiAction Ai = new AiAction();

        internal void Receive(GBAConnection con, byte[] bytes)
        {
            if (isPullingTrainerData)
            {
                sectionBuffer.Add(bytes);

                // Random Handling
                if (offset >= 34 && offset <= 42) // 0x110 - 0x150
                {
                    int s = offset - 34;

                    Random[s] += bytes[1];
                }

                // Name Handling
                if (offset >= 52 && offset <= 62) // 0x1A0 - 0x1F0
                {
                    int loc = offset - 52;
                    char c = CharacterHelper.ToChar(bytes[1]);

                    if (c != '\0')
                        Opponent.Name += c;

                    if (Bot.Name.Length > loc)
                        bytes[1] = CharacterHelper.ToPokemonChar(Bot.Name[loc]);
                    else if (Bot.Name.Length == loc)
                        bytes[1] = 0x50;
                    else
                        bytes[1] = 0x00;
                }

                // Pokemon Team Handling
                if (offset >= 63 && offset <= 71) // 0x1F8 - 0x238
                {
                    int loc = offset - 63;

                    if (loc == 0)
                    {
                        Opponent.PokemonAmount = bytes[1];
                        bytes[1] = Bot.PokemonAmount;
                    }
                    else if (loc < 6)
                    {
                        Opponent.Pokemons[loc - 1].Id = bytes[1];
                        bytes[1] = Bot.Pokemons[loc - 1].Id;
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
                            Opponent.Pokemons[pk].Id += bytes[1];
                            bytes[1] = Bot.Pokemons[pk].Id;
                            break;

                        case 1:
                            Opponent.Pokemons[pk].Hp += (short)(bytes[1] * 0xFF);
                            bytes[1] = (byte)(Math.Floor(Bot.Pokemons[pk].Hp / 255d) * 0xFF);
                            break;

                        case 2:
                            Opponent.Pokemons[pk].Hp += bytes[1];
                            bytes[1] = (byte)(Bot.Pokemons[pk].Hp > 255 ? Bot.Pokemons[pk].Hp - Math.Floor(Bot.Pokemons[pk].Hp / 255d) * 0xFF : Bot.Pokemons[pk].Hp);
                            break;

                        case 4:
                            Opponent.Pokemons[pk].Status = (PokemonStatus)bytes[1];
                            bytes[1] = (byte)Bot.Pokemons[pk].Status;
                            break;

                        case 5:
                            Opponent.Pokemons[pk].Type1 = (PokemonType)bytes[1];
                            bytes[1] = (byte)Bot.Pokemons[pk].Type1;
                            break;

                        case 6:
                            Opponent.Pokemons[pk].Type2 = (PokemonType)bytes[1];
                            bytes[1] = (byte)Bot.Pokemons[pk].Type2;
                            break;

                        case 8:
                            Opponent.Pokemons[pk].Move1 = (PokemonAttack)bytes[1];
                            bytes[1] = (byte)Bot.Pokemons[pk].Move1;
                            break;

                        case 9:
                            Opponent.Pokemons[pk].Move2 = (PokemonAttack)bytes[1];
                            bytes[1] = (byte)Bot.Pokemons[pk].Move2;
                            break;

                        case 10:
                            Opponent.Pokemons[pk].Move3 = (PokemonAttack)bytes[1];
                            bytes[1] = (byte)Bot.Pokemons[pk].Move3;
                            break;

                        case 11:
                            Opponent.Pokemons[pk].Move4 = (PokemonAttack)bytes[1];
                            bytes[1] = (byte)Bot.Pokemons[pk].Move4;
                            break;

                        case 12:
                        case 13:
                            Opponent.Pokemons[pk].TrainerId += bytes[1];

                            if (Bot.Pokemons[pk].TrainerId > 0xFF)
                            {
                                if (s == 12)
                                    bytes[1] = 0xFF;
                                else if (s == 13)
                                    bytes[1] = (byte)(Bot.Pokemons[pk].TrainerId - 0xFF);
                            }
                            else
                                bytes[1] = (byte)Bot.Pokemons[pk].TrainerId;
                            break;

                        case 14:
                        case 15:
                        case 16:
                            Opponent.Pokemons[pk].Experience += bytes[1];

                            if (Bot.Pokemons[pk].Experience > 0xFF)
                            {
                                if (s == 14 || s == 15)
                                    bytes[1] = 0xFF;
                                else if (s == 16)
                                    bytes[1] = (byte)(Bot.Pokemons[pk].Experience - 0xFF);
                            }
                            else
                                bytes[1] = (byte)Bot.Pokemons[pk].Experience;
                            break;

                        case 17:
                        case 18:
                            Opponent.Pokemons[pk].HpEv += bytes[1];

                            if (Bot.Pokemons[pk].HpEv > 0xFF)
                            {
                                if (s == 17)
                                    bytes[1] = 0xFF;
                                else if (s == 18)
                                    bytes[1] = (byte)(Bot.Pokemons[pk].HpEv - 0xFF);
                            }
                            else
                                bytes[1] = (byte)Bot.Pokemons[pk].HpEv;
                            break;

                        case 19:
                        case 20:
                            Opponent.Pokemons[pk].AttackEv += bytes[1];

                            if (Bot.Pokemons[pk].AttackEv > 0xFF)
                            {
                                if (s == 19)
                                    bytes[1] = 0xFF;
                                else if (s == 20)
                                    bytes[1] = (byte)(Bot.Pokemons[pk].AttackEv - 0xFF);
                            }
                            else
                                bytes[1] = (byte)Bot.Pokemons[pk].AttackEv;
                            break;

                        case 21:
                        case 22:
                            Opponent.Pokemons[pk].DefenseEv += bytes[1];

                            if (Bot.Pokemons[pk].DefenseEv > 0xFF)
                            {
                                if (s == 22)
                                    bytes[1] = 0xFF;
                                else if (s == 22)
                                    bytes[1] = (byte)(Bot.Pokemons[pk].DefenseEv - 0xFF);
                            }
                            else
                                bytes[1] = (byte)Bot.Pokemons[pk].DefenseEv;
                            break;

                        case 23:
                        case 24:
                            Opponent.Pokemons[pk].SpeedEv += bytes[1];

                            if (Bot.Pokemons[pk].SpeedEv > 0xFF)
                            {
                                if (s == 23)
                                    bytes[1] = 0xFF;
                                else if (s == 24)
                                    bytes[1] = (byte)(Bot.Pokemons[pk].SpeedEv - 0xFF);
                            }
                            else
                                bytes[1] = (byte)Bot.Pokemons[pk].SpeedEv;
                            break;

                        case 25:
                        case 26:
                            Opponent.Pokemons[pk].SpecialEv += bytes[1];

                            if (Bot.Pokemons[pk].SpecialEv > 0xFF)
                            {
                                if (s == 25)
                                    bytes[1] = 0xFF;
                                else if (s == 26)
                                    bytes[1] = (byte)(Bot.Pokemons[pk].SpecialEv - 0xFF);
                            }
                            else
                                bytes[1] = (byte)Bot.Pokemons[pk].SpecialEv;
                            break;

                        case 27:
                            Opponent.Pokemons[pk].AttackDefenseIv = bytes[1];
                            bytes[1] = Bot.Pokemons[pk].AttackDefenseIv;
                            break;

                        case 28:
                            Opponent.Pokemons[pk].SpeedSpecialIv = bytes[1];
                            bytes[1] = Bot.Pokemons[pk].SpeedSpecialIv;
                            break;

                        case 29:
                            Opponent.Pokemons[pk].PP1 = bytes[1];
                            bytes[1] = Bot.Pokemons[pk].PP1;
                            break;

                        case 30:
                            Opponent.Pokemons[pk].PP2 = bytes[1];
                            bytes[1] = Bot.Pokemons[pk].PP2;
                            break;

                        case 31:
                            Opponent.Pokemons[pk].PP3 = bytes[1];
                            bytes[1] = Bot.Pokemons[pk].PP3;
                            break;

                        case 32:
                            Opponent.Pokemons[pk].PP4 = bytes[1];
                            bytes[1] = Bot.Pokemons[pk].PP4;
                            break;

                        case 33:
                            Opponent.Pokemons[pk].Level = bytes[1];
                            bytes[1] = Bot.Pokemons[pk].Level;
                            break;

                        case 34:
                        case 35:
                            Opponent.Pokemons[pk].MaxHp += bytes[1];

                            if (Bot.Pokemons[pk].MaxHp > 0xFF)
                            {
                                if (s == 34)
                                    bytes[1] = 0xFF;
                                else if (s == 35)
                                    bytes[1] = (byte)(Bot.Pokemons[pk].MaxHp - 0xFF);
                            }
                            else
                                bytes[1] = (byte)Bot.Pokemons[pk].MaxHp;
                            break;

                        case 36:
                        case 37:
                            Opponent.Pokemons[pk].Attack += bytes[1];

                            if (Bot.Pokemons[pk].Attack > 0xFF)
                            {
                                if (s == 34)
                                    bytes[1] = 0xFF;
                                else if (s == 35)
                                    bytes[1] = (byte)(Bot.Pokemons[pk].Attack - 0xFF);
                            }
                            else
                                bytes[1] = (byte)Bot.Pokemons[pk].Attack;
                            break;

                        case 38:
                        case 39:
                            Opponent.Pokemons[pk].Defense += bytes[1];

                            if (Bot.Pokemons[pk].Defense > 0xFF)
                            {
                                if (s == 34)
                                    bytes[1] = 0xFF;
                                else if (s == 35)
                                    bytes[1] = (byte)(Bot.Pokemons[pk].Defense - 0xFF);
                            }
                            else
                                bytes[1] = (byte)Bot.Pokemons[pk].Defense;
                            break;

                        case 40:
                        case 41:
                            Opponent.Pokemons[pk].Speed += bytes[1];

                            if (Bot.Pokemons[pk].Speed > 0xFF)
                            {
                                if (s == 34)
                                    bytes[1] = 0xFF;
                                else if (s == 35)
                                    bytes[1] = (byte)(Bot.Pokemons[pk].Speed - 0xFF);
                            }
                            else
                                bytes[1] = (byte)Bot.Pokemons[pk].Speed;
                            break;

                        case 42:
                        case 43:
                            Opponent.Pokemons[pk].Special += bytes[1];

                            if (Bot.Pokemons[pk].Special > 0xFF)
                            {
                                if (s == 34)
                                    bytes[1] = 0xFF;
                                else if (s == 35)
                                    bytes[1] = (byte)(Bot.Pokemons[pk].Special - 0xFF);
                            }
                            else
                                bytes[1] = (byte)Bot.Pokemons[pk].Special;
                            break;
                    }
                }

                // Pokemon TrainerName Handling
                if (offset >= 335 && offset <= 395) // 0xA78 - 0xC58
                {
                    int loc = (offset - 335) % 10;
                    int s = (int)Math.Floor((offset - 335) / 10d);
                    char c = CharacterHelper.ToChar(bytes[1]);

                    if (c != '\0')
                        Opponent.Pokemons[s].TrainerName += c;

                    if (Bot.Name.Length > loc)
                        bytes[1] = CharacterHelper.ToPokemonChar(Bot.Name[loc]);
                    else if (Bot.Name.Length == loc)
                        bytes[1] = 0x50;
                    else
                        bytes[1] = 0x00;
                }

                // Pokemon Name Handling
                if (offset >= 401 && offset < 461) // 0xC88 - 0xE68
                {
                    int loc = (offset - 401) % 10;
                    int s = (int)Math.Floor((offset - 401) / 10d);
                    char c = CharacterHelper.ToChar(bytes[1]);

                    if (c != '\0')
                        Opponent.Pokemons[s].Name += c;

                    if (Bot.Pokemons[s].Name.Length > loc)
                        bytes[1] = CharacterHelper.ToPokemonChar(Bot.Pokemons[s].Name[loc]);
                    else if (Bot.Pokemons[s].Name.Length == loc)
                        bytes[1] = 0x50;
                    else
                        bytes[1] = 0x00;
                }

                offset++;

                if (offset == 671) // 0x14F0
                    Apply();
            }
            else
            {
                Ai.OnReceive(this, bytes);
            }
        }

        internal void Apply()
        {
            Console.WriteLine("Calulating...");

            Console.WriteLine("Welcome to " + Opponent.Name);
            Console.WriteLine($"Team of {string.Join(", ", Opponent.Pokemons[0].Name, Opponent.Pokemons[1].Name, Opponent.Pokemons[2].Name, Opponent.Pokemons[3].Name)}");
            isPullingTrainerData = false;
        }
    }
}