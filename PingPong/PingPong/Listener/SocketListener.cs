using PingPong.Server.Connections;
using PingPong.Server.RequestHendlers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PingPong.Server.Listener
{
    public class SocketListener : IListener
    {
        const int MAX_CONNECTIONS = 10;
        private List<IConnection> _connections;
        private IPHostEntry _host;
        private IPAddress _ipAddress;
        private IPEndPoint _localEndPoint;
        private IRequestHandler _requestHandler;
        public SocketListener(string address, int port, IRequestHandler requestHandler) 
        {
            _ipAddress = IPAddress.Parse(address);
            _localEndPoint = new IPEndPoint(_ipAddress, port);
            _connections = new List<IConnection>();
            _requestHandler = requestHandler;
        }

        public void StartListening()
        {
            try
            {
                Socket listener = new Socket(_ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(_localEndPoint);
                listener.Listen(MAX_CONNECTIONS);

                while (true)
                {
                    Socket handler = listener.Accept();
                    Task Connect = new Task(() =>
                    {
                        _connections[FindFirstIndexAvailable()] = new SocketConnection(handler, _requestHandler);
                        _connections[FindFirstIndexAvailable()].Run();
                        _connections[FindFirstIndexAvailable()].Dispose();
                    });
                    Connect.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\n Press any key to continue...");
            Console.ReadKey();
        }

        private int FindFirstIndexAvailable()
        {
            while (true)
            {
                int i = 0;
                while(i<MAX_CONNECTIONS)
                {
                    if(!_connections[i].IsRunning)
                    {
                        return i;
                    }
                    i++;
                }
            }
        }
    }
}
