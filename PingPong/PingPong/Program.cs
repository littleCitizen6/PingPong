using PingPong.Server.Runners;
using System;

namespace PingPong.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Runner runner = new SocketRunner(args[0], int.Parse(args[1]));
            runner.Run();
        }
    }
}
