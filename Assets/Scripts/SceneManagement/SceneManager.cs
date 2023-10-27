using System;
using System.Collections;
using OnlineGameTest;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum SceneList {
    StartUp = 0,
    Main = 1,
    Loading = 2,
    Home = 3,
    BattleField = 4
}

public class SceneManager {
    private SceneHandler _sceneHandler;

    public SceneManager(SceneList target = SceneList.StartUp) {
        InitHandler(target);
    }

    private void InitHandler(SceneList target) {
        _sceneHandler?.CleanPreviousScene();

        switch (target) {
            case SceneList.StartUp: _sceneHandler = new StartUpSceneHandler(target); break;
            case SceneList.Main: _sceneHandler = new MainSceneHandler(target, _sceneHandler?.UIHandler); break;
            case SceneList.Home: _sceneHandler = new HomeSceneHandler(target, _sceneHandler?.UIHandler); break;
            case SceneList.Loading: _sceneHandler = new LoadingSceneHandler(target, _sceneHandler?.UIHandler); break;
            case SceneList.BattleField: _sceneHandler = new BattleFieldSceneHandler(target, _sceneHandler?.UIHandler); break;
            default: return;
        }
        
        _sceneHandler.ConstructScene();
    }

    /// <summary>
    /// 供外部调用的异步加载场景的方法
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="loadSceneMode">场景加载模式</param>
    /// <param name="beforeLoadingCallback"></param>
    /// <param name="afterLoadingCallback"></param>
    public void LoadAsyncScene(
        SceneList scene,
        LoadSceneMode loadSceneMode = LoadSceneMode.Single,
        Action beforeLoadingCallback = null,
        Action afterLoadingCallback = null
    ) {
        // 异步加载场景
        MonoSystem.Instance.MonoStartCoroutine(IELoadAsyncScene(scene, loadSceneMode, beforeLoadingCallback, afterLoadingCallback));
    }

    IEnumerator IELoadAsyncScene(
        SceneList scene,
        LoadSceneMode loadSceneMode = LoadSceneMode.Single,
        Action beforeLoadingCallback = null,
        Action afterLoadingCallback = null
    ) {
        beforeLoadingCallback?.Invoke();
        
        var op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)scene, loadSceneMode);
        while (!op.isDone) {
            // 这里面就可以用到我们之前写的事件中心处理池，去派发我们的加载进度条事件，降低了耦合性
            EventPool.Instance.TriggerEvent("SceneLoadingProgress", op.progress);
            // 每帧更新进度条
            yield return op.progress;
        }
        
        afterLoadingCallback?.Invoke();
        InitHandler(scene);
    }

    // /// <summary>
    // /// 供外部调用的异步加载场景的方法
    // /// </summary>
    // /// <param name="scene"></param>
    // /// <param name="loadSceneMode">场景加载模式，有Single和Additive两种</param>
    // /// <param name="callback">场景加载完成后需要处理的业务逻辑</param>
    // public void LoadScene(
    //     SceneList scene,
    //     LoadSceneMode loadSceneMode = LoadSceneMode.Single,
    //     Action callback = null
    // ) {
    //     // 同步加载，并在完成后再去处理额外的业务逻辑
    //     UnityEngine.SceneManagement.SceneManager.LoadScene((int)scene, loadSceneMode);
    //     callback?.Invoke();
    // }
}