using PingPong.Server.RequestHendlers;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace PingPong.Server.Connections
{
    public class SocketConnection: IConnection
    {
        private bool _isRunning;
        private Socket _socket;
        private IRequestHandler _requestHandler;
        public SocketConnection(Socket socket, IRequestHandler requestHandler)
        {
            _socket = socket;
            _requestHandler = requestHandler;

        }

        public bool IsRunning => _socket.Connected;

        public void Dispose()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        public void Run()
        {
            // Incoming data from the client.    
            string data = null;
            byte[] bytes = null;

            while (true)
            {
                bytes = new byte[1024];
                int bytesRec = _socket.Receive(bytes);
                //data += Encoding.ASCII.GetString(bytes, 0, bytesRec); 
                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if (data != null)
                {
                    Console.WriteLine(data);
                    if (data == "exit")
                    {
                        break;
                    }
                    else
                    {
                        _requestHandler.Handle(data);
                    }
                }
            }

            Console.WriteLine("Text received : {0}", data);

            byte[] msg = Encoding.ASCII.GetBytes(data); // the implement of the request handler
            _socket.Send(msg);
        }
    }
    }
}
