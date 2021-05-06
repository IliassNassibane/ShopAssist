using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
//using Java.Net; // ServerSocket
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopAssist_P2Pmodule
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        // Voorbeeld uit: https://forums.xamarin.com/discussion/1576/need-simple-tcp-client-example

        //int i;
        public static TcpClient client;
        public static TcpListener server;
        public static bool keepListeningFlag = false;
        public NetworkStream stream;
        
        public static IPAddress localIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
        public static IPEndPoint ipLocalEndPoint = new IPEndPoint(localIP, 0);

        EditText ServerIPEditText;
        EditText ServerPortEditText;
        Button ListenBtn;
        EditText ClientIPEditText;
        EditText ClientPortEditText;
        Button ConnectBtn;
        TextView ConsoleTextView;
        EditText MessageEditText;
        Button SendBtn;

        // INIT
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Server setup
            ServerIPEditText = FindViewById<EditText>(Resource.Id.ServerIPEditText);
            ServerIPEditText.Text = localIP.ToString();
            ServerIPEditText.Enabled = false;
            ServerPortEditText = FindViewById<EditText>(Resource.Id.ServerPortEditText);
            ListenBtn = FindViewById<Button>(Resource.Id.ListenBtn);

            // Client setup
            ClientIPEditText = FindViewById<EditText>(Resource.Id.ClientIPEditText);
            ClientPortEditText = FindViewById<EditText>(Resource.Id.ClientPortEditText);
            ConnectBtn = FindViewById<Button>(Resource.Id.ConnectBtn);

            // Misc
            ConsoleTextView = FindViewById<TextView>(Resource.Id.ConsoleTextView);
            MessageEditText = FindViewById<EditText>(Resource.Id.MessageEditText);
            MessageEditText.Enabled = false;
            SendBtn = FindViewById<Button>(Resource.Id.SendBtn);
            SendBtn.Enabled = false;

            ListenBtn.Click += ListenBtn_Click;
            ConnectBtn.Click += ConnectBtn_Click;
            SendBtn.Click += SendBtn_Click;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        // Events
        public void ListenBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(ServerPortEditText.Text))
                {
                    if (ListenBtn.Text == "Listen")
                    {
                        server = new TcpListener(localIP, int.Parse(ServerPortEditText.Text));
                        keepListeningFlag = true;
                        ListenBtn.Text = "Stop";
                        AddToConsole($"Started listening on: {ServerIPEditText.Text}:{ServerPortEditText.Text}");

                        // Gebruiker is Server en mag niet tegelijkertijd een Client starten...
                        ClientIPEditText.Enabled = false;
                        ClientPortEditText.Enabled = false;

                        // Gebruiker mag berichten versturen
                        MessageEditText.Enabled = true;
                        SendBtn.Enabled = true;

                        server.Start();
                    } else
                    {
                        ListenBtn.Text = "Listen";
                        AddToConsole($"Stopped listening!");

                        // Gebruiker is vrij om te kiezen
                        ClientIPEditText.Enabled = true;
                        ClientPortEditText.Enabled = true;

                        // Gebruiker mag geen berichten versturen
                        MessageEditText.Enabled = false;
                        SendBtn.Enabled = false;
                    }
                } else
                {
                    Toast.MakeText(this, $"Please fill in the required fields: \"Port\"", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        public void ConnectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if ((!string.IsNullOrWhiteSpace(ClientIPEditText.Text))||(!string.IsNullOrWhiteSpace(ClientIPEditText.Text)))
                {
                    if (ConnectBtn.Text == "Connect")
                    {
                        client = new TcpClient(IPAddress.Parse(ClientIPEditText.Text).ToString(), int.Parse(ClientPortEditText.Text));
                        ConnectBtn.Text = "Disconnect";
                        AddToConsole($"Connected to server: {ClientIPEditText.Text}:{ClientPortEditText.Text}");

                        // Gebruiker is client en mag niet tegelijkertijd "Serven"
                        ServerPortEditText.Enabled = false;
                        ListenBtn.Enabled = false;

                        // Gebruiker mag berichten versturen
                        MessageEditText.Enabled = true;
                        SendBtn.Enabled = true;

                        clientReceive();
                    }
                    else
                    {
                        ConnectBtn.Text = "Connect";
                        AddToConsole($"Disconnected from server: {ClientIPEditText.Text}:{ClientPortEditText.Text}");
                        client.GetStream().Close();
                        client.Close();

                        // Gebruiker is vrij om te kiezen
                        ServerPortEditText.Enabled = true;
                        ListenBtn.Enabled = true;

                        // Gebruiker mag geen berichten versturen
                        MessageEditText.Enabled = false;
                        SendBtn.Enabled = false;
                    }
                } else
                {
                    Toast.MakeText(this, $"Please fill in the required fields: \"Client IP address\" and \"Port\"", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        public void SendBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (client.Connected)
                {
                    if (!string.IsNullOrWhiteSpace(MessageEditText.Text))
                    {
                        clientSend($"({ ServerIPEditText.Text }) { MessageEditText.Text }");
                    } else
                    {
                        Toast.MakeText(this, "Please type a message before sending...", ToastLength.Short).Show();
                    }
                } else
                {
                    Toast.MakeText(this, "Connection missing...", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        // Methods
        private void clientReceive()
        {
            if (client.Connected & stream.CanRead) {
                try
                {
                    stream = client.GetStream();
                    byte[] Buffer = new byte[2048];
                    int bytesRead = 0;

                    new Thread(() => {
                        while (stream.CanRead)
                        {
                            while ((bytesRead = stream.Read(Buffer, 0, Buffer.Length)) != 0)
                            {
                                stream.Read(Buffer, 0, bytesRead);

                                clientSend($"{DateTime.Now} - message received");

                                RunOnUiThread(() => ConsoleTextView.Text = $"{ConsoleTextView.Text}{DateTime.Now} - {Encoding.ASCII.GetString(Buffer)}{System.Environment.NewLine}");
                            }
                        }
                    }).Start();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
                }
            }
        }

        // Idee is om een foreground thread op te starten die verschillende 
        // connection requests op het achtergrond accepteerd.
        private void ServerListening()  
        {
            try
            {
                new Thread(() => {
                    // plaats hier de listening code...
                    while (keepListeningFlag)
                    {

                    }
                }).Start(); 
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        public void clientSend(string msg)
        {
            try
            {
                stream = client.GetStream();            // Gets the stream of the connection
                byte[] data = Encoding.Default.GetBytes(msg);  // Put the msg in the byte (it automatically uses the size of the msg)
                byte[] datalength = BitConverter.GetBytes(data.Length); // Put the length in a byte to send it
                stream.Write(datalength, 0, 4);         // sends the data's length
                stream.Write(data, 0, data.Length);     // Sends the real data

                AddToConsole($"(Self) { msg }");
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        public void AddToConsole(string consoleMessage)
        {
            ConsoleTextView.Text += $"\n{DateTime.Now.ToString("HH:mm, dd/MM/yyyy")} - {consoleMessage}";
        }
    }
}