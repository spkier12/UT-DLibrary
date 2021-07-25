using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;
using System.Threading;
using Websocket.Client;

namespace Uech_Discord_Library
{
    public class Client
    {
        bool Debug = false;
        string Token = string.Empty;
        Uri GatewayURI = new("wss://gateway.discord.gg/?v=9&encoding=json");
        public event EventHandler<Data> tick;
        public static Data data = new Data();


        public Client(string token, bool debug = false)
        {
            Identify identify0 = new();
            Debug = debug;
            Token = token;

            WebsocketClient wsclient = new(GatewayURI);
            {
                // Reconnections
                wsclient.ReconnectionHappened.Subscribe(info => { if(debug) Console.WriteLine(info.Type); });

                // Get messages
                wsclient.MessageReceived.Subscribe(msg =>
                {
                    //Console.WriteLine(msg.ToString());
                    dynamic d = JsonConvert.DeserializeObject(msg.ToString());
                    if (debug) Console.WriteLine("\r\n" + d.ToString());
                    data.OnMessageCreated = d;
                    tick.Invoke(this, data);
                });

                // Start connection...
                wsclient.Start();

                // Identify payload
                Task.Run(() =>
                {
                    wsclient.Send(identify0.Verify(identify0, token));
                });

                // Handshake payload
                Task.Run(() => Handshake(identify0, wsclient));

                Console.ReadLine();
            }
        }

        void Handshake(Identify identify, WebsocketClient wsclient)
        {
            while (true)
            {
                Thread.Sleep(30000);
                Console.WriteLine("Sending hearthbeat!");
                wsclient.Send(identify.Heartbeat());
            }
        }
    }

    public class Data
    {
        public string OnMessageCreated { get; set; }
        public string OnMessageDeleted { get; set; }
        public string OnMessageUpdated { get; set; }
    }
}

