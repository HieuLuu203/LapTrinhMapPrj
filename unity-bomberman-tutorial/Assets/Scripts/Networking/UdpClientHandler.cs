using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Networking
{
    public class UdpClientHandler : MonoBehaviour
    {
        [SerializeField]
        private UdpSender sender;
        [SerializeField]
        private UdpReceiver receiver;
        // [SerializeField]
        // private UdpMulticastReceiver multicastReceiver;
        // private UdpClient _sender;
        // private UdpClient _receiver;
        // private readonly UdpConfig _config = UdpConfig.sInstance;

        public void SetupSender()
        {
            // // Thiết lập UdpClient cho việc gửi
            // _sender = new UdpClient();
            // _sender.Connect(_config.serverIp, _config.sendPort);

            // Gửi yêu cầu kết nối
            // var connectMessage = Encoding.ASCII.GetBytes($"Connect to server");
            // _sender.Send(connectMessage, connectMessage.Length);
            sender.SendMessage("Connect to server");
        }

        public string ReceivePlayerId()
        {
            // var remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            // var receivedBytes = _sender.Receive(ref remoteEndPoint);
            // return Encoding.ASCII.GetString(receivedBytes);
            return "";
        }

        public void SetupMulticastReceiver()
        {
            // _receiver = new UdpClient();
            //
            // var localEndPoint = new IPEndPoint(IPAddress.Any, _config.receivePort);
            // _receiver.Client.Bind(localEndPoint);
            //
            // _receiver.JoinMulticastGroup(IPAddress.Parse(_config.multicastGroup));
            // Debug.Log($"Joined multicast group {_config.multicastGroup} on port {_config.receivePort}.");
        }


        public byte[] ReceiveMulticastData()
        {
            // var remoteEndPoint = new IPEndPoint(IPAddress.Any, _config.receivePort);
            // return _receiver.Receive(ref remoteEndPoint);
            // var remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            // var receivedBytes = await _receiver.ReceiveAsync();
            // return receivedBytes.Buffer;
            return Array.Empty<byte>();
        }

        public void SendPlayerInput(string playerId, string input, float deltaTime)
        {
            // var message = playerId + "|" + input + "|" + deltaTime;
            // var messageBytes = Encoding.ASCII.GetBytes(message);
            // _sender.Send(messageBytes, messageBytes.Length);
        }
    }
}