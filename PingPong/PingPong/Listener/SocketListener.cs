using PingPong.Server.Connections;
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
        public SocketListener() 
        {
            // Get Host IP Address that is used to establish a connection  
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
            // If a host has multiple addresses, you will get a list of addresses  
            _host = Dns.GetHostEntry("localhost");
            _ipAddress = _host.AddressList[0];
            _localEndPoint = new IPEndPoint(_ipAddress, 11000);
            _connections = new List<IConnection>();
            
        }

        public void StartServer()
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
                        _connections[FindFirstIndexAvailable()] = new SocketConnection(handler);
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
