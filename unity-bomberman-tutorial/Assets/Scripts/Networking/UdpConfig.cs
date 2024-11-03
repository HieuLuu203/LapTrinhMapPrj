using System;
using System.IO;
using UnityEngine;

namespace Networking
{
    [Serializable]
    public class UdpConfig
    {
        public string serverIp;
        public int sendPort;
        public int receivePort;
        public string multicastGroup;
        
        public static string ConfigPath => "Assets/Configs/UdpConfig.json";
        
        public static UdpConfig Load() => JsonUtility.FromJson<UdpConfig>(File.ReadAllText(ConfigPath));
        
        public static UdpConfig sInstance = Load();
    }
}