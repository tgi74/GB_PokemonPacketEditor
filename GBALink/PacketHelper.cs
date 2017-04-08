namespace PokemonPacketCorruptor
{
    internal enum Action : byte
    {
        Attack1 = 0x60,
        Attack2 = 0x61,
        Attack3 = 0x62,
        Attack4 = 0x63,
        Switch1 = 0x64,
        Switch2 = 0x65,
        Switch3 = 0x66,
        Switch4 = 0x67
    }

    internal static class CharacterHelper
    {
        internal static byte ToPokemonChar(char c)
        {
            return (byte)(c + 63);
        }

        internal static char ToChar(byte c)
        {
            if (c == 0x50 || c == 0x00) return '\0';

            return (char)((char)c - 63);
        }
    }
}