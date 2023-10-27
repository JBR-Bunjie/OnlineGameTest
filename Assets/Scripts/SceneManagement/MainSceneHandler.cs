using System.Collections.Generic;
using NetworkProcessor;
using UI;

namespace OnlineGameTest {
    public class MainSceneHandler : SceneHandler {
        public MainSceneHandler(SceneList target, UIHandler uiHandler) : base(target, uiHandler) { }

        public override void ConstructScene() {
            base.ConstructScene();
        }

        protected override void InjectSceneSpecifiedFunctionsToEventPool() {
            base.InjectSceneSpecifiedFunctionsToEventPool();
            EventPool.Instance.AddEventListener("Login", Login);
            EventPool.Instance.AddEventListener("Register", Register);
        }

        protected override void RemoveSceneSpecifiedFunctionsToEventPool() {
            base.RemoveSceneSpecifiedFunctionsToEventPool();
            EventPool.Instance.RemoveEventListener("Login", Login);
            EventPool.Instance.RemoveEventListener("Register", Register);
        }

        void Register(object o = null) {
            MonoSystem.Instance.MonoStartCoroutine(
                MonoSystem.Instance.NetworkHandler.UniversalGet(
                    MonoSystem.Instance.NetworkHandler.SerializingGetParams(
                        NetworkHandler.RegisterRoute,
                        GenerateUserInfo()
                    ),
                    null,
                    (value, succeed) => {
                        if ((string)value == "True") {
                            MonoSystem.Instance.SceneMgr.LoadAsyncScene(SceneList.BattleField);
                        }
                        else {
                            UIHandler.DoAlert("Register Failed, Please Check your acc and pwd");
                        }
                    }
                )
            );
        }

        void Login(object o = null) {
            MonoSystem.Instance.MonoStartCoroutine(
                MonoSystem.Instance.NetworkHandler.UniversalGet(
                    MonoSystem.Instance.NetworkHandler.SerializingGetParams(
                        NetworkHandler.LoginRoute,
                        GenerateUserInfo()
                    ),
                    null,
                    (value, succeed) => {
                        if ((string)value == "True") {
                            MonoSystem.Instance.SceneMgr.LoadAsyncScene(SceneList.BattleField);
                        }
                        else {
                            UIHandler.DoAlert("Login Failed, Please Check your acc and pwd");
                        }
                    }
                )
            );
        }

        private Dictionary<string, string> GenerateUserInfo() {
            return new Dictionary<string, string> {
                { "acc", NetworkConfig.Account },
                { "pwd", NetworkConfig.Password }
            };
        }
    }
}