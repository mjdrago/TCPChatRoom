using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ChatLog
    {
        ILogger chatLog;
        public ChatLog (ILogger chatlog)
        {
            this.chatLog = chatlog; 
        }
        public void InsertToLog(string logText)
        {
            chatLog.InsertToLog(logText);
        }
    }
}
