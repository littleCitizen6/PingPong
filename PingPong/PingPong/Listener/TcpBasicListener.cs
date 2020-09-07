using PingPong.Server.Connections;
using PingPong.Server.RequestHendlers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace PingPong.Server.Listener
{
    public class TcpBasicListener : IListener
    {
        const int MAX_CONNECTIONS = 10;
        private List<IConnection> _connections;
        private IPAddress _ipAddress;
        private int _port;
        private IRequestHandler _requestHandler;
        private object _locker;
        public TcpBasicListener(string address, int port, IRequestHandler requestHandler)
        {
            _ipAddress = IPAddress.Parse(address);
            _port = port;
            _connections = new List<IConnection>();
            _requestHandler = requestHandler;
            _locker = new object();
        }
        public void StartListening()
        {
            try
            {
                var server = new TcpListener(_ipAddress, _port);
                server.Start();
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    Task Connect = new Task(() =>
                    {
                        int index;
                        lock (_locker)
                        {
                            _connections.Add(new TcpConnection(client, _requestHandler));
                            index = _connections.Count - 1;
                        }
                        _connections[index].Run();
                        _connections[index].Dispose();
                    });
                    Connect.Start();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            /*finally
            {
                // Stop listening for new clients.
                server.Stop();
            }*/
        }
    }
}
