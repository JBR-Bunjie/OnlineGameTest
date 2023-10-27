using UnityEngine;

namespace NetworkProcessor {
    public class NetworkConfig {
        public readonly string ServerAddress = "http://192.168.0.104:5000";
        
        public static string Account {
            get => PlayerPrefs.GetString("User", "None");
            set => PlayerPrefs.SetString("User", value);
        }
        
        public static string Password {
            get => PlayerPrefs.GetString("Pwd", "None");
            set => PlayerPrefs.SetString("Pwd", value);
        }
        
        // public string Cookie => PlayerPrefs.GetString("Cookie", "None");
    }
}