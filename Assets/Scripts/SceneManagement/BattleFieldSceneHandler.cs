using System;
using System.Collections;
using System.Reflection;
using UI;

namespace OnlineGameTest {
    public class BattleFieldSceneHandler : SceneHandler{
        public BattleFieldSceneHandler(SceneList target, UIHandler uiHandler = null) : base(target, uiHandler) {
            HotfixAssembly = MonoSystem.Instance.SystemHotFixAssembly.GetType("ResMgr");
            // HotfixAssembly.GetMethod("Hello")?.Invoke(null, null);
            // object hotfix = MonoSystem.Instance.SystemHotFixAssembly.CreateInstance("ResMgr");
            object hotfix = Activator.CreateInstance(HotfixAssembly);
            MethodInfo method = HotfixAssembly.GetMethod("LoadAssetsIEnumerator");
            MonoSystem.Instance.MonoStartCoroutine((IEnumerator)method.Invoke(hotfix, new object[]{"Assets/ABResources/Prefabs/HumanoridPlayerPrefab/PlayerHandler_OnlinePlayer_UnityChanUss.prefab"}));
        }
    }
}