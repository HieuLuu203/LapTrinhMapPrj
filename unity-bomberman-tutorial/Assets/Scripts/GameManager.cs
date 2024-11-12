using System;
using System.Collections;
using System.Text;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using Networking;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // private UdpClient udpClientSend;   // Client dùng để gửi input qua cổng 4445
    // private UdpClient udpClientReceive; // Client dùng để nhận dữ liệu từ server qua multicast cổng 4446
    public GameObject playerPrefab;
    public GameObject wallPrefab;
    public string currentPlayerId = ""; // Lưu ID của player
    public Vector3 startPos;
    private Vector3 posGap;
    public Vector2 gap;

    private Dictionary<string, GameObject> playerObjects = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> wallObjects = new Dictionary<string, GameObject>();

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

        udpClientHandler.OnMessageReceived += OnServerMessage;
    }

    private void Start()
    {
        udpClientHandler.Connect();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }

        udpClientHandler.OnMessageReceived -= OnServerMessage;
    }

    private void OnServerMessage(string message)
    {
        if (currentPlayerId == "")
        {
            currentPlayerId = message;

            Debug.Log("Connected with Player ID: " + currentPlayerId);

            return;
        }

        MainThreadDispatcher.Enqueue(() => ReceivePlayerPositions(message));
    }

    private void ReceivePlayerPositions(string receivedData)
    {
        try
        {
            Debug.Log($"Received message: {receivedData}");

            string[] entityData = receivedData.Split('|');
            foreach (string entityInfo in entityData)
            {
                if (string.IsNullOrEmpty(entityInfo)) continue;

                string[] details = entityInfo.Split(';');
                string entityId = details[0];
                string entityType = details[1];

                // Xử lý thông tin ngắt kết nối
                if (entityType == "Disconnect")
                {
                    if (playerObjects.ContainsKey(entityId))
                    {
                        Destroy(playerObjects[entityId]);
                        playerObjects.Remove(entityId);
                    }
                    if (wallObjects.ContainsKey(entityId))
                    {
                        Destroy(wallObjects[entityId]);
                        wallObjects.Remove(entityId);
                    }
                    continue;
                }

                float xPos = float.Parse(details[2]);
                float yPos = float.Parse(details[3]);

                if (entityType == "Wall")
                {
                    if (!wallObjects.ContainsKey(entityId))
                    {
                        GameObject newWall = Instantiate(wallPrefab);
                        wallObjects.Add(entityId, newWall);
                    }
                    wallObjects[entityId].transform.position = new Vector3(
                        xPos * 16, yPos * 9, 0
                    );
                }
                else
                {
                    if (!playerObjects.ContainsKey(entityId))
                    {
                        GameObject newPlayer = Instantiate(playerPrefab);
                        playerObjects.Add(entityId, newPlayer);
                        posGap = new Vector3(
                            xPos * 16, yPos * 9, 0
                        ) - startPos;
                    }
                    playerObjects[entityId].transform.position = new Vector3(
                        xPos * 16, yPos * 9, 0
                    ) - posGap;
                }
            }
        }
        catch (Exception ex)

        {
            Debug.LogError($"Error receiving multicast data: {ex.Message}");
        }
    }


    public void SendPlayerInput(string input, float deltaTime)
    {
        udpClientHandler.Send(currentPlayerId + "|" + input + "|" + deltaTime);
    }
}