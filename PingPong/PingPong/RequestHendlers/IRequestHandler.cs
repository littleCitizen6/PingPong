using System;
using System.Collections.Generic;
using System.Text;

namespace PingPong.Server.RequestHendlers
{
    public interface IRequestHandler
    {
        public byte[] Handle(string msg);
    }
}
