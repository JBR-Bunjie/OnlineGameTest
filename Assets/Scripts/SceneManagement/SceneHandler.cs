using System;
using UI;

namespace OnlineGameTest {
    public class SceneHandler {
        protected const string NetworkError = "Network Error";
        public readonly UIHandler UIHandler;
        protected Type HotfixAssembly; 

        protected SceneHandler(SceneList target, UIHandler uiHandler = null) {
            switch (target) {
                case SceneList.StartUp: UIHandler = new StartUpUIHandler(); break;
                case SceneList.Main: UIHandler = new MainUIHandler(uiHandler); break;
                // case SceneList.Home: _sceneHandler = new (); break;
                // case SceneList.Loading: _sceneHandler = new (); break;
                case SceneList.BattleField: UIHandler = new BattleFieldUIHandler(uiHandler); break;
            }
        }
        
        public virtual void ConstructScene() {
            UIHandler.GraphicsFadeIn();
            UIHandler.InjectSceneSpecifiedUIFunctionToEventPool();
            InjectSceneSpecifiedFunctionsToEventPool();
        }

        public virtual void CleanPreviousScene() {
            UIHandler.GraphicsFadeOut();
            UIHandler.RemoveSceneSpecifiedUIFunctionFromEventPool();
            UIHandler.RemoveCurrentSceneUIEntityComponents();
            RemoveSceneSpecifiedFunctionsToEventPool();
        }

        protected virtual void InjectSceneSpecifiedFunctionsToEventPool() {}
        protected virtual void RemoveSceneSpecifiedFunctionsToEventPool() {}
    }
}