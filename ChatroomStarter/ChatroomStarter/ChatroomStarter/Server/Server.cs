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
        Dictionary<int, Client> listOfClients;
        public static Queue<Message> messages;
        TcpListener server;
        private Object enumerationLock = new Object();

        public Server()
        {
            server = new TcpListener(IPAddress.Parse("192.168.0.162"), 9999);
            server.Start(); 
            listOfClients = new Dictionary<int, Client>();
            messages = new Queue<Message>();
        }
        public void Run() 
        {
            Parallel.Invoke(
                () => {
                    AcceptClient();
                }
                ,
                () => {
                    SendFromQueue();
                }
                //,
                //() =>
                //{
                //    ReceiveToQueue();
                //}
            );
        }
        public void AcceptClient() 
        {
            int i = 0;
            while (true)
            { 
                TcpClient clientSocket = default(TcpClient);
                clientSocket = server.AcceptTcpClient();
                Console.WriteLine("Connected");
                NetworkStream stream = clientSocket.GetStream();
                Client newClient = new Client(stream, clientSocket);
                if (clientSocket  != null)
                {
                    Thread client = new Thread(() => newClient.Recieve());
                    client.Start();
                    Notify("New user has joined the chat."); //Update to make it user specific
                    Attach(i, newClient);
                    i++;
                }
                
            }
        }
        private void SendFromQueue()
        {
            while (true)
            {
                if (messages.Count > 0)
                {
                    Message messageToSend = messages.Dequeue();
                    foreach (KeyValuePair<int,Client> user in listOfClients)
                    {
                        try
                        {
                            user.Value.Send(messageToSend.Body);
                        }
                        catch (Exception)
                        {
                            Detach();
                            Notify("A user has left the chat");
                            throw;
                        }
                    }
                }
            }
        }
        //public void ReceiveToQueue()
        //{
        //    while (true)
        //    {
        //        lock (enumerationLock)
        //        {
        //            foreach (KeyValuePair<int, Client> user in listOfClients)
        //            {
        //                    if (user.Value.messages.Count > 0)
        //                    {
        //                        Message messageToReceive = new Message(user.Value, user.Value.messages.Dequeue());
        //                        messages.Enqueue(messageToReceive);
        //                    }
        //            }
        //        }
                
        //    }
        //}
        public void Attach(int id,Client newClient)
        {
            listOfClients.Add(id, newClient);
        }
        public void Detach()
        {

        }
        public void Notify(string message)
        {
            foreach (KeyValuePair<int, Client> user in listOfClients)
            {
                user.Value.Send(message);
            }
        }
    }
}
