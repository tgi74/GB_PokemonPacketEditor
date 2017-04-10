using System;
using System.Collections.Generic;

namespace PokemonPacketCorruptor
{
    internal class AiAction
    {
        internal Battle battle;
        private readonly Random r = new Random();
        private Action? action;

        internal List<byte> Buffer = new List<byte>();

        internal byte Process(Battle bat, byte data)
        {
            battle = bat;
            Buffer.Add(data);

            if (Buffer.Count == 23)
                Compute();

            if (data != 0x00 && action.HasValue)
            {
                data = (byte)action;
            }

            return data;
        }

        internal void Compute()
        {
            var action = (Action)Buffer[1];
            Console.WriteLine($"[VS{battle.Opponent.Name}] Opponent Action is {action}");

            MonitorHelper.Log(Buffer, "ai.bin");

            if (r.Next(0, 10) != 1)
                Attack();
            else
                Switch();

            // Buffer reset
            Buffer.Clear();
        }

        internal void Attack()
        {
            short tries = 0;
            byte index = 0;
            PokemonAttack attack = PokemonAttack.None;
            while (attack == PokemonAttack.None)
            {
                tries++;
                index = (byte)r.Next(0, 4);
                attack = battle.Bot.Pokemons[battle.Bot.CurrentPokemonIndex].MoveFromId(index);

                if (tries > 20)
                    break;
            }

            action = Action.Attack1 + index;
            Console.WriteLine($"[VS{battle.Opponent.Name}] Bot Attacked #{battle.Bot.CurrentPokemonIndex} ({battle.Bot.Pokemons[battle.Bot.CurrentPokemonIndex].Name}) with {attack}#{index}");
        }

        internal void Switch()
        {
            byte index = (byte)r.Next(0, battle.Bot.PokemonAmount + 1);

            battle.Bot.CurrentPokemonIndex = index;
            action = Action.Switch1 + index;

            Console.WriteLine($"[VS{battle.Opponent.Name}] Bot Switched to {battle.Bot.Pokemons[battle.Bot.CurrentPokemonIndex].Name}#{index}");
        }
    }
}