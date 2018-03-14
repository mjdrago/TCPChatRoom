using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        public static Client client;
        Dictionary<int, Client> listOfClients; //deals with clients, can pick what you take out
        Queue<Message> messages; //deals with messages, first in first out
        TcpListener server;

        public Server()
        {
            server = new TcpListener(IPAddress.Parse("192.168.0.153"), 9999);
            server.Start(); //Makes it start listening to connections
            listOfClients = new Dictionary<int, Client>();
            messages = new Queue<Message>();
        }
        public void Run() //create the two threads we need - acceptclient, sendmessage, receivemessage
        {
            //Thread thread = new Thread(AcceptClient);
            //thread.Start();
            //Thread thread2 = new Thread(SendMessage);
            //thread2.Start();
            //Thread thread3 = new Thread(ReceiveMessage);
            //thread3.Start();
            AcceptClient();
            string message = client.Recieve();
            Respond(message);
        }
        private void AcceptClient() //create while loop, thread should go here. dictionary gets information from client here         
        {
            TcpClient clientSocket = default(TcpClient); //like a virtual handshake between two servers
            clientSocket = server.AcceptTcpClient();
            Console.WriteLine("Connected");
            NetworkStream stream = clientSocket.GetStream(); 
            client = new Client(stream, clientSocket);

        }
        private void Respond(string body)
        {
             client.Send(body);
        }
    }
}
