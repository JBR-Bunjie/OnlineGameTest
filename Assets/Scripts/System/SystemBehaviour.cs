
using InputDetector;
using OnlineGameTest;
using UnityEngine;

public class SystemBehaviour {
    public const string LoadHotFixAssemblyEvent = "LoadHotFixAssembly";
    public const string QuitGameEvent = "QuitGame";
    public const string StopTheWorldEvent = "StopTheWorld";
    public const string WorldContinueEvent = "WorldContinue";
    public const string InjectCharacterActionsEvent = "InjectCharacterActions";

    // The Functions Affect Entire Game
    public readonly InputHandler GlobalInput;

    public SystemBehaviour() {
        GlobalInput = new InputHandler();
        var systemFunctions = new SystemFunctions();

        // More Local Init Situations in Further Time
        EventPool.Instance.AddEventListener(LoadHotFixAssemblyEvent, systemFunctions.LoadHotFixAssembly);
            
        // Global SYSTEM Behaviour
        EventPool.Instance.AddEventListener(QuitGameEvent, systemFunctions.QuitGame);
        EventPool.Instance.AddEventListener(StopTheWorldEvent, systemFunctions.StopTheWorld);
        EventPool.Instance.AddEventListener(WorldContinueEvent, systemFunctions.WorldContinue);
        

        // SYSTEM EVENTS REGISTER
        GlobalInput.BindDelegateOnPressKeys(V2RInputMapConf.QuitGameKBV, () => EventPool.Instance.TriggerEvent(QuitGameEvent, null));
        GlobalInput.BindDelegateOnPressKeys(V2RInputMapConf.QuitGameJSV, () => EventPool.Instance.TriggerEvent(QuitGameEvent, null));

        // For Update Events:
        MonoSystem.Instance.RegisterUpdateEvent(GlobalInput.GetInput);
    }
}