using System;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server test = new Server("127.0.0.1",80);
            test.Start();
        }    
    }
}