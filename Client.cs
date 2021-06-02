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

        public Client(string token, bool debug = false)
        {
            Identify identify0 = new();
            Debug = debug;
            Token = token;
            Listeners listeners = new();

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
                    Task.Run(() => { listeners.Value = msg; });
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

        private void Handshake(Identify identify, WebsocketClient wsclient)
        {
            while (true)
            {
                Thread.Sleep(30000);
                Console.WriteLine("Sending hearthbeat!");
                wsclient.Send(identify.Heartbeat());
            }
        }
    }
    public class DiscordEventArgs : EventArgs
    {
        object _value;
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
    public class Listeners
    {
        public event EventHandler<DiscordEventArgs> OnResponse;
        private object _value;

        protected virtual void SendResponse()
        {
            DiscordEventArgs discodEventArgs = new();
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

