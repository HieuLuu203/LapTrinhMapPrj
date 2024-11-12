using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Networking
{
    public class UdpMulticastReceiver
    {
        private readonly UdpClient _udpClient;
        private readonly Thread _receiveThread;
        private readonly UdpConfig _config = UdpConfig.sInstance;

        public delegate void MessageHandler(string message);

        private readonly MessageHandler _messageHandler;

        public UdpMulticastReceiver(MessageHandler handler)
        {
            _messageHandler = handler;

            _udpClient = new UdpClient();
            _udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            var localEndPoint = new IPEndPoint(IPAddress.Any, _config.receivePort);
            _udpClient.Client.Bind(localEndPoint);

            _udpClient.JoinMulticastGroup(IPAddress.Parse(_config.multicastGroup));
            Debug.Log($"Joined multicast group {_config.multicastGroup} on port {_config.receivePort}.");

            _receiveThread = new Thread(StartListening)
            {
                IsBackground = true,
                Name = "UdpMulticastReceiverThread"
            };
        }
        
        public void Start()
        {
            _receiveThread.Start();
        }

        private void StartListening()
        {
            Debug.Log("Listening for multicast messages...");
            
            while (true)
            {
                try
                {
                    if (_udpClient.Available > 0)
                    {
                        var remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        var receivedBytes = _udpClient.Receive(ref remoteEndPoint);

                        var receivedMessage = Encoding.ASCII.GetString(receivedBytes);

                        _messageHandler.Invoke(receivedMessage);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        public void Destroy()
        {
            _receiveThread.Abort();
            _udpClient.DropMulticastGroup(IPAddress.Parse(_config.multicastGroup));
            _udpClient.Close();
        }
    }
}