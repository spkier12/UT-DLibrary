using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uech_Discord_Library
{
    public class Test
    {
        public void Start()
        {
            Listeners listeners = new Listeners();
            listeners.OnResponse += new EventHandler<DiscordEventArgs>(onResponse);
        }
        private void onResponse(object sender, DiscordEventArgs e)
        {

        }
    }
}
