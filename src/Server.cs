using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GameServer 
{
    class Server 
    {
        //Thread? handleClients;
        TcpListener server;  
        TcpClient? client;
        public Server(string ip_string, int port_number)
        {
            server = new TcpListener(IPAddress.Parse(ip_string), port_number);
        }
        public void Start()
        {
            server.Start();
            while (true)
            {
                client = server.AcceptTcpClient();         
                //HandleClient(client);       
                NetworkStream stream = client.GetStream();
                StreamReader reader = new StreamReader(stream); 
                StreamWriter writer = new StreamWriter(stream);

                while (client.Connected)
                {
                    string? message = reader.ReadLine();
                    Console.WriteLine(message);
                }
            }
        }
    }
}