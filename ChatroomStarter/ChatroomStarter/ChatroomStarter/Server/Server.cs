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
                    List<int> skipped = new List<int>();
                    foreach (KeyValuePair<int,Client> user in listOfClients)
                    {
                        if (user.Value != messageToSend.sender)
                        {
                            try
                            {
                                user.Value.Send(messageToSend.Body);
                            }
                            catch (Exception)
                            {
                                Notify("A user has left the chat");
                                skipped.Add(user.Key);
                                continue;
                            }

                        }
                        
                    }
                    foreach (int key in skipped) //creat own method for detach
                    {
                        Detach(key);
                    }
                }
            }
        }
        public void Attach(int id,Client newClient)
        {
            listOfClients.Add(id, newClient);
        }
        public void Detach(int key)
        {
            listOfClients.Remove(key);
        }
        public void Notify(string message)
        {
            foreach (KeyValuePair<int, Client> user in listOfClients)
            {
                user.Value.Send(message);
            }
        }
        public void CheckConnections()
        {
            int keys = 1;
            while (keys < listOfClients.Count)
            {

                if (listOfClients[keys].IsConnected() == false)
                {
                    Detach(keys);
                    Notify("User has left chat");
                }
                else
                {
                    keys++;
                }
                
            }
            
        }
    }
}
