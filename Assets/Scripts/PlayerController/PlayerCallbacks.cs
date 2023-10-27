using UnityEngine;

namespace OnlineGameTest {
    public class PlayerCallbacks {
        #region Player Action Variables
        
        // We maintain two velocity values, in this way we can get a smooth transition between them
        // ideal velocity
        private float _targetVUp;
        private float _targetVRight;
        // current velocity
        private float _currentVUp;
        private float _currentVRight;
        // final interpolated velocity
        private float _velocityUp;
        private float _velocityRight;
        // smooth time
        private float _interpolateTime = 0.2f;
        
        // Exposed Values
        public bool jumpTriggered = false;
        public PlayerStatesEnum NextState = PlayerStatesEnum.Empty;
        private float _velocityValue;
        private Vector3 _realSceneVelocity;
        private const float _targetForwardMax = 3.5f;
        private const float _targetForwardAiming = 1f;
        private float _targetForwardValue = 1.5f;
        // private bool _cameraLock = true;
        private bool _cameraLock = false;
        private float _mouseXPositiveVelocityMag;
        private float _mouseYPositiveVelocityMag;
        private float _mouseXNegativeVelocityMag;
        private float _mouseYNegativeVelocityMag;
        
        public float TargetForwardValue => _targetForwardValue;
        public float VelocityValue => _velocityValue;
        public Vector3 RealSceneVelocity => _realSceneVelocity;
        public bool CameraLock => _cameraLock;
        public float MouseXPositiveVelocityMag => _mouseXPositiveVelocityMag;
        public float MouseYPositiveVelocityMag => _mouseYPositiveVelocityMag;
        public float MouseXNegativeVelocityMag => _mouseXNegativeVelocityMag;
        public float MouseYNegativeVelocityMag => _mouseYNegativeVelocityMag;

        #endregion

        #region Flags Refresh

        public void KeyboardPressMoveForward() {
            _targetVUp += 1f;
        }

        public void KeyboardPressMoveBack() {
            _targetVUp += -1f;
        }

        public void KeyboardReleaseMoveForward() {
            _targetVUp -= 1f;
        }
        
        public void KeyboardReleaseMoveBack() {
            _targetVUp -= -1f;
        }

        public void KeyboardPressMoveRight() {
            _targetVRight += 1f;
        }

        public void KeyboardPressMoveLeft() {
            _targetVRight += -1f;
        }

        public void KeyboardReleaseMoveRight() {
            _targetVRight -= 1f;
        }

        public void KeyboardReleaseMoveLeft() {
            _targetVRight -= -1f;
        }

        public void KeyboardPressSprint() {
            _targetForwardValue = _targetForwardMax;
        }

        public void KeyboardReleaseSprint() {
            _targetForwardValue = 1.3f;
        }

        public void MouseXPositive(float velocityMag) {
            _mouseXPositiveVelocityMag = velocityMag;
        }

        public void MouseXNegative(float velocityMag) {
            _mouseXNegativeVelocityMag = velocityMag;
        }

        public void MouseYPositive(float velocityMag) {
            _mouseYPositiveVelocityMag = velocityMag;
        }

        public void MouseYNegative(float velocityMag) {
            _mouseYNegativeVelocityMag = velocityMag;
        }

        private void KeyboardJumpFlag() {
            jumpTriggered = true;
        }

        private void CameraStateChangeFlag() {
            _cameraLock = !_cameraLock; // Once press button, change the state between free and action camera.
            Debug.Log("CameraLock: " + _cameraLock);
        }

        #endregion

        private PlayerController _playerController;
        public PlayerCallbacks(PlayerController playerController) {
            _playerController = playerController;
        }

        private Transform ModelHandlerTransform => _playerController.ModelHandler.transform;
        private Transform CameraHandlerTransform => _playerController.CameraHandler.transform;
        
        #region Flag Apply
        
        public void Move(Vector3 forward, Vector3 right) {
            // Interpolate
            // _currentVUp = Mathf.SmoothDamp(_currentVUp, _targetVUp, ref _velocityUp, _interpolateTime);
            // _currentVRight = Mathf.SmoothDamp(_currentVRight, _targetVRight, ref _velocityRight, _interpolateTime);
            _currentVUp = _targetVUp;
            _currentVRight = _targetVRight;

            // Remap
            Vector2 idealVelocity = new Vector2(_currentVUp, _currentVRight).SquareToCircleRemap();

            // Get Result in public properties
            _velocityValue = Mathf.Sqrt(idealVelocity.x * idealVelocity.x + idealVelocity.y * idealVelocity.y);
            _realSceneVelocity = idealVelocity.x * forward + idealVelocity.y * right;
        }

        public void KeyboardJump() {
            KeyboardJumpFlag();
            NextState = PlayerStatesEnum.State_InAir;
        }

        public void CameraStateChange() {
            CameraStateChangeFlag();
            _playerController.tempEuler = 0;
            if (_cameraLock) CameraHandlerTransform.forward = ModelHandlerTransform.forward;
            // CameraLock: FPS Like Cam; !CameraLock: RPG Cam;
        }

        #endregion
    }
}
