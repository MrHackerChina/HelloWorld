using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSever;

namespace GameSeverMain
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerPeer server = new ServerPeer();
            //指定应用层
            server.SetApplication(new NetMsgCenter());
            server.Start(9999, 10);
            Console.ReadKey();
        }
    }
}
