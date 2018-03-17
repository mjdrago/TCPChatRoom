using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Client
    {
        NetworkStream stream; 
        TcpClient client;
        public string UserId;
        //public Queue<String> messages;
        public Client(NetworkStream Stream, TcpClient Client) 
        {
            stream = Stream;
            client = Client;
            UserId = "495933b6-1762-47a1-b655-483510072e73";
            //messages = new Queue<string>();
        }
        public void Send(string Message)
        {
            byte[] message = Encoding.ASCII.GetBytes(Message);
            stream.Write(message, 0, message.Count());
        }
        public void Recieve()
        {
            while (true)//Figure out how to allow someone to leave chat with erroring out.
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
        public bool IsConnected()
        {
            return client.Connected;
        }
    }
}
