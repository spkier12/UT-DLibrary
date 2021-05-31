using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Uech_Discord_Library
{

    // First time identify to discord gateway
    class Identify
    {
        public int op = 2;
        public D d = new();

        public class D
        {
            public string token { get; set; }
            public int intents = 513;
            public Properties properties = new();

        }
        public class Properties
        {
            [JsonProperty("$os")]
            public string Os = "linux";

            [JsonProperty("$browser")]
            public string Browser = "utech";

            [JsonProperty("$device")]
            public string Device = "utech";
        }

        public class Handshake
        {
            public readonly int op = 1;
            public readonly int d = 251;
        }


        public string Verify(Identify getref, string token)
        {
            getref.d.token = token;
            return JsonConvert.SerializeObject(getref);
        }

        public string Heartbeat()
        {
            return JsonConvert.SerializeObject(new Handshake());
        }
    }
}
