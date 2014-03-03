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

        public override string ToString()
        {
            return string.Format("Address: {0}, Port: {1}, Priority: {2}, DateTime: {3}", Address, Port, Data.Priority,
                                 Data.SendDateTime);
        }
    }
}
