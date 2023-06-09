using System;
using Mirror;
using UnityEngine;
using OnlineGameTest;

namespace OnlineGameTest.Welcome {
    public class WelcomeSubMenu : MonoBehaviour {
        private GameObject _gameObject;
        private GameObject[] _childObjects;

        private string[] _basicMenuStrings;
        private string[] _clientMenuStrings;
        private string[] _serverMenuStrings;
        private string[] _hostMenuStrings;

        private void Awake() {
            _gameObject = transform.gameObject;
            _childObjects = new GameObject[_gameObject.transform.childCount];

            for (int i = 0; i < _gameObject.transform.childCount; i++)
                _childObjects[i] = _gameObject.transform.GetChild(i).gameObject;

            #region Setup SubMenu Strings

            _basicMenuStrings = new string[] {
                GameSystemGlobalVariables.Host,
                GameSystemGlobalVariables.Server,
                GameSystemGlobalVariables.Client,
                GameSystemGlobalVariables.Quit
            };

            _clientMenuStrings = new string[] {
                GameSystemGlobalVariables.ServerAddressInput,
                GameSystemGlobalVariables.ServerPortInput,
                GameSystemGlobalVariables.GamePasswordInput,
                GameSystemGlobalVariables.Connect,
                GameSystemGlobalVariables.BackToWelcomeMenu
            };

            _serverMenuStrings = new string[] {
                GameSystemGlobalVariables.ServerPortInput,
                GameSystemGlobalVariables.StartServer,
                GameSystemGlobalVariables.GamePasswordInput,
                GameSystemGlobalVariables.BackToWelcomeMenu
            };

            _hostMenuStrings = new string[] {
                GameSystemGlobalVariables.ServerPortInput,
                GameSystemGlobalVariables.StartHost,
                GameSystemGlobalVariables.GamePasswordInput,
                GameSystemGlobalVariables.BackToWelcomeMenu
            };

            #endregion
        }

        public void SelectHost() {
            foreach (var child in _childObjects) {
                if (Array.Exists(_basicMenuStrings, s => s == child.name)) child.SetActive(false);
                else if (Array.Exists(_hostMenuStrings, s => s == child.name)) child.SetActive(true);
            }
        }

        public void SelectServer() {
            foreach (var child in _childObjects) {
                if (Array.Exists(_basicMenuStrings, s => s == child.name)) child.SetActive(false);
                else if (Array.Exists(_serverMenuStrings, s => s == child.name)) {
                    child.SetActive(true);
                }
            }
        }

        public void SelectClient() {
            foreach (var child in _childObjects) {
                if (Array.Exists(_basicMenuStrings, s => s == child.name)) child.SetActive(false);
                else if (Array.Exists(_clientMenuStrings, s => s == child.name)) child.SetActive(true);
            }
        }

        public void SelectQuit() {
        #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_STANDALONE)
            Application.Quit();
        #endif
        }

        public void SelectBackToWelcomeMenu() {
            foreach (var child in _childObjects) {
                child.SetActive(Array.Exists(_basicMenuStrings, s => s == child.name));
            }
        }

        // public void GetServerAddressInput() {
        //     Debug.Log("Server Address: " +
        //               GameObject.Find("ServerAddressInput").GetComponent<UnityEngine.UI.InputField>().text);
        // }
        //
        // public void GetClientAddress() {
        //     Debug.Log(NetworkManager.Singleton.NetworkConfig);
        // }
    }
}