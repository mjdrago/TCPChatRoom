using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class TextLog : ILogger
    {
        public void InsertToLog(string logText)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\mario\Documents\devCodeCamp\Projects\8 - TCP Chat Room\TCPChatRoom\ChatroomStarter\ChatroomStarter\ChatroomStarter\ChatLog.txt",true))
            {
                file.WriteLine(logText);
            }
        }
    }
}
