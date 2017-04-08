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

        internal const string Name = "PkAi";
        internal string OpponentName;
        internal AiAction Ai = new AiAction();

        internal void Receive(GBAConnection con, byte[] bytes)
        {
            if (isPullingTrainerData)
            {
                sectionBuffer.Add(bytes);

                // Name Handling
                if (offset >= 52 && offset <= 62)
                {
                    int loc = offset - 52;

                    OpponentName += CharacterHelper.ToChar(bytes[1]);

                    if (Name.Length > loc)
                        bytes[1] = CharacterHelper.ToPokemonChar(Name[loc]);
                    else if (Name.Length == loc)
                        bytes[1] = 0x50;
                    else
                        bytes[1] = 0x00;
                }

                offset++;

                if (offset == 671)
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

            Console.WriteLine("Welcome to " + OpponentName);
            isPullingTrainerData = false;
        }
    }
}