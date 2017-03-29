using System;
using System.Threading;

namespace GBALink
{
    internal class Program
    {
        internal static GBAConnection Connection;
        internal static Random Random = new Random();

        static void Main(string[] args)
        {
            try {
                Connection = new GBAConnection("127.0.0.1", 8765);
            }catch(Exception)
            {
                Console.WriteLine("Could not connect, Retrying...");
                Main(args);
            }
            commandListenerMain();

        }

        static void commandListenerMain()
        {
            Console.WriteLine("[+] Now listening commands");
            while (true)
            {
                Thread.Sleep(250);

                string[] args = Console.ReadLine().Split(' ');
                switch (args[0])
                {
                    case ("mode"):
                        Connection.Mode = (Mode)int.Parse(args[1]);
                        Console.WriteLine("Mode changed to #{0}", Connection.Mode);
                        break;

                    case ("reset"):
                        Connection.Reset();
                        break;

                    case ("exit"):
                        Console.WriteLine("Bye!");
                        Connection.Dispose();
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
