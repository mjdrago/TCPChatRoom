using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        TcpClient clientSocket;
        NetworkStream stream;
        public Client(string IP, int port)
        {
            clientSocket = new TcpClient();
            clientSocket.Connect(IPAddress.Parse(IP), port);
            stream = clientSocket.GetStream();
        }
        public void Run()
        {
            Parallel.Invoke(
                () =>
                {
                    Send();
                }
                ,
                () =>
                {
                    Recieve();
                }
                );
        }
        public void Send()
        {
            while (true)
            {
                string messageString = UI.GetInput();
                if (messageString != null)
                {
                    byte[] message = Encoding.ASCII.GetBytes(messageString);
                    stream.Write(message, 0, message.Count());
                }
                
            }
        }
        public void Recieve()
        {
            while (true)
            {
                byte[] recievedMessage = new byte[256];
                if (recievedMessage != null)
                {
                    stream.Read(recievedMessage, 0, recievedMessage.Length); 
                    UI.DisplayMessage(Encoding.ASCII.GetString(recievedMessage));
                }
                
            }
        }
    }
}
