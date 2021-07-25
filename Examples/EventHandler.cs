using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uech_Discord_Library
{
    public class EventHandler
    {
        public void Start()
        {
            var client = new Client("Token");
            client.tick += OnMessageCreated;
        }

        public static void OnMessageCreated(Object sender, Data Message)
        {
            Console.WriteLine("We recived a message: {Message}");
        }
    }
}
