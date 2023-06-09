using OnlineGameTest.Bit;
using UnityEngine;

namespace OnlineGameTest {
    public class PlayerInputProcessing : MonoBehaviour {
        // Prepare
        private string PlayerId => SearchLocalInstance.GetPlayerID(gameObject);
        private PlayerManager LocalInstance => SearchLocalInstance.GetPlayerManager(PlayerId);

        
        // outside references
        private UserSettings _userSettings;
        private PlayerStatus.CharacterStates CharacterStates => LocalInstance.CharacterStates;
        private PlayerStatus.GunBitStates GunBitStates => LocalInstance.GunBitStates;
        private CharacterProperties CharacterProperties => LocalInstance.CharacterProperties;
        private GunBitProperties GunBitProperties => LocalInstance.GunBitProperties;
        private UserSettings UserSettings => LocalInstance.UserSettings;
        

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
        [SerializeField] private float _interpolateTime = 0.1f;
        
        
        private float _cameraVerticalInput;
        private float _cameraHorizontalInput;

        private float _velocityValue;
        private Vector3 _realSceneVelocity;
        
        public float CameraVerticalInput {
            get => _cameraVerticalInput;
            set => _cameraVerticalInput = value;
        }

        public float CameraHorizontalInput {
            get => _cameraHorizontalInput;
            set => _cameraHorizontalInput = value;
        }
        
        public float VelocityValue {
            get => _velocityValue;
            set => _velocityValue = value;
        }

        public Vector3 RealSceneVelocity {
            get => _realSceneVelocity;
            set => _realSceneVelocity = value;
        }
        
        #endregion

        #region Keyboard

        private void KeyboardCameraCon() { }

        private void KeyboardGunBitAttack() { }

        private void KeyboardMove() { }

        #endregion

        #region Controller

        void ControllerCameraCon() {
            // Reverse?
            CameraVerticalInput = UserSettings.ReverseCameraVertical ? -1 : 1;

            // Raw Input And Get Result
            CameraVerticalInput *= CharacterProperties.InputEnabled
                ? Input.GetAxis(UserSettings.ControllerRY)
                : 0.0f;

            CameraHorizontalInput = CharacterProperties.InputEnabled
                ? Input.GetAxis(UserSettings.ControllerRX)
                : 0.0f;
        }


        void ControllerMove() {
            // Raw Input
            _targetVUp = CharacterProperties.InputEnabled ? Input.GetAxis(UserSettings.ControllerLY) : 0.0f;
            _targetVRight = CharacterProperties.InputEnabled ? Input.GetAxis(UserSettings.ControllerLX) : 0.0f;

            // Interpolate
            _currentVUp = Mathf.SmoothDamp(_currentVUp, _targetVUp, ref _velocityUp, _interpolateTime);
            _currentVRight = Mathf.SmoothDamp(_currentVRight, _targetVRight, ref _velocityRight, _interpolateTime);

            // Remap
            Vector2 idealVelocity = SquareToCircleRemap(new Vector2(_currentVUp, _currentVRight));

            // Get Result in public properties
            VelocityValue = Mathf.Sqrt(idealVelocity.x * idealVelocity.x + idealVelocity.y * idealVelocity.y);
            RealSceneVelocity = idealVelocity.x * transform.forward + idealVelocity.y * transform.right;

            // Debug.Log("PlayerInputProcessing: "+RealSceneVelocity);
            
            /* ----------------- Set Built-in States ----------------- */
            // Set Moving State
            CharacterStates.Moving = VelocityValue > 0.1f;
            // Set Running State
            CharacterStates.Running = Input.GetKey(UserSettings.ControllerLB);
            // Set WantJumping State
            CharacterStates.WantToJump = Input.GetKeyDown(UserSettings.ControllerA);
        }

        void ControllerGunBitAttack() {
            /* ----------------- Set Built-in States ----------------- */
            GunBitStates.WantToAttack = Input.GetKey(UserSettings.ControllerB);
            // GunBitAnimationString.BuiltInStates.WantToAttack = Input.GetKeyDown(_playerInputSettings.ControllerB);
            //!(Input.GetAxis(_playerInputSettings.ControllerLRT) <= 0.1);
        }

        void ControllerGunBitReload() {
            GunBitStates.WantToReload = Input.GetKeyDown(UserSettings.ControllerX);
        }
        
        #endregion


        #region Shared Functions
        
        private Vector2 SquareToCircleRemap(Vector2 input) {
            Vector2 output = Vector2.zero;

            output.x = input.x * Mathf.Sqrt(1.0f - input.y * input.y / 2.0f);
            output.y = input.y * Mathf.Sqrt(1.0f - input.x * input.x / 2.0f);

            return output;
        }

        #endregion

        void Update() {
            if (CharacterProperties.InputEnabled){
                if (UserSettings.UsingKeyboard) {
                    KeyboardMove();
                    KeyboardGunBitAttack();
                    KeyboardCameraCon();
                }
                else if (UserSettings.UsingController) {
                    // User Right
                    ControllerCameraCon();
                    
                    // Need For RPC
                    ControllerMove();
                    ControllerGunBitAttack();
                    ControllerGunBitReload();
                }
            }
            // TODO: Input Disabled, Load Special Status
            // else {
            //     
            // }
        }
    }
}