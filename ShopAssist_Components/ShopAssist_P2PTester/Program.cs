using System;
using System.Net;
using System.Net.Sockets;

namespace ShopAssist_P2PTester
{
    class Program
    {
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            LoopConnect();
            Console.ReadLine();
        }

        private static void LoopConnect()
        {
            int attempts = 0;

            while (!_clientSocket.Connected)
            {
                // Voor nu een loopback op dezelfde IP, later een IP opvragen...
                try
                {
                    attempts++;
                    _clientSocket.Connect(IPAddress.Loopback, 5000);
                }
                catch (SocketException)
                {
                    Console.Clear();
                    Console.WriteLine($"Connection attempts: {attempts.ToString()}");
                }
            }

            Console.Clear();
            Console.WriteLine("Connected");
        }
    }
}
