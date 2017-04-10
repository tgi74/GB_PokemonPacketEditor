using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GBALink
{
    internal class Program
    {
        internal static List<GBAConnection> Connections = new List<GBAConnection>();
        internal static TcpListener Listener;
        internal static Random Random = new Random();

        private static Thread listenerThread;

        private static void Main(string[] args)
        {
            try
            {
                Listener = new TcpListener(IPAddress.Any, 8765);
                Listener.Start();
                Console.WriteLine("[#] Listener Initialized !");
            }
            catch (Exception)
            {
                Console.WriteLine("[!] Could not connect, Retrying...");
                Main(args);
            }
            listenerThread = new Thread(listener);
            listenerThread.Start();
            commandListenerMain();
        }

        private static void listener()
        {
            Console.WriteLine("[#] Listening !");

            try
            {
                while (true)
                    try
                    {
                        Thread.Sleep(10);
                        Connections.Add(new GBAConnection(Listener.AcceptTcpClient()));
                        Console.WriteLine("[#] Handshake");
                    }
                    catch
                    {
                    }
            }
            catch (ThreadAbortException)
            {
                return;
            }
        }

        private static void commandListenerMain()
        {
            Console.WriteLine("[#] Now listening commands");
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
                        Connections.ForEach((g) => { g.Mode = newMode.Value; });
                        Console.WriteLine($"[!] Mode changed to #{newMode}");
                        break;

                    case ("reset"):
                        Connections.ForEach((g) => { g.Reset(); });
                        break;

                    case ("exit"):
                        Console.WriteLine("[-] Bye!");
                        Connections.ForEach((g) => { g.Dispose(); });
                        Connections.Clear();
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}