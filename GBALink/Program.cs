using System;
using System.Diagnostics;
using System.Threading;

namespace GBALink
{
    internal class Program
    {
        internal static GBAConnection Connection;
        internal static Random Random = new Random();

        private static void Main(string[] args)
        {
            try
            {
                Connection = new GBAConnection("127.0.0.1", 8765);
            }
            catch (Exception)
            {
                Console.WriteLine("Could not connect, Retrying...");
                Main(args);
            }
            commandListenerMain();
        }

        private static void commandListenerMain()
        {
            Console.WriteLine("[+] Now listening commands");
            while (true)
            {
                Thread.Sleep(250);

                var args = Console.ReadLine().Split(' ');
                switch (args[0])
                {
                    case ("mode"):
                        Mode? newMode;
                        try
                        {
                            newMode = (Mode)Enum.Parse(typeof(Mode), args[1], true);
                        }
                        catch (Exception)
                        {
                            newMode = null;
                        }
                        Debug.Assert(newMode != null, $"Incorrect mode detected: {args[1]}");
                        Connection.Mode = newMode.Value;
                        Console.WriteLine($"Mode changed to #{Connection.Mode}");
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