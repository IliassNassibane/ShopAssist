using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopAssist_P2Pmodule
{
    class _1stAttempt
    {
        /*
        private static byte[] _buffer = new byte[1024];
        private static List<Socket> _clientSockets = new List<Socket>();
        private static Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static TextView textViewConsole;
        private static string outputToConsole = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            textViewConsole = (TextView)FindViewById(Resource.Id.TextConsole);

            SetupServer();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private static void SetupServer()
        {
            OutputToConsole("\nSetting up server...");
            _socket.Bind(new IPEndPoint(IPAddress.Any, 5000));
            OutputToConsole($"\nIP address: {(string)_socket.LocalEndPoint.ToString()}");
            _socket.Listen(100);
            _socket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
        }

        private static void AcceptCallBack(IAsyncResult aResult)
        {
            Socket socket = _socket.EndAccept(aResult);
            _clientSockets.Add(socket);
            OutputToConsole("\nClient connected...");
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            _socket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
        }

        private static void ReceiveCallback(IAsyncResult aResult)
        {
            Socket socket = (Socket)aResult.AsyncState;
            int received = socket.EndReceive(aResult);
            byte[] dataBuff = new byte[received];
            Array.Copy(_buffer, dataBuff, received);

            string text = Encoding.ASCII.GetString(dataBuff);
            OutputToConsole($"\nText received {text}");

            string response = text.ToLower() != "get time" ? "Invalid Request" : DateTime.Now.ToLongTimeString();

            byte[] data = Encoding.ASCII.GetBytes(response);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallBack), null);

            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
        }

        private static void SendCallBack(IAsyncResult aResult)
        {
            Socket socket = (Socket)aResult.AsyncState;
            socket.EndSend(aResult);
        }

        private static void OutputToConsole(string input)
        {
            textViewConsole.SetText(input.ToCharArray(), 0, input.Length);
        }
        */
    }
}