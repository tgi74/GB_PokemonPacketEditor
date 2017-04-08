using GBALink;
using System;
using System.Collections.Generic;

namespace PokemonPacketCorruptor
{
    internal class AiAction
    {
        internal Battle battle;
        private Action? action;

        internal List<byte[]> Buffer = new List<byte[]>();

        internal void OnReceive(Battle bat, byte[] bytes)
        {
            battle = bat;
            Buffer.Add(bytes);

            if (Buffer.Count == 23)
                Compute();

            if (bytes[1] != 0x00 && action.HasValue)
            {
                bytes[1] = (byte)action;
            }
        }

        internal void Compute()
        {
            /* ========== //
            // Infos:
            // 00: 0x68 / 0x69
            // 01: ActionByte
            // 02: 0x81
            // 03: 0x00
            // 04: ? ends with '0' (Action informations?) (0x00: Unknown, 0x40: Attacks works, 0x60: Unknown, 0xC0: Unknown(con. crash)
            // 05: ?
            // 06: Info Block delimiter (increments on each begining)
            // 07: Change on each new Action
            // ========== */

            Console.WriteLine($"\nOpponent Attack: {(Action)Buffer[1][1]}");

            MonitorHelper.Log(Buffer, $"ai-{battle.OpponentName}.bin");

            action = (Action)new Random().Next((byte)Action.Attack1, (byte)Action.Switch4);
            Console.WriteLine($"\nNext Ai Attack: {action}");

            // Buffer reset
            Buffer.Clear();
        }
    }
}