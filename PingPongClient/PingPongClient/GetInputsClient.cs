using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PingPongClient
{
    public class GetInputsClient
    {
        private IPEndPoint _remoteEP;
        public GetInputsClient()
        {
            IPAddress ipAddress = IPAddress.Parse("10.1.0.14");
            _remoteEP = new IPEndPoint(ipAddress, 9000);
        }

        public void Run()
        {
            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(_remoteEP);
            while (true)
            {
                byte[] byData = Encoding.ASCII.GetBytes(Console.ReadLine());
                sender.Send(byData);
                byte[] b = new byte[100];
                int k = sender.Receive(b);
                string msgReceived = Encoding.ASCII.GetString(b, 0, k);
                Console.WriteLine(msgReceived);
            }
        }
    }
}
