using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Client: IObeserverDesignPattern
    {
        NetworkStream stream; 
        TcpClient client;
        public string UserName;
        public Client(NetworkStream Stream, TcpClient Client) 
        {
            stream = Stream;
            client = Client;
            //UserId = "495933b6-1762-47a1-b655-483510072e73";
        }
        public void Send(string Message)
        {
            byte[] message = Encoding.ASCII.GetBytes(Message);
            stream.Write(message, 0, message.Count());
        }
        public void Recieve()
        {
            while (true)
            {
                byte[] recievedMessage = new byte[256];
                try
                {
                    stream.Read(recievedMessage, 0, recievedMessage.Length);
                }
                catch (Exception)
                {
                    break;
                }
                
                string recievedMessageString = Encoding.ASCII.GetString(recievedMessage);
                Message messageToAdd = new Message(this, recievedMessageString);
                Server.messages.Enqueue(messageToAdd);         
            }
        }
        public void Update(string textToSend)
        {
            Send(textToSend);
        }
        public void GetUserName()
        {
            Send("Please enter your User Name:");
            byte[] recievedMessage = new byte[256];
            stream.Read(recievedMessage, 0, recievedMessage.Length);
            UserName = Encoding.ASCII.GetString(recievedMessage);
        }
    }
}
