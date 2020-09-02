using PingPong.Server.Listener;
using PingPong.Server.RequestHendlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PingPong.Server.Runners
{
    public abstract class Runner
    {
        protected IListener _listener;
        protected IRequestHandler _requesHandler;
        private string _address;
        private int _port;

        public Runner(string address, int port)// any child should choose implement for the interfaces
        {
            _address = address;
            _port = port;
        }
        
        public void Run()
        {
            _listener.StartListening();
        }

    }
}
