using PingPong.Server.Runners;
using System;

namespace PingPong.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //Runner runner = new SocketRunner(args[0], int.Parse(args[1]));
            Runner runner = new SocketRunner("10.1.0.14", 9000);
            runner.Run();
        }
    }
}
