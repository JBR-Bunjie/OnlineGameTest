using System;
using UnityEngine;

namespace OnlineGameTest {
    public class ClientData : MonoBehaviour {
        [SerializeField] private string _clientId;
        public string ClientId => _clientId;
        
        public ClientData() {
            _clientId = Guid.NewGuid().ToString();
        }

        public void RefreshClientGuid() {
            _clientId = Guid.NewGuid().ToString();
        }
    }
}