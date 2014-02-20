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
            if (values.Length < 2) return null;
            int priority;
            DateTime sendDateTime;
            if (int.TryParse(values[0], out priority) && DateTime.TryParse(values[1], out sendDateTime))
                return new Datagram(priority, sendDateTime);
            return null;
        }
    }
}
