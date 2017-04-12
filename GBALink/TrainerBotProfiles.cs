using GBALink;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.IO;

namespace PokemonPacketCorruptor
{
    internal static class TrainerBotProfiles
    {
        internal static Trainer Model = new Trainer
        {
            Name = "PokeAi",
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
                Move1 = PokemonAttack.Absorb,
                Move2 = PokemonAttack.Acid,
                Move3 = PokemonAttack.Agility,
                Move4 = PokemonAttack.AuroraBeam,
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
                Special = 15
            },
            new Pokemon(),
            new Pokemon(),
            new Pokemon(),
            new Pokemon(),
            new Pokemon()
        }
        };

        private static JsonSerializerSettings settings = new JsonSerializerSettings { Formatting = Formatting.Indented, Converters = new List<JsonConverter> { new StringEnumConverter { CamelCaseText = true } } };

        internal static Trainer LoadRandomTrainerProfile()
        {
            string[] ss = Directory.GetFiles(@"profiles\");

            return LoadTrainerProfile(ss[Program.Random.Next(0, ss.Length)]);
        }

        internal static Trainer LoadTrainerProfile(string path)
        {
            return JsonConvert.DeserializeObject<Trainer>(File.ReadAllText(path), settings);
        }

        internal static void SaveTrainerProfile(Trainer t, string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            File.WriteAllText(path, JsonConvert.SerializeObject(t, settings));
        }
    }
}