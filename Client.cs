using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;
using System.Threading;
using Websocket.Client;

namespace Uech_Discord_Library
{
    class Client
    {
        public bool Debug { get; set; }
        string Token { get; set; }
        WebsocketClient wsclient;
        public async Task Login(string Token, bool debug = false)
        {
            var start = DateTime.Now;
            Client getref = new();
            Identify identify0 = new();
            var url = new Uri("wss://gateway.discord.gg/?v=9&encoding=json");
            getref.Token = Token;

            getref.wsclient = new WebsocketClient(url);
            {
                // Reconnections
                {
                    getref.wsclient.ReconnectTimeout = TimeSpan.FromSeconds(30000);
                    getref.wsclient.ReconnectionHappened.Subscribe(info => {
                        Console.WriteLine(info.GetType());
                        Console.WriteLine(DateTime.Now - start);
                    });
                }

                // Get messages
                getref.wsclient.MessageReceived.Subscribe(msg => {
                    dynamic d = JsonConvert.DeserializeObject(msg.ToString());
                    if (getref.Debug) Console.WriteLine("\r\n"+d.ToString());
                    if (d.t == "MESSAGE_CREATE") Console.WriteLine($"Author: {d.d.author.username} Message: {d.d.content}");
                });

                // Start connection...
                await getref.wsclient.Start();

                // Identify payload
                await Task.Run(() => getref.wsclient.Send(identify0.Verify(identify0, Token)));

                // Handshake payload
                await Task.Run(() =>
                {
                    while(true)
                    {
                        Thread.Sleep(30000);
                        Console.WriteLine("Sending hearthbeat!");
                        getref.wsclient.Send(identify0.Heartbeat());
                    }
                });

                Console.ReadLine();
            }
            
        }
    }
}
