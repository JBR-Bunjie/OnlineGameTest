using UnityEngine;

namespace OnlineGameTest {
    public static class SearchLocalInstance {
        public static string GetPlayerID(GameObject gameObject) {
            Transform transform = gameObject.transform;

            while (transform.parent != null) transform = transform.parent;

            var root = transform.gameObject;

            return root.GetComponent<ClientData>().ClientId;
        }

        public static PlayerManager GetPlayerManager(string playerId) {
            if (RemoteGlobalValues.PlayerManagers.TryGetValue(playerId, out var manager)) {
                return manager;
            }
            else {
                Debug.LogError("PlayerManager Not Found");
                return null;
            }
        }
    }
}