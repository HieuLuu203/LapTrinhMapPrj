using System;
using System.Collections;
using System.Text;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private UdpClient udpClientSend;   // Client dùng để gửi input qua cổng 4445
    private UdpClient udpClientReceive; // Client dùng để nhận dữ liệu từ server qua multicast cổng 4446
    public GameObject playerPrefab;
    public string currentPlayerId;  // Lưu ID của player
    public Vector3 startPos;
    private Vector3 posGap;
    public Vector2 gap;

    private Dictionary<string, GameObject> playerObjects = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        // Thiết lập UdpClient cho việc gửi
        udpClientSend = new UdpClient();
        udpClientSend.Connect("127.0.0.1", 4445);  // Gửi input qua cổng 4445

        // Gửi yêu cầu kết nối
        byte[] connectMessage = Encoding.ASCII.GetBytes("Connect to server");
        udpClientSend.Send(connectMessage, connectMessage.Length);

        // Nhận ID từ server qua cổng 4445 (cùng cổng gửi)
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        byte[] receivedBytes = udpClientSend.Receive(ref remoteEndPoint);
        currentPlayerId = Encoding.ASCII.GetString(receivedBytes);
        Debug.Log("Connected with Player ID: " + currentPlayerId);

        // Thiết lập UdpClient cho việc nhận multicast
        SetupMulticastReceiver();

        // Bắt đầu Coroutine để lắng nghe dữ liệu từ server qua cổng 4446
        StartCoroutine(ReceivePlayerPositions());
    }

    private void SetupMulticastReceiver()
    {
        // Thiết lập nhận dữ liệu multicast qua cổng 4446
        udpClientReceive = new UdpClient();

        int multicastPort = 4446;
        string multicastGroup = "230.0.0.0";

        // Bind UdpClient nhận multicast
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, multicastPort);
        udpClientReceive.Client.Bind(localEndPoint);

        // Tham gia nhóm multicast
        udpClientReceive.JoinMulticastGroup(IPAddress.Parse(multicastGroup));

        Debug.Log($"Joined multicast group {multicastGroup} on port {multicastPort}.");
    }

    private IEnumerator ReceivePlayerPositions()
    {
        while (true)
        {
            try
            {
                // Lắng nghe gói tin từ server qua multicast cổng 4446
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] receivedBytes = udpClientReceive.Receive(ref remoteEndPoint);
                string receivedData = Encoding.ASCII.GetString(receivedBytes);

                Debug.Log($"Received message: {receivedData}");

                // Xử lý chuỗi vị trí player từ server
                string[] playerData = receivedData.Split('|');
                foreach (string playerInfo in playerData)
                {
                    if (string.IsNullOrEmpty(playerInfo)) continue;

                    string[] details = playerInfo.Split(';');
                    string playerId = details[0];

                    // Xử lý thông tin ngắt kết nối
                    if (details[1] == "Disconnect")
                    {
                        if (playerObjects.ContainsKey(playerId))
                        {
                            Destroy(playerObjects[playerId]);
                            playerObjects.Remove(playerId);
                        }
                        continue;
                    }

                    float xPos = float.Parse(details[1]);
                    float yPos = float.Parse(details[2]);

                    // Kiểm tra và tạo mới đối tượng cho player nếu chưa có
                    if (!playerObjects.ContainsKey(playerId))
                    {
                        GameObject newPlayer = Instantiate(playerPrefab);
                        playerObjects.Add(playerId, newPlayer);
                        posGap = new Vector3(xPos * gap.x, yPos*gap.y, 0) - startPos;
                    }

                    // Cập nhật vị trí player theo tỷ lệ
                    playerObjects[playerId].transform.position = new Vector3(
                        xPos*gap.x, yPos*gap.y, 0
                    ) - posGap;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error receiving multicast data: {ex.Message}");
            }

            yield return null;
        }
    }

    public void SendPlayerInput(string input, float deltaTime)
    {
        // Gửi thông tin input kèm ID hiện tại của player qua cổng 4445
        string message = currentPlayerId + "|" + input + "|" + deltaTime;
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
        udpClientSend.Send(messageBytes, messageBytes.Length);
    }
}
