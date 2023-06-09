using UnityEngine;

namespace OnlineGameTest {
    public class CameraHandlerController : MonoBehaviour {
        // [SerializeField] private float _cameraHeightHigh = 10.0f;
        // [SerializeField] private float _cameraHeightNormal = 8.0f;
        // [SerializeField] private float _cameraHeightLow = 5.0f;
        //
        // [SerializeField] private float _cameraDistanceFar = 10.0f;
        // [SerializeField] private float _cameraDistanceNormal = 8.0f;
        // [SerializeField] private float _cameraDistanceClose = 5.0f;
        
        // Refere
        private string PlayerId => SearchLocalInstance.GetPlayerID(gameObject);
        private PlayerManager LocalInstance => SearchLocalInstance.GetPlayerManager(PlayerId);


        [SerializeField] private GameObject _mainCamera;
        [SerializeField] private float _smoothTime = 0.1f;
        private Vector3 _smoothVelocity;
        private Vector3 _smoothRotation;
        private float _eulerAngleX;

        
        // References
        private bool IsLocalPlayer => LocalInstance.IsLocalPlayer; 
        private GameObject CameraPos => LocalInstance.CameraPos;

        
        private void Awake() {
            if (Camera.main != null) _mainCamera = Camera.main.gameObject;
        }

        private void RotateLocalMainCamera() {
            _mainCamera.transform.position = Vector3.SmoothDamp(
                current: _mainCamera.transform.position,
                target: CameraPos.transform.position, 
                ref _smoothVelocity, 
                _smoothTime
            );
            
            _mainCamera.transform.eulerAngles = CameraPos.transform.eulerAngles;
        }

        private void FixedUpdate() {
            if (_mainCamera && IsLocalPlayer) {
                RotateLocalMainCamera();
            }
        }
    }
}