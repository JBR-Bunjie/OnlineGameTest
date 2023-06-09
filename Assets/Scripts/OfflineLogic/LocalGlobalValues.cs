using UnityEngine;

namespace OnlineGameTest {
    public abstract class LocalGlobalValues : MonoBehaviour{
        private static int _currentPlayerNum = 0;
        public static bool PlayerNumJustChanged { get; set; }
        public static int CurrentPlayerNum {
            get => _currentPlayerNum;
            set {
                if (_currentPlayerNum != value) {
                    PlayerNumJustChanged = true;
                    _currentPlayerNum = value;
                }
            }
        }
    }
}