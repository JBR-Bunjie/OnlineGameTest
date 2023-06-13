using System.Linq;
using UnityEngine;

namespace OnlineGameTest {
    public static class SearchLocalInstance {
        public static string GetPlayerID(GameObject gameObject) {
            Transform transform = gameObject.transform;

            while (transform.parent != null) transform = transform.parent;

            var root = transform.gameObject;

            string clientID = root.GetComponent<ClientData>().ClientId;

            if (!LocalGlobalValues.PlayerLists.Keys.Contains(clientID)) {
                LocalGlobalValues.PlayerLists.Add(clientID, root.GetComponent<PlayerManager>());
                LocalGlobalValues.ClientPlayerIds.Add(clientID);
            }

            return clientID;
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