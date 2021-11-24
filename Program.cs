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
        static FlatDB? gameDB, mailDB, serverLog, serverConfig;
        static void Main(string[] args)
        {
            gameDB = new FlatDB("db");
            //mailDB = new FlatDB("mail");
            serverLog = new FlatDB("serverlog");
            //serverConfig = new FlatDB("serverconfig");

            Console.WriteLine("Game/Engine/Server v" + server_version);
            serverLog.Log("Game/Engine/Server v" + server_version);

            
            //gameDB.Write("Test:{x:1000,y:1000,moving:false}");
            //gameDB.Write("SamTest:{x:10,y:100,moving:true}");

            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);
            server.Start();
            while(true) 
            {
                client = server.AcceptTcpClient();
                server_instance = new Server(client, gameDB);
            }
        }    
    }
}