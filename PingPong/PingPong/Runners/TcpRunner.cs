using PingPong.Server.Listener;
using PingPong.Server.RequestHendlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PingPong.Server.Runners
{
    public class TcpRunner : Runner
    {
        public TcpRunner(string address, int port) : base(address, port)
        {
            _requesHandler = new MirrorRequestHandler();
            _listener = new TcpBasicListener(address, port, _requesHandler);
        }
    }
}
