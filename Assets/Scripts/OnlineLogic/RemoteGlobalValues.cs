using System.Collections.Generic;
using UnityEngine;

namespace OnlineGameTest {
    public static class RemoteGlobalValues {
        // Prefabs
        public static List<GameObject> PrefabPointer;
        public static int TotalPrefabsNum => CharacterModelPrefabsNum + ToolPrefabsNum;
        public static int CharacterModelPrefabsNum;
        public static int CharacterModelPrefabStartIndex;
        public static int ToolPrefabsNum;
        public static int ToolPrefabStartIndex;


        public static Dictionary<string, PlayerManager> PlayerManagers { get; set; } = new ();
    }
}