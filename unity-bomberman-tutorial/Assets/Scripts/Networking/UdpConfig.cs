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
        
        public static string ConfigPath => "Configs/UdpConfig";

        public static UdpConfig Load()
        {
            var configText = Resources.Load<TextAsset>(ConfigPath);
            return JsonUtility.FromJson<UdpConfig>(configText.text);
        }
        
        public static UdpConfig sInstance = Load();
    }
}