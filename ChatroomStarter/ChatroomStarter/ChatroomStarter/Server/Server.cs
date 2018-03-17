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
            Parallel.Invoke(
                () => {
                    AcceptClient();
                }
                ,
                () => {
                    Respond("Message");
                }

            );
            Thread thread = new Thread(AcceptClient);
            thread.Start();
            //Thread thread2 = new Thread(()=>
            //thread2.Start();
            //Thread thread3 = new Thread(ReceiveMessage);
            //thread3.Start();
            //AcceptClient();
            //string message = client.Recieve();
            //Respond(message);
        }
        private void AcceptClient() //create while loop, thread should go here. dictionary gets information from client here         
        {
            int i = 0;
            while (true)
            { 
                TcpClient clientSocket = default(TcpClient); //like a virtual handshake between two servers
                clientSocket = server.AcceptTcpClient();
                Console.WriteLine("Connected");
                NetworkStream stream = clientSocket.GetStream();
                Client newClient = new Client(stream, clientSocket);
                //newClient.run();
                if (clientSocket  != null)
                {
                    Thread client = new Thread(() => newClient.Recieve());
                    client.Start();
                    listOfClients.Add(i, newClient);
                    //notification to users goes here
                    i++;
                }
                
            }
        }
        private void Respond()
        {
            while (true)
            {
                if (messages.Count > 0)
                {
                    Message messageToSend = messages.Dequeue();
                    foreach (int,Client index,item in listOfClients)
                    {
                        item.Send(messageToSend.Body);
                    }
                }
            }
             //client.Send(body);
        }
    }
}
