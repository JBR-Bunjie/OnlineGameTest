using UnityEngine;
using UnityEngine.Serialization;

namespace OnlineGameTest {
    public class SceneSpecificData : MonoBehaviour {
        // Scene Status
        [SerializeField] private string _currentSceneName;
        public bool CurrentSceneChanged { get; set; }
        public string CurrentSceneName {
            get { return _currentSceneName; }
            set {
                if (value != _currentSceneName) {
                    _currentSceneName = value;
                    CurrentSceneChanged = true;
                }
            }
        }
        
        [SerializeField] private Material _welcomeSceneSkybox = null;
        [SerializeField] private float _intensityMultiplierForWelcomeScene = 0.0f;
        [SerializeField] private Color _welcomeSceneAmbientColor = Color.black;
        [SerializeField] private Material _gamePlayerSceneSkybox;
        [SerializeField] private float _intensityMultiplierForGamePlayerScene = 0.5f;
        // Default Ambient Color: intensity = 0; rgb only
        [SerializeField] private Color _gamePlayerSceneAmbientColor = new(54, 58, 66);
        
        
        public Material WelcomeSceneSkybox => _welcomeSceneSkybox;
        public float IntensityMultiplierForWelcomeScene => _intensityMultiplierForWelcomeScene;
        public Color WelcomeSceneAmbientColor => _welcomeSceneAmbientColor;
        public Material GamePlayerSceneSkybox => _gamePlayerSceneSkybox;
        public float IntensityMultiplierForGamePlayerScene => _intensityMultiplierForGamePlayerScene;
        public Color GamePlayerSceneAmbientColor => _gamePlayerSceneAmbientColor;
    }
}