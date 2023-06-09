using Mirror;

namespace OnlineGameTest {
    public class CharacterSpawner : NetworkBehaviour {
        private void Start() {
            // List<NetworkPrefabsList> networkPrefabsList = NetworkManager.Singleton.NetworkConfig.Prefabs.NetworkPrefabsLists;
            // foreach (var networkPrefab in networkPrefabsList) {
            //     // Debug.Log(networkPrefab.PrefabList.ToString());
            //     foreach (var prefab in networkPrefab.PrefabList) {
            //         Debug.Log(prefab.Prefab.ToString());
            //         Instantiate(prefab.Prefab);
            //     }
            // }
            // NetworkPrefabsList networkPrefabsList =
            //     NetworkManager.Singleton.NetworkConfig.Prefabs.NetworkPrefabsLists[0];
            
            
        }

        public void SetupCharacter(int characterId) {
            
        }
    }
}