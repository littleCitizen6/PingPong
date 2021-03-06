﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace PingPong.Server.Connections
{
    public interface IConnection : IDisposable
    {
        public bool IsRunning { get;}
        public void Run();
    }
}
