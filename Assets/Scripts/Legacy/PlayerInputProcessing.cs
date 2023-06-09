using System;
using System.Collections;
using UnityEngine;

namespace OnlineGameTest.LocalLogic {
    public class PlayerInputProcessing : MonoBehaviour {
        [SerializeField] private PlayerInputSettings _playerInputSettings;

        
        [SerializeField] private PlayerStatus.CharacterStates _characterStates;
        [SerializeField] private PlayerStatus.GunBitStates _gunBitStates;

        public PlayerStatus.CharacterStates CharacterStates {
            get => _characterStates;
            set => _characterStates = value;
        }

        public PlayerStatus.GunBitStates GunBitStates {
            get => _gunBitStates;
            set => _gunBitStates = value;
        }


        #region Player Moving Velocity

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
        [SerializeField] private float _interpolateTime = 0.1f;

        #endregion

        #region Camera Moving Catch

        private float _cameraVerticalInput;
        private float _cameraHorizontalInput;

        public float CameraVerticalInput {
            get => _cameraVerticalInput;
            set => _cameraVerticalInput = value;
        }

        public float CameraHorizontalInput {
            get => _cameraHorizontalInput;
            set => _cameraHorizontalInput = value;
        }

        #endregion

        #region Player Action

        private float _velocityValue;
        private Vector3 _realSceneVelocity;

        public float VelocityValue {
            get => _velocityValue;
            set => _velocityValue = value;
        }

        public Vector3 RealSceneVelocity {
            get => _realSceneVelocity;
            set => _realSceneVelocity = value;
        }

        #endregion


        #region Controller Input Functions

        void ControllerCameraCon() {
            // Reverse?
            CameraVerticalInput = _playerInputSettings.ReverseCameraVertical ? -1 : 1;

            // Raw Input And Get Result
            CameraVerticalInput *= _playerInputSettings.InputEnabled
                ? Input.GetAxis(_playerInputSettings.ControllerRY)
                : 0.0f;

            CameraHorizontalInput = _playerInputSettings.InputEnabled
                ? Input.GetAxis(_playerInputSettings.ControllerRX)
                : 0.0f;

            // CameraVerticalInput 
        }

        void ControllerMove() {
            // Raw Input
            _targetVUp = _playerInputSettings.InputEnabled ? Input.GetAxis(_playerInputSettings.ControllerLY) : 0.0f;
            _targetVRight = _playerInputSettings.InputEnabled ? Input.GetAxis(_playerInputSettings.ControllerLX) : 0.0f;

            // Interpolate
            _currentVUp = Mathf.SmoothDamp(_currentVUp, _targetVUp, ref _velocityUp, _interpolateTime);
            _currentVRight = Mathf.SmoothDamp(_currentVRight, _targetVRight, ref _velocityRight, _interpolateTime);

            // Remap
            Vector2 idealVelocity = SquareToCircleRemap(new Vector2(_currentVUp, _currentVRight));

            // Get Result in public properties
            VelocityValue = Mathf.Sqrt(idealVelocity.x * idealVelocity.x + idealVelocity.y * idealVelocity.y);
            RealSceneVelocity = idealVelocity.x * transform.forward + idealVelocity.y * transform.right;

            /* ----------------- Set Built-in States ----------------- */
            // Set Moving State
            CharacterStates.Moving = VelocityValue > 0.1f;
            // Set Running State
            CharacterStates.Running = Input.GetKey(_playerInputSettings.ControllerLB);
            // Set WantJumping State
            CharacterStates.WantToJump = Input.GetKeyDown(_playerInputSettings.ControllerA);
        }


        void ControllerGunBitAttack() {
            /* ----------------- Set Built-in States ----------------- */
            GunBitStates.WantToAttack = Input.GetKey(_playerInputSettings.ControllerB);
            // GunBitAnimationString.BuiltInStates.WantToAttack = Input.GetKeyDown(_playerInputSettings.ControllerB);
            //!(Input.GetAxis(_playerInputSettings.ControllerLRT) <= 0.1);
        }

        void ControllerGunBitReload() {
            GunBitStates.WantToReload = Input.GetKeyDown(_playerInputSettings.ControllerX);
        }

        #endregion

