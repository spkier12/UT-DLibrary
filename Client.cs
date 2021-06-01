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
        public Client(string token, bool debug = false)
        {
            Identify identify0 = new Identify();
            var url = new Uri("wss://gateway.discord.gg/?v=9&encoding=json");
            Debug = debug;
            Token = token;

            WebsocketClient wsclient = new WebsocketClient(url);
            {
                // Reconnections
                wsclient.ReconnectionHappened.Subscribe(info => { if(debug) Console.WriteLine(info.GetType()); });

                // Get messages
                wsclient.MessageReceived.Subscribe(msg =>
                {
                    listeners.Value = msg;
                    dynamic d = JsonConvert.DeserializeObject(msg.ToString());
                    if (debug) Console.WriteLine("\r\n" + d.ToString());
                });

                // Start connection...
                wsclient.Start();

                // Identify payload
                Task.Run(() =>
                {
                    wsclient.Send(identify0.Verify(identify0, token));
                });

                // Handshake payload
                Task.Run(() => Handshake(identify0));

                Console.ReadLine();
            }
        }
        Listeners listeners = new Listeners();

        // Listner for messages
        //public event TickHandler tick;
        //public delegate void TickHandler(Client m);

        private async Task Handshake(Identify identify)
        {
            var url = new Uri("wss://gateway.discord.gg/?v=9&encoding=json");

            WebsocketClient wsclient = new WebsocketClient(url);
            {
                await Task.Run(() => {
                    while (true)
                    {
                        Thread.Sleep(30000);
                        Console.WriteLine("Sending hearthbeat!");
                        wsclient.Send(identify.Heartbeat());
                    }
                });
            }
        }
    }
    public class DiscordEventArgs : EventArgs
    {
        object _value;
        public object Value
        {
            get { return _value; }
            set
            {
                _value = value;
            }
        }
    }
    public class Listeners
    {
        public event EventHandler<DiscordEventArgs> OnResponse;
        private object _value;

        protected virtual void SendResponse()
        {
            DiscordEventArgs discodEventArgs = new DiscordEventArgs();
            discodEventArgs.Value = _value;
            OnResponse(this, discodEventArgs);
        }
        public object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                SendResponse();
            }
        }
    }
}

