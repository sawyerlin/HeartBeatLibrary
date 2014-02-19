using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HeartBeatLibrary
{
    public class HeartBeat
    {
        private readonly List<Remote> _remotes;
        private readonly UdpClient _client;
        private readonly int _priority;

        public bool IsMaster
        {
            get
            {
                bool isMaster = true;

                foreach (var remote in _remotes)
                {
                    if (remote.Data != null && remote.Data.Priority < _priority)
                    {
                        if ((DateTime.Now - remote.Data.SendDateTime) > TimeSpan.FromSeconds(10))
                        {
                            isMaster = false;
                            break;
                        }
                    }
                    else
                    {
                        isMaster = false;
                    }
                }

                return isMaster;
            }
        }

        public HeartBeat(List<Remote> remotes, string address, int port, int priority)
        {
            _remotes = remotes;
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(address), port);
            _client = new UdpClient(endPoint);
            _priority = priority;
            Thread thread = new Thread(Beat);
            thread.Start();
        }

        private void Beat()
        {
            while (true)
            {
                IPEndPoint endPoint = null;
                Thread.Sleep(1000);
                while (true)
                {
                    byte[] rData = _client.Receive(ref endPoint);
                    string rValue = Encoding.UTF8.GetString(rData);
                    foreach (var remote in _remotes)
                    {
                        if (remote.Address == endPoint.Address.ToString() && remote.Port == endPoint.Port)
                            remote.Data = Datagram.Parse(rValue);
                    }
                    string sValue = new Datagram(_priority, DateTime.Now).ToString();
                    byte[] sData = Encoding.UTF8.GetBytes(sValue);
                    _client.Send(sData, sData.Length, endPoint);
                }
            }
        }
    }
}
