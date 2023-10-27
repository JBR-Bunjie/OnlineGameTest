using System;
using System.Collections.Generic;

namespace OnlineGameTest {
    public class EventPool : BaseSingletonIm<EventPool> {
        private readonly Dictionary<string, Action<object>> _pool = new();

        /// <summary>
        /// 在我们的事件中心池子中建立监听
        /// </summary>
        /// <param name="name">监听的事件</param>
        /// <param name="callback">配发的方法(我们收到事件触发时候需要处理的函数)</param>
        public void AddEventListener(string name, Action<object> callback) {
            // 判断我们的事件中心池子中是否存在该事件
            if (_pool.ContainsKey(name)) {
                // 如果存在就直接在对应事件上继续增加监听
                _pool[name] += callback;
            }
            else {
                // 如果不存在就添加一个新事件，并且建立监听
                _pool.Add(name, callback);
            }
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="callback">监听的函数</param>
        public void RemoveEventListener(string name, Action<object> callback) {
            if (_pool.ContainsKey(name)) {
                _pool[name] -= callback;
            }
        }

        /// <summary>
        /// 配发事件(执行监听事件的所有函数)
        /// </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="o">事件内容</param>
        public void TriggerEvent(string name, object o = null) {
            if (_pool.ContainsKey(name) && _pool[name] != null)
                _pool[name].Invoke(o);
        }

        /// <summary>
        /// 这个函数是当我们过场景的时候帮助我们清空事件中心池
        /// </summary>
        public void Clear() {
            _pool.Clear();
        }
    }
}