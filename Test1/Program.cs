using System;
using System.Collections.Generic;
using System.Threading;
using HeartBeatLibrary;

namespace Test1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Remote> remotes = new List<Remote>
                {
                    new Remote("192.168.102.118", 8011),
                    new Remote("192.168.102.118", 8012)
                };
            HeartBeat heartBeat = new HeartBeat(remotes, "192.168.102.118", 8010, 0);

            while (true)
            {
                if (heartBeat.IsMaster)
                {
                    Console.WriteLine("8010 is Master");
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
