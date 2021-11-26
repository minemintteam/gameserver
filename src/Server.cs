using System.Net.Sockets;
using System.Text;

namespace GameServer 
{
    class Server 
    {
        Thread? gameThread;
        TcpClient client;
        NetworkStream? stream;
        FlatDB? gameDB;
        WSHelper? wsHelper;
        public Server(object obj, object obj2)
        {
            client = (TcpClient)obj;
            gameDB = (FlatDB)obj2;

            stream = client.GetStream();
            wsHelper = new WSHelper(stream);
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
                string data = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead.HasValue ? bytesRead.Value : 0);
                string[] splitData = data.Split('\r');
                
                if (bytesRead == 0)
                {
                    Console.WriteLine("Client disconnected");
                    break;
                }

                switch (splitData[0])
                {
                    case "GET / HTTP/1.1":
                        string[] temp = splitData[11].Split(' ');

                        const string eol = "\r\n";

                        Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + eol
                        + "Connection: Upgrade" + eol
                        + "Upgrade: websocket" + eol
                        + "Sec-WebSocket-Accept: " + Convert.ToBase64String(System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(temp[1] + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"))) + eol + eol);
                        stream?.Write(response, 0, response.Length);
                        break;
                    default:
                        string[]? message = wsHelper?.Decoder(buffer).Split(' ');
                        HandleAction(message?[0], message);
                        break;
                }
            }
            
        }

         private void HandleAction(string? action, string[]? data) {
            switch (action) {
                case "LOGIN":
                    break;
                case "MOVE":
                    string[]? splitData = data?[1].Split(',');
                    gameDB?.Write(splitData?[0] + ":" + splitData?[1] + ":" + splitData?[2]);
                    Console.WriteLine("MOVE " + splitData?[0] + " " + splitData?[1] + " " + splitData?[2]); 
                    break;
                case "PING":
                    wsHelper?.SendMsg("PONG");
                    break;
                default:
                    Console.WriteLine("Unknown action: " + action);
                    break;
            }
        }

        private void Relay(string message) {
        }
    }
}