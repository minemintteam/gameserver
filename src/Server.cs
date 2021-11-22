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
                NetworkStream stream = client.GetStream();
                stream.Write(System.Text.Encoding.ASCII.GetBytes("Hello World!"),0,System.Text.Encoding.ASCII.GetBytes("Hello World!").Length);
            }
        }  
        public void Sex() {
            Console.WriteLine("Hellow World!");
        }
    }
}