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
        Listeners listeners = new Listeners();
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
                    listeners.Value = msg;
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

