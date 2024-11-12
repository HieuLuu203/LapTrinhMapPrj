using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Networking
{
    public class UdpBidirectionalConnection
    {
        private readonly UdpClient _udpClient;
        private readonly IPEndPoint _endPoint;
        
        private readonly Thread _receiveThread;

        private readonly UdpConfig _config = UdpConfig.sInstance;
        
        public delegate void MessageHandler(string message);

        private readonly MessageHandler _messageHandler;

        public UdpBidirectionalConnection(MessageHandler handler)
        {
            _messageHandler = handler;
            
            _udpClient = new UdpClient();
            _endPoint = new IPEndPoint(IPAddress.Parse(_config.serverIp), _config.sendPort);
            
            _receiveThread = new Thread(StartListening)
            {
                IsBackground = true,
                Name = "UdpBidirectionalConnectionThread"
            };
        }
        
        public void Start()
        {
            _receiveThread.Start();
        }

        public void SendMessage(string message)
        {
            var messageBytes = Encoding.ASCII.GetBytes(message);
            _udpClient.Send(messageBytes, messageBytes.Length, _endPoint);
            Debug.Log($"Message sent: {message}");
        }
        
        private void StartListening()
        {
            Debug.Log("Listening for messages...");
            while (true)
            {
                try
                {
                    var remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    var receivedBytes = _udpClient.Receive(ref remoteEndPoint);

                    var receivedMessage = Encoding.ASCII.GetString(receivedBytes);
                
                    _messageHandler.Invoke(receivedMessage);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e);
                }
            }
        }

        public void Destroy()
        {
            _receiveThread.Abort();
            _udpClient.Close();
        }
    }
}