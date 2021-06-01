using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace Uech_Discord_Library.DiscordAPI
{
    class Messages
    {
        // Need to send header data unsure how in a GET request..
        public async Task<HttpResponseMessage> GetChannelID(string ID, string Token)
        {
            var content = new
            {
                method = "GET",
                header = new
                {
                    authorization = $"Bot {Token}"
                },
            };
            HttpClient client = new();
            return await client.GetAsync($"https://discord.com/api/v9/channels/{ID}");
        }

        // No functions below here is usable without getting channel id first
        public async Task<HttpResponseMessage> SendMessage(string ID, string Token, string Message = "Test")
        {
            var content = new {
                method = "POST",
                header = new
                {
                    authorization = $"Bot {Token}"
                },

                body = new
                {
                    content = Message
                }
        };
            HttpClient client = new();
            return await client.PostAsync($"https://discord.com/api/v9/channels/${ID}/messages", new StringContent(JsonConvert.SerializeObject(content)));
        }
    }
}
