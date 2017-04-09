using PokemonPacketCorruptor;
using System;
using System.Net.Sockets;
using System.Threading;

namespace GBALink
{
    public class GBAConnection : IDisposable
    {
        public TcpClient Client;
        public NetworkStream Stream;
        private Thread receiveThread;

        public Stage Stage = Stage.Synchronization;
        public Mode Mode = Mode.Ai;
        private int skipPacketCount;

        public GBAConnection(string ip, int port)
        {
            Client = new TcpClient(ip, port);
            Stream = Client.GetStream();

            receiveThread = new Thread(receive);
            receiveThread.Start();
        }

        /// <summary>
        /// Write bytes to the NetworkStream
        /// </summary>
        /// <param name="content">content that will be writen</param>
        public void Send(byte[] content)
        {
            Stream.Write(content, 0, content.Length);
        }

        private int ticks;
        private int frames;

        private byte[] getStatus()
        {
            byte[] status = { 0x6A, 0, 0, 0, 0, 0, 0, 0 };

            ticks++;
            frames += 8;

            status[2] = BitConverter.GetBytes(ticks % 256d)[0];

            status[3] = BitConverter.GetBytes((ticks / 256d) % 256d)[0];

            status[5] = BitConverter.GetBytes(frames % 256d)[0];

            status[6] = BitConverter.GetBytes((frames / 256d) % 256d)[0];

            status[7] = BitConverter.GetBytes((frames / 256d / 256d) % 256d)[0];

            return status;
        }

        private void receive()
        {
            while (true)
            {
                byte[] bytes = new byte[8];
                int lenght = Stream.Read(bytes, 0, 8);

                if (lenght == 0)
                {
                    Console.WriteLine("[!] Connection closed");
                    break;
                }

                if (bytes[0] == 0x01)// "Who am I" Packet
                {
                    Reset();
                    Send(bytes);
                    Send(new byte[] { 0x6C, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    Send(new byte[] { 0x6C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                }
                if (bytes[0] == 0x6C)// Authentification packet
                {
                    Send(new byte[] { 0x6C, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    Send(getStatus());
                }
                if (bytes[0] == 0x6A)// Clock status packet
                {
                    Send(getStatus());
                }
                if (bytes[0] == 0x69 || bytes[0] == 0x68)// PK packets identificator
                {
                    ProcessPackets(bytes);// Do stuff to the packet

                    Send(bytes);// Send Packet

                    Send(getStatus());// Send Clock Status
                }
            }
        }

        public void Reset()
        {
#if MONITOR
            MonitorHelper.Reset("monitor.bin");
            MonitorHelper.Reset("action.bin");
#endif
            Console.WriteLine("[!] Reset detected");
            Stage = Stage.Synchronization;
            skipPacketCount = 0;
            battle = new Battle();
        }

        private Battle battle;

        public void ProcessPackets(byte[] bytes)
        {
            if (bytes == null || (bytes[0] != 0x69 && bytes[0] != 0x68)) return;

#if MONITOR
            MonitorHelper.Log(bytes, "monitor.bin");
#endif

            switch (Stage)
            {
                case (Stage.Synchronization):// Synchronization section
                    if (bytes[1] == 0x60)// Synchronization successfull packet
                    {
                        Console.WriteLine("[!] Synchronized");
                        Stage++;
                        bytes[1] = 0x00;
                        break;
                    }
                    if (bytes[1] == 0x61)// Useless packet
                    {
                        break;
                    }
                    bytes[1] = 0x02;// Answer to accept synchronization
                    break;

                case (Stage.ModeSelection):// Mode Selection section
                    if (bytes[1] == 0xD5)// Option selected packet
                    {
                        Console.WriteLine("[+] Option has been selected !");
                        Stage++;
                    }
                    break;

                case (Stage.TrainerData):// Trainer Data section

                    skipPacketCount++;
                    if (skipPacketCount < 3)// Skip packets that has be untouched
                        break;
                    skipPacketCount = 0;

                    Stage = Stage.Action;
                    break;

                case (Stage.Action):// Trigger Actions
#if MONITOR
                    MonitorHelper.Log(bytes, "action.bin");
#endif
                    switch (Mode)
                    {
                        case Mode.Corrupt:// Sends stable random data
                            Program.Random.NextBytes(bytes);
                            bytes[0] = 0x69;// PK Packets identifier
                            break;

                        case Mode.Mirror:// Leaves packets untouched
                            break;

                        case Mode.Monitor:// Logs all, leaves packets untouched
                            MonitorHelper.Log(bytes, "monitor.bin");
                            break;

                        case Mode.Ai:// Sends data over to the Battle instance
                            battle.Receive(this, bytes);
                            break;
                    }

                    break;
            }
        }

        public void Dispose()
        {
            Stream.Dispose();
            Client.Close();
            receiveThread.Abort();
        }
    }

    public enum Mode
    {
        Corrupt,
        Mirror,
        Monitor,
        Ai
    }

    public enum Stage
    {
        Synchronization,
        ModeSelection,
        TrainerData,
        Action,
    }
}