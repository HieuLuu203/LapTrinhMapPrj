using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private UdpClient udpClient;
    private IPEndPoint serverEndPoint;
    private string currentPlayerId;

    void Start()
    {
        udpClient = new UdpClient();
        serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4445); // Địa chỉ server

        // Gửi gói tin "Connect to server" và nhận id
        SendPacket("Connect to server");
        ReceivePacket();
    }

    void Update()
    {
        // Liên tục nhận dữ liệu từ server
        ReceivePacket();
    }

    public void SendPacket(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        udpClient.Send(data, data.Length, serverEndPoint);
    }

    private void ReceivePacket()
    {
        if (udpClient.Available > 0)
        {
            byte[] data = udpClient.Receive(ref serverEndPoint);
            string message = Encoding.UTF8.GetString(data);

            // Nếu nhận id sau khi kết nối
            if (string.IsNullOrEmpty(currentPlayerId) && !message.Contains("Disconnect"))
            {
                currentPlayerId = message; // Lưu id
            }
            else
            {
                // Xử lý thông tin từ server (vị trí player, disconnect,...)
                ProcessServerMessage(message);
            }
        }
    }

    private void ProcessServerMessage(string message)
    {
        string[] playersData = message.Split('|');
        foreach (var playerData in playersData)
        {
            string[] playerInfo = playerData.Split(';');
            string playerId = playerInfo[0];

            if (playerInfo[1] == "Disconnect")
            {
                // Xử lý khi player ngắt kết nối
                RemovePlayer(playerId);
            }
            else
            {
                // Update vị trí player
                UpdatePlayerPosition(playerId, float.Parse(playerInfo[1]), float.Parse(playerInfo[2]));
            }
        }
    }

    private void UpdatePlayerPosition(string playerId, float x, float y)
    {
        // Code cập nhật vị trí player trên client
    }

    private void RemovePlayer(string playerId)
    {
        // Xử lý khi một player bị loại bỏ
    }

    private void OnApplicationQuit()
    {
        // Khi tắt client, gửi Disconnect
        SendPacket($"Disconnect;{currentPlayerId}");
    }
}
