using System.IO;
using UnityEngine;
using System.Linq;
using System.Reflection;
using InputDetector;
using OnlineGameTest;

public class SystemFunctions {
    public void StopTheWorld(object o = null) {
        Time.timeScale = o is null ? 0 : (float)o;
    }

    public void WorldContinue(object o = null) {
        Time.timeScale = o is null ? 1 : (float)o;
    }

    public void QuitGame(object o = null) {
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
                Application.Quit();
#endif
    }

    public void LoadHotFixAssembly(object o = null) {
#if !UNITY_EDITOR
        Assembly hotUpdateAss = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/OnlineGameTest.HotFix.dll.bytes"));
#else
        // Editor环境下，HotUpdate.dll.bytes已经被自动加载，直接查找获得HotUpdate程序集
        Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "OnlineGameTest.HotFix");
#endif
        MonoSystem.Instance.SystemHotFixAssembly = hotUpdateAss;
        EventPool.Instance.RemoveEventListener(SystemBehaviour.LoadHotFixAssemblyEvent, LoadHotFixAssembly);
    }
}