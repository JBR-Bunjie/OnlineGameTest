using Mirror;
using UnityEngine;

namespace OnlineGameTest {
    public class OnlineGameTestNetworkManager : NetworkManager {
        private GameObject _thisPlayerPrefab;
        private GameObject _prePlayerObject;
        
        
        public override void OnStartServer() {
            base.OnStartServer();
            RemoteGlobalValues.PrefabPointer = spawnPrefabs;

            NetworkServer.RegisterHandler<CharacterProperties>(OnCreateCharacter);
        }
        
        // public override void OnServerAddPlayer(NetworkConnectionToClient conn) {
        //     Debug.Log("==================Yes!==================");
        //     Debug.Log(spawnPrefabs[0].ToString());
        //     // add player at correct spawn position
        //     _thisPlayerPrefab = spawnPrefabs[numPlayers % spawnPrefabs.Count];
        //     Transform start = startPositions[Random.Range(0, startPositions.Count)];
        //     GameObject player = Instantiate(_thisPlayerPrefab, start.position, start.rotation);
        //     NetworkServer.AddPlayerForConnection(conn, player);
        // }
        
        public override void OnServerDisconnect(NetworkConnectionToClient conn) {
            
            
            // call base functionality (actually destroys the player)
            base.OnServerDisconnect(conn);
            
        }

        void OnCreateCharacter(NetworkConnectionToClient conn, CharacterProperties properties) {
            GameObject go = Instantiate(spawnPrefabs[numPlayers % RemoteGlobalValues.CharacterModelPrefabsNum]);
            // spawnPrefabs.Find("Player")
            
            NetworkServer.AddPlayerForConnection(conn, go);
        }
        
        public override void OnClientConnect() {
            base.OnClientConnect();
            Debug.Log("Client connected");

            CharacterProperties _characterProperties = new CharacterProperties() {
                PlayerName = "True - UnityChan",
            };
            
            NetworkClient.Send(_characterProperties);
            LocalGlobalValues.CurrentPlayerNum++;
        }
        
        public override void OnClientDisconnect() {
            LocalGlobalValues.CurrentPlayerNum--;
        }
    }
}
