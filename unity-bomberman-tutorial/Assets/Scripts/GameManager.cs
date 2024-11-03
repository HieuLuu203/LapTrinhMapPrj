using System;
using System.Collections;
using System.Text;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using Networking;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // private UdpClient udpClientSend;   // Client dùng để gửi input qua cổng 4445
    // private UdpClient udpClientReceive; // Client dùng để nhận dữ liệu từ server qua multicast cổng 4446
    public GameObject playerPrefab;
    public string currentPlayerId;  // Lưu ID của player
    public Vector3 startPos;
    private Vector3 posGap;
    public Vector2 gap;

    private Dictionary<string, GameObject> playerObjects = new Dictionary<string, GameObject>();
    
    [SerializeField] private UdpClientHandler udpClientHandler;

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
        udpClientHandler.SetupSender();
        
        // Nhận ID từ server qua cổng 4445 (cùng cổng gửi)
        currentPlayerId = udpClientHandler.ReceivePlayerId();
        Debug.Log("Connected with Player ID: " + currentPlayerId);
        
        // Thiết lập UdpClient cho việc nhận multicast
        udpClientHandler.SetupMulticastReceiver();
        
        // Bắt đầu Coroutine để lắng nghe dữ liệu từ server qua cổng 4446
        StartCoroutine(ReceivePlayerPositions());
    }

    private IEnumerator ReceivePlayerPositions()
    {
        while (true)
        {
            try
            {
                // Lắng nghe gói tin từ server qua multicast cổng 4446
                // var receivedBytes = udpClientHandler.ReceiveMulticastData();
                //
                // var receivedData = Encoding.ASCII.GetString(receivedBytes);
                //
                // Debug.Log($"Received message: {receivedData}");
                //
                // // Xử lý chuỗi vị trí player từ server
                // var playerData = receivedData.Split('|');
                // foreach (var playerInfo in playerData)
                // {
                //     if (string.IsNullOrEmpty(playerInfo)) continue;
                //
                //     var details = playerInfo.Split(';');
                //     var playerId = details[0];
                //
                //     // Xử lý thông tin ngắt kết nối
                //     if (details[1] == "Disconnect")
                //     {
                //         if (playerObjects.ContainsKey(playerId))
                //         {
                //             Destroy(playerObjects[playerId]);
                //             playerObjects.Remove(playerId);
                //         }
                //         continue;
                //     }
                //
                //     var xPos = float.Parse(details[1]);
                //     var yPos = float.Parse(details[2]);
                //
                //     // Kiểm tra và tạo mới đối tượng cho player nếu chưa có
                //     if (!playerObjects.ContainsKey(playerId))
                //     {
                //         var newPlayer = Instantiate(playerPrefab);
                //         playerObjects.Add(playerId, newPlayer);
                //         posGap = new Vector3(xPos * gap.x, yPos*gap.y, 0) - startPos;
                //     }
                //
                //     // Cập nhật vị trí player theo tỷ lệ
                //     playerObjects[playerId].transform.position = new Vector3(
                //         xPos*gap.x, yPos*gap.y, 0
                //     ) - posGap;
                // }
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
        udpClientHandler.SendPlayerInput(currentPlayerId, input, deltaTime);
    }
}
