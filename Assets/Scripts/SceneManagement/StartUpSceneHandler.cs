using NetworkProcessor;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OnlineGameTest {
    public class StartUpSceneHandler : SceneHandler {
        public StartUpSceneHandler(SceneList target) : base(target) { }

        public override void ConstructScene() {
            base.ConstructScene();

            MonoSystem.Instance.MonoStartCoroutine(
                MonoSystem.Instance.NetworkHandler.UniversalGet(
                    NetworkHandler.DataVerInterface,
                    StartUpProgressCallback,
                    GameDataVersionCheckSuffixCallback
                )
            );

            // MonoSystem.Instance.MonoStartCoroutine(
            //     MonoSystem.Instance.NetworkHandler.UniversalGet(
            //         "/client_version",
            //         // VersionCheckProgressCallback,
            //         // VersionCheckSuffixCallback
            //     )
            // );
        }

        private void StartUpProgressCallback(object value) {
            EventPool.Instance.TriggerEvent(UIHandler.StartUpLoadingProgress, value);
        }

        private void GameDataVersionCheckSuffixCallback(object value, bool succeed) {
            string backString = (string)value;
            PlayerPrefs.SetString("RemoteDataVersion", backString);

            // Failed At Requesting
            if (!succeed) {
                // _uiHandler.Warn(NetworkError + "\n" + backString);
                Debug.Log(NetworkError + "\n" + backString);
                return;
            }

            // Game is Up2Date
            CheckGameResources(MonoSystem.Instance.DataVersion == backString);
            
            // Last Situation, we need to update the client.
        }

        private void CheckGameResources(bool usingPrevCatalog) {
            // Refresh The Title
            EventPool.Instance.TriggerEvent(UIHandler.StrippingNextStage, 1);

            // Do A quick check with previous catalog
            MonoSystem.Instance.MonoStartCoroutine(
                MonoSystem.Instance.ResourcesHandler.CheckGameABResources(
                    usingPrevCatalog,
                    StartUpProgressCallback,
                    CheckGameResourcesSuffixCallback
                )
            );
        }

        void CheckGameResourcesSuffixCallback(string text, bool networkError, bool succeed) {
            if (networkError) {
                // _uiHandler.Warn(NetworkError + "\n" + text);
                Debug.Log(NetworkError + "\n" + text);
                return;
            }

            if (!succeed) {
                UIHandler.DoAlert(
                    "There is newer game data needing to be downloaded. \n Press \'Confirm\' to update.",
                    DownloadABResources,
                    () => EventPool.Instance.TriggerEvent(SystemBehaviour.QuitGameEvent, null)
                );
                return;
            }
            
            // check succeed, we dont need to do any resources downloading.
            Debug.Log(text);
            MonoSystem.Instance.DataVersion = PlayerPrefs.GetString("RemoteDataVersion");
            MonoSystem.Instance.SceneMgr.LoadAsyncScene(SceneList.Main);
        }

        private void DownloadABResources() {
            EventPool.Instance.TriggerEvent(UIHandler.StrippingNextStage, 2);
            MonoSystem.Instance.MonoStartCoroutine(
                MonoSystem.Instance.ResourcesHandler.GetGameABResources(
                    // false,
                    StartUpProgressCallback,
                    DownloadABResourcesSuffixCallback
                )
            );
        }

        void DownloadABResourcesSuffixCallback(string text, bool succeed) {
            // Failed At Requesting
            if (!succeed) {
                // _uiHandler.Warn(NetworkError + ": \n" + text);
                Debug.Log(NetworkError + ": \n" + text);
                return;
            }

            CheckGameResources(false);
        }
    }
}