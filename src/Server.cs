using System.Net;
using System.Net.Sockets;


namespace GameServer 
{
    class Server 
    {
        Thread? gameThread;
        TcpClient client;
        NetworkStream? stream;
        FlatDB? gameDB;
        public Server(object obj, object obj2)
        {
            client = (TcpClient)obj;
            gameDB = (FlatDB)obj2;

            stream = client.GetStream();
            Console.WriteLine("Client connected");

            gameThread = new Thread(new ThreadStart(Run));
            gameThread.Start();
        }

        private void Run()
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int? bytesRead = stream?.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    Console.WriteLine("Client disconnected");
                    break;
                }
                string data = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead.HasValue ? bytesRead.Value : 0);
                string[] splitData = data.Split('\r');
                switch (splitData[0])
                {
                    case "LOGIN":
                        break;
                    case "GET / HTTP/1.1":
                        Console.WriteLine("Hit GET");
                        break;
                }
            }
        }
    }
}