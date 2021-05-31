using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;
using System.Threading;
using Websocket.Client;

namespace Uech_Discord_Library
{
    public class Client
    {
        bool Debug { get; set; }
        string Token { get; set; }

        // Listner for messages
        //public event TickHandler tick;
        //public delegate void TickHandler(Client m);

        public async Task Login(string Token, Client getref, bool debug = false)
        {
            Identify identify0 = new();
            var url = new Uri("wss://gateway.discord.gg/?v=9&encoding=json");
            getref.Token = Token;

            var wsclient = new WebsocketClient(url);
            {
                // Reconnections
                wsclient.ReconnectionHappened.Subscribe(info => { Console.WriteLine(info.GetType()); });

                // Get messages
                wsclient.MessageReceived.Subscribe(msg =>
                {
                    dynamic d = JsonConvert.DeserializeObject(msg.ToString());
                    if (getref.Debug) Console.WriteLine("\r\n" + d.ToString());
                });

                // Start connection...
                await wsclient.Start();

                // Identify payload
                await Task.Run(() =>
                {
                    wsclient.Send(identify0.Verify(identify0, Token));
                });

                // Handshake payload
                await Task.Run(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(30000);
                        Console.WriteLine("Sending hearthbeat!");
                        wsclient.Send(identify0.Heartbeat());
                    }
                });

                Console.ReadLine();
            }

        }
    }
}
/*
    public class Listner
    {
        public void Subscribe(Client m)
        {
            m.tick += new Client.TickHandler(Message);
        }

        private void Message(Client m)
        {
            //string msg = "";
            m.wsclient.MessageReceived.Subscribe(msg => {
            dynamic d = JsonConvert.DeserializeObject(msg.ToString());
                //if (d.t == "MESSAGE_CREATE")
                Console.WriteLine("\r\r\nNew data recived...");
            });
            //return msg;
        }
    }
}
*/
