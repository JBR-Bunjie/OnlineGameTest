using System.Reflection;
using InputDetector;
using NetworkProcessor;
using OnlineGameTest;
using UnityEngine;

public class MonoSystem : MonoSingleton<MonoSystem> {
    private SystemBehaviour _systemBehaviour;

    public ResourcesHandler ResourcesHandler;
    public NetworkHandler NetworkHandler;

    private Assembly _systemHotFixAssembly;

    public Assembly SystemHotFixAssembly {
        get {
            if (_systemHotFixAssembly is null) EventPool.Instance.TriggerEvent(SystemBehaviour.LoadHotFixAssemblyEvent, null);
            return _systemHotFixAssembly;
        }
        set => _systemHotFixAssembly = value;
    }

    public InputHandler GlobalInputHandler => _systemBehaviour.GlobalInput;

    public string ClientVersion {
        get => PlayerPrefs.GetString("ClientVersion", Application.version);
        set => PlayerPrefs.SetString("ClientVersion", value);
    }

    public string DataVersion {
        get => PlayerPrefs.GetString("DataVersion", "0");
        set => PlayerPrefs.SetString("ClientVersion", value);
    }

    public SceneManager SceneMgr { get; private set; }

    protected void Start() {
        // Basic Parts of Game System Start First
        _systemBehaviour = new SystemBehaviour();

        // Network Related Init
        ResourcesHandler = new ResourcesHandler();
        NetworkHandler = new NetworkHandler();

        // SceneManger Init
        SceneMgr = new SceneManager();
    }
}