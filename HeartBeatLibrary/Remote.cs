using System.Net;

namespace HeartBeatLibrary
{
    public class Remote
    {
        public string Address { get; private set; }
        public int Port { get; private set; }
        public Datagram Data { get; set; }

        public Remote(string address, int port)
        {
            Address = address;
            Port = port;
        }
    }
}
