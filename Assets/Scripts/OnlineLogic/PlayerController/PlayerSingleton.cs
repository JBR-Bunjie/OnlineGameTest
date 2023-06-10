using Mirror;

namespace OnlineGameTest {
    public class PlayerSingleton<T> : NetworkBehaviour where T : PlayerSingleton<T> {
        private static T _instance;
        public static T Instance => _instance;

        public static bool IsInitialized => _instance != null;

        protected virtual void Awake() {
            if (_instance != null && isLocalPlayer) {
                Destroy(gameObject);
            }
            else {
                _instance = (T)this;
            }
        }

        protected void OnDestroy() {
            if (_instance == this) {
                _instance = null;
            }
        }
    }
}