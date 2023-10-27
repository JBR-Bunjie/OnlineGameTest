// using Mirror;
// using OnlineGameTest;
// using UnityEngine;
//
// public class NetworkManagerController : MonoBehaviour {
//     // [SerializeField] private GameObject[] _playerPrefabList;
//     // [SerializeField] private GameObject[] _toolPrefabList;
//     // private OnlineGameTestNetworkManager _costumeNetworkManager;
//     // private OnlineGameTestKcpTransport _costumeKcpTransport;
//     // private NetworkPingDisplay _pingDisplay;
//     //
//     // public OnlineGameTestNetworkManager CostumeNetworkManager {
//     //     get => _costumeNetworkManager;
//     //     private set => _costumeNetworkManager = value;
//     // }
//     //
//     // public OnlineGameTestKcpTransport CostumeKcpTransport {
//     //     get => _costumeKcpTransport;
//     //     private set => _costumeKcpTransport = value;
//     // }
//     //
//     // public NetworkPingDisplay PingDisplay {
//     //     get => _pingDisplay;
//     //     private set => _pingDisplay = value;
//     // }
//     //
//     // private void Awake() {
//     //     CostumeKcpTransport = gameObject.AddComponent<OnlineGameTestKcpTransport>();
//     //     CostumeNetworkManager = gameObject.AddComponent<OnlineGameTestNetworkManager>();
//     //     PingDisplay = gameObject.AddComponent<NetworkPingDisplay>();
//     //
//     //     CostumeNetworkManager.transport = CostumeKcpTransport;
//     // }
//     //
//     // private void Start() {
//     //     // Setup Default Values
//     //     CostumeNetworkManager.networkAddress = "localhost";
//     //     CostumeKcpTransport.port = (ushort)8888;
//     //
//     //     CostumeNetworkManager.autoCreatePlayer = false;
//     //     // CostumeNetworkManager.offlineScene = 
//     //     
//     //     
//     //     // set up prefabs
//     //     // character prefabs
//     //     RemoteGlobalValues.PrefabPointer = CostumeNetworkManager.spawnPrefabs;
//     //     RemoteGlobalValues.CharacterModelPrefabStartIndex = 0;
//     //     foreach (var singlePrefab in _playerPrefabList) CostumeNetworkManager.spawnPrefabs.Add(singlePrefab);
//     //     RemoteGlobalValues.CharacterModelPrefabsNum = _playerPrefabList.Length;
//     //     // tool prefabs
//     //     RemoteGlobalValues.ToolPrefabStartIndex = CostumeNetworkManager.spawnPrefabs.Count;
//     //     foreach (var singlePrefab in _toolPrefabList) CostumeNetworkManager.spawnPrefabs.Add(singlePrefab);
//     //     RemoteGlobalValues.ToolPrefabsNum = _toolPrefabList.Length;
//     // }
//     //
//     //
//     // public void ServerChangeScene(string sceneName) {
//     //     CostumeNetworkManager.ServerChangeScene(sceneName);
//     // }
//     //
//     // public void StartClient() {
//     //     CostumeNetworkManager.StartClient();
//     // }
//     //
//     // public void StartServer() {
//     //     CostumeNetworkManager.StartServer();
//     // }
//     //
//     // public void StartHost() {
//     //     CostumeNetworkManager.StartHost();
//     // }
// }