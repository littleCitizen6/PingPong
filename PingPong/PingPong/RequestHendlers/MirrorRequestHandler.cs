using System;
using System.Collections.Generic;
using System.Text;

namespace PingPong.Server.RequestHendlers
{
    public class MirrorRequestHandler : IRequestHandler
    {
        public byte[] Handle(string data)
        {
            return Encoding.ASCII.GetBytes(data);        
        }
    }
}
