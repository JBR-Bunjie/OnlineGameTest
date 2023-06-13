using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace OnlineGameTest {
    public class ScoreCalculate : NetworkBehaviour {
        // Prepare
        private string PlayerId => SearchLocalInstance.GetPlayerID(gameObject);
        private PlayerManager LocalInstance => SearchLocalInstance.GetPlayerManager(PlayerId);
        
        public int Score => LocalInstance.CharacterProperties.Health;
        
    }
}
