using System;
using UnityEngine;

namespace OnlineGameTest {
    
    public class LoadSceneSetting : Singleton<LoadSceneSetting> {
        [SerializeField] private Material defaultSkybox;

        private void Start() {
            DontDestroyOnLoad(gameObject);
        } 
        
        public void LoadSceneData(string sceneName) {
            if (sceneName == SceneSpecificData.WelcomeSceneName) {
                LoadWelcomeSceneSetting();
            }
            // else if (sceneName == GameSystemGlobalVariables.CharacterSelectSceneName) {
            //     LoadGamePlaySceneSetting();
            // }
            else if (sceneName == SceneSpecificData.GameplaySceneName) {
                Load02GamePlaySceneSetting();
            }
            
        }

        private void LoadWelcomeSceneSetting() {
            RenderSettings.skybox = null;
            RenderSettings.ambientLight = Color.black;
        }

        private void Load02GamePlaySceneSetting() {
            RenderSettings.skybox = defaultSkybox;
            RenderSettings.ambientLight = new Color(54, 58, 66); // Default Ambient Color: intensity = 0; rgb only
        }
        
        // CODE FOR TEST
        public bool usingWelcomeScene;
        public bool usingGamePlayScene;
        private void Update() {
            if(usingWelcomeScene) LoadSceneData(SceneSpecificData.WelcomeSceneName);
            else if (usingGamePlayScene) LoadSceneData(SceneSpecificData.GameplaySceneName);
        }
    }
}