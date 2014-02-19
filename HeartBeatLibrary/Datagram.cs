using System;

namespace HeartBeatLibrary
{
    public class Datagram
    {
        public int Priority { get; set; }

        public DateTime SendDateTime { get; set; }

        public Datagram(int priority, DateTime sendDateTime)
        {
            Priority = priority;
            SendDateTime = sendDateTime;
        }

        public override string ToString()
        {
            return "" + Priority + ";" + SendDateTime;
        }

        public static Datagram Parse(string value)
        {
            string[] values = value.Split(';');
            return new Datagram(int.Parse(values[0]), DateTime.Parse(values[1]));
        }
    }
}
