using System;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    class Program
    {
        static string server_version = "0.0.0.1";
        static TcpListener? listener;
        static TcpClient? client;
        static Server? server;
        static FlatDB? gameDB; //mailDB, serverLog, serverConfig;
        static void Main(string[] args)
        {
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);
            listener.Start();

            Console.WriteLine("Game/Engine/Server v" + server_version);

            gameDB = new FlatDB("db");

            while(true) 
            {
                client = listener.AcceptTcpClient();
                server = new Server(client, gameDB);
            }
        }    
    }
}