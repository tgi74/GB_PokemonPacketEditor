using GBALink;
using System;
using System.Collections.Generic;

namespace PokemonPacketCorruptor
{
    class AiAction
    {
        Action action = 0x00;

        internal List<byte[]> Bytes = new List<byte[]>();
        internal byte[] Response { get { return new byte[8]; } }

        internal void OnReceive(GBAConnection co, byte[] bytes)
        {
            Bytes.Add(bytes);

            if (Bytes.Count == 23)
                Compute();
        }

        internal void Compute()
        {
            foreach (byte[] b in Bytes)
                Console.Write(b[1] + " ");

            Console.WriteLine("\nAttack: " + (Action)Bytes[1][1]);

            Bytes.Clear();

            action = (Action)new Random().Next((int)Action.Attack1, (int)Action.Switch4);
        }
    }
}
