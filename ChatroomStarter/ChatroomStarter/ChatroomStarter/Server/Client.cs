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
        NetworkStream stream; //provides stream of data for network. Allows client to stream, send and receive messages
        TcpClient client;
        public string UserId;
        public Client(NetworkStream Stream, TcpClient Client) 
        {
            stream = Stream;
            client = Client;
            UserId = "495933b6-1762-47a1-b655-483510072e73";
        }
        public void Send(string Message)
        {
            byte[] message = Encoding.ASCII.GetBytes(Message);//allows it to transfer from high level language to low level
            stream.Write(message, 0, message.Count());
        }
        public string Recieve()
        {
            byte[] recievedMessage = new byte[256]; //don't get why we need an array of bytes?
            stream.Read(recievedMessage, 0, recievedMessage.Length);
            string recievedMessageString = Encoding.ASCII.GetString(recievedMessage);// converts from low level, back to high level language
            Console.WriteLine(recievedMessageString);
            return recievedMessageString;
        }

    }
}
