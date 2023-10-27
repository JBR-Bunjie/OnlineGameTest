using System;
using System.Collections;
using UnityEngine;

namespace OnlineGameTest {
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {
        public static bool IsInitialized => _instance != null;
        private static T _instance;
        public static T Instance => _instance;
        
        protected virtual void Awake() {
            if (_instance != null) {
                Destroy(gameObject);
            }
            else {
                // GameObject obj = new GameObject();
                // obj.name = typeof(T).ToString() + "Manager";
                // _instance = obj.AddComponent<T>();
                _instance = (T)this; // Should Create a gameobject if needed.
                DontDestroyOnLoad(gameObject);
            }
        }
        
        /* Exposed Functions */
        // Events
        // Update Part:
        private event Action MonoUpdateEvents;
        protected virtual void Update() {
            MonoUpdateEvents?.Invoke();
        }

        public virtual void RegisterUpdateEvent(Action action) {
            MonoUpdateEvents += action;
        }

        public virtual void RemoveUpdateEvent(Action action) {
            MonoUpdateEvents -= action;
        }

        public virtual void UpdateClear() {
            MonoUpdateEvents = null;
        }
        
        // FixUpdate Part:
        private event Action MonoFixedUpdateEvents;
        protected virtual void FixedUpdate() {
            MonoFixedUpdateEvents?.Invoke();
        }

        public virtual void RegisterFixedUpdateEvent(Action action) {
            MonoFixedUpdateEvents += action;
        }
    
        public virtual void RemoveFixedUpdateEvent(Action action) {
            MonoFixedUpdateEvents -= action;
        }

        public virtual void FixUpdateClear() {
            MonoFixedUpdateEvents = null;
        }
        
        // Coroutine
        public virtual Coroutine MonoStartCoroutine(IEnumerator iEnumerator) {
            return StartCoroutine(iEnumerator);
        }
        
        public void DoNotDestroySth(GameObject go) {
            DontDestroyOnLoad(go);
        }
    }
}