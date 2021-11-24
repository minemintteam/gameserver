using System.Net;
using System.Net.Sockets;


namespace GameServer 
{
    class Server 
    {
        //Thread? gameThread;
        TcpClient client;
        NetworkStream? stream;
        FlatDB? flatDB;
        public Server(object obj, object obj2)
        {
            client = (TcpClient)obj;
            flatDB = (FlatDB)obj2;
            stream = client.GetStream();
            Console.WriteLine("Client connected");
        }
    }
}