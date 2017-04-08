using System.Collections.Generic;
using System.IO;

namespace PokemonPacketCorruptor
{
    public static class MonitorHelper
    {
        public static void Log(IEnumerable<byte[]> bytes, string file = "packets.bin")
        {
            foreach (byte[] bs in bytes)
                Log(bs, file);
        }

        public static void Log(byte[] bytes, string file = "packets.bin")
        {
            var s = File.OpenWrite(file);
            s.Position = s.Length;
            s.Write(bytes, 0, bytes.Length);
            s.Close();
        }
    }
}