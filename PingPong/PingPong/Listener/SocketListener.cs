using PingPong.Server.Connections;
using PingPong.Server.RequestHendlers;
using System;
using System.Collections.Generic;
using System.Linq;
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
                        int index = FindFirstIndexAvailable();
                        if (index == _connections.Count)
                        {
                            _connections.Add(new SocketConnection(handler, _requestHandler));
                        }
                        else
                        {
                            _connections[index] = new SocketConnection(handler, _requestHandler); // in case there was more then the maximum connection
                        }
                        _connections[index].Run();
                        _connections[index].Dispose();
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
            if(_connections.Count<MAX_CONNECTIONS)
            {
                return _connections.Count;
            }
            while (true)
            {
                var firstAvailable = _connections.FirstOrDefault(x => !x.IsRunning);
                if (firstAvailable != null)
                {
                    return _connections.IndexOf(firstAvailable);
                }
            }
        }
    }
}
