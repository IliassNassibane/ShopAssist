using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net;

namespace ShopAssist_P2Pmodule.P2P
{
    /// <summary>
    /// A Peer-to-Peer client service class with methods for sending and sharing data to
    /// other Shop Assist users.
    /// 
    /// Prerequisites:
    /// - A connected TcpConnection to a server
    /// </summary>
    public static class ClientService
    {
        public static TcpClient ConnectToServer(string ip, int port)
        {
            return new TcpClient(IPAddress.Parse(ip).ToString(), port);
        }

        /// <summary>
        /// Sends data through a NetworkStream from a connected TcpConnection, 
        /// i.e. Send(client.GetStream(), "Message");
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="message"></param>
        public static void Send(NetworkStream stream, string message) 
        {
            try
            {
                byte[] data = Encoding.Default.GetBytes(message);
                byte[] dataLength = BitConverter.GetBytes(data.Length);
                stream.Write(dataLength, 0, 4);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Receives data from a TcpConnection through a NetworkStream,
        /// i.e. Receive(client.GetStream());
        /// </summary>
        /// <param name="stream"></param>
        public static void Receive(NetworkStream stream) 
        {
            try
            {
                int i;
                byte[] dataLength = new byte[4];

                new Thread(() => {
                    while ((i = stream.Read(dataLength, 0, 4)) != 0)
                    {
                        byte[] data = new byte[BitConverter.ToInt32(dataLength, 0)];
                        stream.Read(data, 0, data.Length);
                    }
                }).Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}