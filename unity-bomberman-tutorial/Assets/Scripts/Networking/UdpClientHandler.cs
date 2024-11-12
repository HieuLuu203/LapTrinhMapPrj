using UnityEngine;

namespace Networking
{
    public class UdpClientHandler : MonoBehaviour
    {
        private UdpBidirectionalConnection _bidirectionalConnection;
        private UdpMulticastReceiver _multicastReceiver;

        // Define a delegate and an event for message reception
        public delegate void MessageReceivedHandler(string message);

        public event MessageReceivedHandler OnMessageReceived;

        private void Awake()
        {
            _bidirectionalConnection = new UdpBidirectionalConnection(OnMessage);
            _multicastReceiver = new UdpMulticastReceiver(OnMessage);
        }

        private void OnDestroy()
        {
            _bidirectionalConnection.Destroy();
            _multicastReceiver.Destroy();
        }

        public void Connect()
        {
            _bidirectionalConnection.Start();
            _bidirectionalConnection.SendMessage("Connect to server");
            
            _multicastReceiver.Start();
        }

        public void Send(string sendMessage)
        {
            _bidirectionalConnection.SendMessage(sendMessage);
        }

        private void OnMessage(string message)
        {
            OnMessageReceived?.Invoke(message); // Raise the event
        }

        // public void SendPlayerInput(string playerId, string input, float deltaTime)
        // {
        //     var message = playerId + "|" + input + "|" + deltaTime;
        //     var messageBytes = Encoding.ASCII.GetBytes(message);
        //     _sender.Send(messageBytes, messageBytes.Length);
        // }
    }
}