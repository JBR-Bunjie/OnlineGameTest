using System;
using TMPro;
using UnityEngine;

namespace OnlineGameTest {
    public class PlayerScoreApply : MonoBehaviour {
        [SerializeField] private TMP_Text _thisPlayerNameText;
        [SerializeField] private TMP_Text _thisPlayerScore;
        
        public string targetPlayerId;
        public PlayerManager targetPlayerManager;
        
        private void UpdateSinglePanelScore(string clientPlayerId, PlayerManager playerManager) {
            var scoreCalculate = playerManager.GetComponent<ScoreCalculate>();
                
            var playerName = playerManager.CharacterProperties.PlayerName;
            var score = scoreCalculate.Score;
                
            _thisPlayerNameText.text = playerName;
            _thisPlayerScore.text = score.ToString();
        }

        public void Refresh() {
            UpdateSinglePanelScore(targetPlayerId, targetPlayerManager);
        }
    }
}