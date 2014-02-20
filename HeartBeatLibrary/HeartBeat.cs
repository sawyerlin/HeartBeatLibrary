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
        private DateTime _startDateTime;

        public bool IsMaster
        {
            get
            {
                bool isAlone = true;
                bool isPriority = true;
                bool isMaster = false;

                foreach (var remote in _remotes)
                {
                    if (remote.Data != null)
                    {
                        isAlone = false;
                        if (remote.Data.Priority < _priority)
                            isPriority = false;
                    }
                }

                if (isAlone)
                {
                    if (DateTime.Now - _startDateTime >= TimeSpan.FromSeconds(10))
                        isMaster = true;
                }
                else
                {
                    _startDateTime = DateTime.Now;
                    if (isPriority)
                        isMaster = true;
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
            Thread threadSend = new Thread(BeatSend),
                threadReceive = new Thread(BeatReceive);
            threadSend.Start();
            threadReceive.Start();
            _startDateTime = DateTime.Now;
        }

        private void BeatSend()
        {
            while (true)
            {
                string sValue = new Datagram(_priority, DateTime.Now).ToString();
                byte[] sData = Encoding.UTF8.GetBytes(sValue);
                foreach (var remote in _remotes)
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(remote.Address), remote.Port);
                    _client.Send(sData, sData.Length, endPoint);
                }
            }
        }

        private void BeatReceive()
        {
            IPEndPoint endPoint = null;
            while (true)
            {
                try
                {
                    byte[] rData = _client.Receive(ref endPoint);
                    string rValue = Encoding.UTF8.GetString(rData);
                    foreach (var remote in _remotes)
                    {
                        if (remote.Address == endPoint.Address.ToString() && remote.Port == endPoint.Port)
                            remote.Data = Datagram.Parse(rValue);
                    }
                }
                catch (Exception)
                {
                    foreach (var remote in _remotes)
                    {
                        if (remote.Data != null && DateTime.Now - remote.Data.SendDateTime > TimeSpan.FromSeconds(10))
                            remote.Data = null;
                    }
                }
            }
        }
    }
}