using PingPong.Server.RequestHendlers;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace PingPong.Server.Connections
{
    public class TcpConnection : IConnection
    {
        private TcpClient _client;
        private IRequestHandler _handler;
        public TcpConnection(TcpClient client, IRequestHandler requesrHandler)
        {
            _client = client;
            _handler = requesrHandler;
        }
        public bool IsRunning => _client.Connected;

        public void Dispose()
        {
            _client.Dispose();
        }

        public void Run()
        {
            Byte[] bytes = new Byte[256];
            String data;
            NetworkStream stream = _client.GetStream();

            int i;
            while (true)
            {
                if ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    stream.Write(_handler.Handle(data));
                    Console.WriteLine("Sent: {0}", data);
                }
            }

        }
    }
}
