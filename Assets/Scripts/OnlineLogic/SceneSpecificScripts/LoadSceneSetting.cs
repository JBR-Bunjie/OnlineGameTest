using System;
using UnityEngine;

namespace OnlineGameTest {
    // NOTE:
    // Current, this Script is "unchecked" in unity project, because i found that it is not necessary to use this script.
    // At first, the light effect is not correct in my project where the mirror changed the scene.
    // But after i baked the light, the light effect is correct. And Also, I found the code here cannot make effects. 
    // Maybe I should just left this code snippets here, and delete the script in the future. 
    public class LoadSceneSetting : Singleton<LoadSceneSetting> {
        public SceneSpecificData SceneSpecificData { get; set; }
        public static bool NewSceneReady { get; set; }
        private float TOLERANCE = 0.01f;

        private void Start() {
            SceneSpecificData = GetComponent<SceneSpecificData>();
            DontDestroyOnLoad(gameObject);
        }

        public void LoadSceneData(string sceneName) {
            if (sceneName == SceneNavigator.WelcomeSceneName) {
                LoadWelcomeSceneSetting();
            }
            // else if (sceneName == GameSystemGlobalVariables.CharacterSelectSceneName) {
            //     LoadGamePlaySceneSetting();
            // }
            else if (sceneName == SceneNavigator.GameplaySceneName) {
                Load02GamePlaySceneSetting();
            }
            
        }

        private void LoadWelcomeSceneSetting() {
            RenderSettings.skybox = SceneSpecificData.WelcomeSceneSkybox;
            RenderSettings.ambientLight = SceneSpecificData.WelcomeSceneAmbientColor;
            RenderSettings.ambientIntensity = SceneSpecificData.IntensityMultiplierForWelcomeScene;
        }

        private void Load02GamePlaySceneSetting() {
            RenderSettings.skybox = SceneSpecificData.GamePlayerSceneSkybox;
            RenderSettings.ambientLight = SceneSpecificData.GamePlayerSceneAmbientColor;
            RenderSettings.ambientIntensity = SceneSpecificData.IntensityMultiplierForGamePlayerScene;
        }
        
        // CODE FOR TEST
        // public bool usingWelcomeScene;
        // public bool usingGamePlayScene;
        private void Update() {
            // if(usingWelcomeScene) LoadSceneData(SceneNavigator.WelcomeSceneName);
            // else if (usingGamePlayScene) LoadSceneData(SceneNavigator.GameplaySceneName);
            bool setSuccess = false;
            if (SceneSpecificData.CurrentSceneChanged) {
                if (NewSceneReady) {
                    LoadSceneData(SceneSpecificData.CurrentSceneName);
                    setSuccess = CheckSceneInitSuccess();
                }
                
                if (setSuccess) {
                    NewSceneReady = false;
                    SceneSpecificData.CurrentSceneChanged = CheckSceneInitSuccess();
                }
            }
        }

        private bool CheckSceneInitSuccess() {
            bool result = true;
            
            if (SceneSpecificData.CurrentSceneName == SceneNavigator.WelcomeSceneName)
                result = RenderSettings.skybox == SceneSpecificData.WelcomeSceneSkybox &&
                         RenderSettings.ambientLight == SceneSpecificData.WelcomeSceneAmbientColor && 
                         Math.Abs(RenderSettings.ambientIntensity - SceneSpecificData.IntensityMultiplierForWelcomeScene) < TOLERANCE;
            else if (SceneSpecificData.CurrentSceneName == SceneNavigator.GameplaySceneName)
                result = RenderSettings.skybox == SceneSpecificData.WelcomeSceneSkybox &&
                         RenderSettings.ambientLight == SceneSpecificData.GamePlayerSceneAmbientColor && 
                         Math.Abs(RenderSettings.ambientIntensity - SceneSpecificData.IntensityMultiplierForGamePlayerScene) < TOLERANCE;

            return result;
        }
    }
}