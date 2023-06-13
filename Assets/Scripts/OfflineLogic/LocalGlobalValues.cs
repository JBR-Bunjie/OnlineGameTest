using System.Collections.Generic;
using UnityEngine;

namespace OnlineGameTest {
    public abstract class LocalGlobalValues {
        public static Dictionary<string, PlayerManager> PlayerLists = new();
        public static List<string> ClientPlayerIds = new();
        public static int CurrentPlayerNum => ClientPlayerIds.Count;
        public static Dictionary<string, GameObject> PlayerScoreBoardDict = new();
    }
}