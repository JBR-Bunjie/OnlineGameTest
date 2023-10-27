using UnityEngine;

namespace OnlineGameTest {
    public class BaseSingletonIm<T> where T : BaseSingletonIm<T>, new(){
        private static bool IsInitialized => _instance != null;
        private static readonly T _instance = new T();
        public static T Instance => _instance;
    }
}