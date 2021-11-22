using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GameServer 
{
    class Server 
    {
        TcpListener server; 
        public Server(string ip_string, int port_number)
        {
            server = new TcpListener(IPAddress.Parse(ip_string), port_number);
        }
        public void Start()
        {
            server.Start();
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Client connected");
            }
        }

    }
}