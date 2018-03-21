using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Message //may get used for dependency injection? currently dead code
    {
        public Client sender;
        public string Body;
        public string UserName;
        public Message(Client Sender, string Body)
        {
            sender = Sender;
            this.Body = Body;
            UserName = sender?.UserName;
        }
    }
}