        #region Keyboard Input Functions

        void KeyboardCameraCon() {
            // Reverse?
            CameraVerticalInput = _playerInputSettings.ReverseCameraVertical ? -1 : 1;

            // Raw Input And Get Result
            CameraVerticalInput *= _playerInputSettings.InputEnabled
                ? (Input.GetKey(_playerInputSettings.CameraLookUp) ? 1.0f : 0.0f) -
                  (Input.GetKey(_playerInputSettings.CameraLookDown) ? 1.0f : 0.0f)
                : 0.0f;

            CameraHorizontalInput = _playerInputSettings.InputEnabled
                ? (Input.GetKey(_playerInputSettings.CameraLookRight) ? 1.0f : 0.0f) -
                  (Input.GetKey(_playerInputSettings.CameraLookLeft) ? 1.0f : 0.0f)
                : 0.0f;

            // CameraVerticalInput 
        }

        void KeyboardMove() {
            // Raw Input
            _targetVUp = _playerInputSettings.InputEnabled
                ? (Input.GetKey(_playerInputSettings.KeyUp) ? 1.0f : 0.0f) -
                  (Input.GetKey(_playerInputSettings.KeyDown) ? 1.0f : 0.0f)
                : 0.0f;
            _targetVRight = _playerInputSettings.InputEnabled
                ? (Input.GetKey(_playerInputSettings.KeyRight) ? 1.0f : 0.0f) -
                  (Input.GetKey(_playerInputSettings.KeyLeft) ? 1.0f : 0.0f)
                : 0.0f;

            // Interpolate
            _currentVUp = Mathf.SmoothDamp(_currentVUp, _targetVUp, ref _velocityUp, _interpolateTime);
            _currentVRight = Mathf.SmoothDamp(_currentVRight, _targetVRight, ref _velocityRight, _interpolateTime);

            // Remap
            Vector2 idealVelocity = SquareToCircleRemap(new Vector2(_currentVUp, _currentVRight));

            // Get Result in public properties
            VelocityValue = Mathf.Sqrt(idealVelocity.x * idealVelocity.x + idealVelocity.y * idealVelocity.y);
            RealSceneVelocity = idealVelocity.x * transform.forward + idealVelocity.y * transform.right;

            /* ----------------- Set Built-in States ----------------- */
            // Set Moving State
            CharacterStates.Moving = VelocityValue > 0.1f;
            // Set Running State
            CharacterStates.Running = Input.GetKey(_playerInputSettings.Run);
            // Set WantJumping State
            CharacterStates.WantToJump = Input.GetKeyDown(_playerInputSettings.Jump);
        }


        void KeyboardGunBitAttack() {
            /* ----------------- Set Built-in States ----------------- */
            GunBitStates.WantToAttack = Input.GetKeyDown(_playerInputSettings.AttackKey) ||
                                                               Input.GetKeyDown(_playerInputSettings.AttackMouse);
        }

        #endregion


        #region Shared Functions

        private Vector2 SquareToCircleRemap(Vector2 input) {
            Vector2 output = Vector2.zero;

            output.x = input.x * Mathf.Sqrt(1.0f - input.y * input.y / 2.0f);
            output.y = input.y * Mathf.Sqrt(1.0f - input.x * input.x / 2.0f);

            return output;
        }

        IEnumerator CheckInput() {
            if (Input.anyKey)
                foreach (KeyCode ckeycode in Enum.GetValues(typeof(KeyCode))) {
                    if (Input.GetKeyDown(ckeycode)) {
                        Debug.Log(ckeycode.ToString());
                        yield return null;
                    }
                }
        }

        #endregion


        private void Awake() {
            _playerInputSettings = GetComponent<PlayerInputSettings>();
        }

        private void Update() {
            if (_playerInputSettings.InputEnabled) {
                StartCoroutine(CheckInput());
                if (_playerInputSettings.UsingKeyboard) {
                    KeyboardMove();
                    KeyboardGunBitAttack();
                    KeyboardCameraCon();
                }
                else if (_playerInputSettings.UsingController) {
                    ControllerMove();
                    ControllerGunBitAttack();
                    ControllerCameraCon();
                    ControllerGunBitReload();
                }
            }
        }
    }
}