using System;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    class Program
    {
        static string server_version = "0.0.0.1";
        static TcpListener? server;
        static TcpClient? client;
        static Server? server_instance;
        static FlatDB? gameDB; //mailDB, serverLog, serverConfig;
        static void Main(string[] args)
        {
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);
            server.Start();

            Console.WriteLine("Game/Engine/Server v" + server_version);

            gameDB = new FlatDB("db");
            while(true) 
            {
                client = server.AcceptTcpClient();
                server_instance = new Server(client, gameDB);
            }
        }    
    }
}