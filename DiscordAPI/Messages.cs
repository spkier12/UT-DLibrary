using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace Uech_Discord_Library.DiscordAPI
{
    public class Messages
    {
        // Need to send header data unsure how in a GET request..
        public string GetChannelID(string ID, string Token)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("authorization", $"Bot {Token}");
            var data = client.GetAsync($"https://discord.com/api/v9/channels/{ID}").Result;
            return data.Content.ReadAsStringAsync().Result;
        }

        // No functions below here is usable without getting channel id first
        public string SendMessage(string ID, string Token, string Message = "Test")
        {
            var body = new {
                content = "Hello, World!",
                tts = false,
                embed = new
                {
                    title = "Hello, Embed!",
                    description = "This is an embedded message."
                }
            };
            HttpClient client = new();

            client.DefaultRequestHeaders.Add("authorization", $"Bot {Token}");
            var body1 = JsonConvert.SerializeObject(body);
            StringContent content1 = new StringContent(body1, Encoding.UTF8, "application/json");
            var data = client.PostAsync($"https://discord.com/api/v9/channels/${ID}/messages", content1).Result;
            return data.Content.ReadAsStringAsync().Result;
        }
    }
}
