using UnityEngine;

namespace OnlineGameTest {
    public class BaseSingletonLazy<T> where T : BaseSingletonLazy<T>, new(){
        private static bool IsInitialized => _instance != null;
        private static T _instance;

        public static T Instance {
            get {
                if (!IsInitialized) _instance = new();
                return _instance;
            }
        }
    }
}