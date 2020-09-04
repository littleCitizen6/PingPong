using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PingPongClient
{
    class Program
    {
        static void Main(string[] args)
        {
            GetInputsClient client = new GetInputsClient();
            client.Run();
        }
    }
}
