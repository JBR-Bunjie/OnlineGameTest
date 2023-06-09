using System;
using TMPro;
using UnityEngine;

namespace OnlineGameTest.Welcome {
    public class WelcomeMainMenu : MonoBehaviour {
        #region Sub Menu Data

        [SerializeField] private GameObject _targetManagerObject;
        private NetworkManagerController NetworkManagerController { get; set; }

        private string ServerAddress { get; set; } = "localhost";
        private ushort ServerPort { get; set; } = (ushort)8888;
        private string GamePassword { get; set; }

        #endregion

        private void Awake() {
            NetworkManagerController = _targetManagerObject.GetComponent<NetworkManagerController>();
        }

        public void ServerAddressInput(GameObject passedInGameObject) {
            string inputString = passedInGameObject.GetComponent<TMP_InputField>().text;
            if (inputString == "") {
                ServerAddress = "localhost"; // default value
            }
            else {
                ServerAddress = inputString;
            }

            Debug.Log(ServerAddress);
            NetworkManagerController.CostumeNetworkManager.networkAddress = ServerAddress;
        }

        public void ServerPortInput(GameObject passedInGameObject) {
            string inputString = passedInGameObject.GetComponent<TMP_InputField>().text;
            ushort outValue;

            if (inputString == "") {
                outValue = (ushort)8888; // default value
            }
            else if (ushort.TryParse(inputString, out outValue)) {
                // Debug.Log(outValue);
                ServerPort = outValue;
            }
            // else {
            //     alert("Invalid Port Number, Please Change Your Input");
            // }
            
            Debug.Log(ServerPort);
            NetworkManagerController.CostumeKcpTransport.port = ServerPort;
        }

        public void GamePasswordInput(GameObject passedInGameObject) {
            string inputString = passedInGameObject.GetComponent<TMP_InputField>().text;
            GamePassword = inputString;
            
            Debug.Log(GamePassword);
            NetworkManagerController.CostumeKcpTransport.Password = GamePassword;
        }


        #region Setup Startup UI

        public void StartHostAndLoadNewScene() {
            NetworkManagerController.StartHost();
            NetworkManagerController.ServerChangeScene(
                GameSystemGlobalVariables.GameplaySceneName
            );
        }

        public void StartServerAndLoadNewScene() {
            NetworkManagerController.StartServer();
            NetworkManagerController.ServerChangeScene(
                GameSystemGlobalVariables.GameplaySceneName
            );
        }

        public void StartClientAndLoadNewScene() {
            NetworkManagerController.StartClient();
        }

        #endregion
    }
}