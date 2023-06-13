using UnityEngine;
using Mirror;
using OnlineGameTest.Bit;
using Vector3 = UnityEngine.Vector3;

namespace OnlineGameTest {
    public class PlayerControllerRPC : NetworkBehaviour {
        // Prepare
        private string PlayerId => SearchLocalInstance.GetPlayerID(gameObject);
        private PlayerManager LocalInstance => SearchLocalInstance.GetPlayerManager(PlayerId);

        // Im
        private bool IsLocalPlayer => LocalInstance.IsLocalPlayer;


        #region References

        // outside references
        private PlayerInputProcessing PlayerInputProcessing => LocalInstance.PlayerInputProcessing;
        private Rigidbody Rigidbody => LocalInstance.Rigidbody;
        private PlayerStatus.CharacterStates CharacterStates => LocalInstance.CharacterStates;
        private CharacterProperties CharacterProperties => LocalInstance.CharacterProperties;
        private PlayerStatus.GunBitStates GunBitStates => LocalInstance.GunBitStates;
        private GunBitProperties GunBitProperties => LocalInstance.GunBitProperties;
        private BitFire BitFire => LocalInstance.BitFire;
        private GameObject TotalHandler => LocalInstance.TotalHandler;
        private GameObject ModelHandler => LocalInstance.ModelHandler;
        private GameObject CharacterModel => LocalInstance.CharacterModel;
        private GameObject GunBitModel => LocalInstance.GunBitModel;
        private GameObject CameraHandler => LocalInstance.CameraHandler;
        private GameObject PlayerInfoHandler => LocalInstance.PlayerInfoHandler;
        private Animator CharacterAnimator => LocalInstance.CharacterAnimator;
        private Animator GunBitAnimator => LocalInstance.GunBitAnimator;
        private Transform ModelHandlerTransform => ModelHandler.transform;

        #endregion


        // Set in inspector
        [SerializeField] private float _MovingFactorTransitionTime = 0.01f;
        [SerializeField] private float _AnimationTransitionTime = 0.2f;

        // store locally, so that we can make sure transform can always be set correctly
        private float _interpolatedSpeedFactor;
        private Vector3 _thrustVec;
        private float _tempEulerX;

        // SyncVars
        [SyncVar] private Vector3 _remoteForward;
        [SyncVar] private float _remoteInterpolatedSpeedFactor;



        #region Network Behaviour

        private void PlayerForward() {
            float targetSpeedFactor = CharacterStates.Moving
                ? CharacterStates.Running
                    ? CharacterProperties.RunSpeed
                    : CharacterProperties.WalkSpeed
                : 0;

            float currentSpeedFactor = CharacterAnimator.GetFloat(CharacterAnimationString.Forward);

            _interpolatedSpeedFactor = Mathf.Lerp(currentSpeedFactor, targetSpeedFactor, _MovingFactorTransitionTime);

            Vector3 targetForward = ModelHandlerTransform.forward;

            if (CharacterStates.Moving)
                targetForward = Vector3.Slerp(
                    a: ModelHandlerTransform.forward,
                    b: PlayerInputProcessing.RealSceneVelocity * _interpolatedSpeedFactor,
                    _AnimationTransitionTime
                );

            ModelHandlerTransform.forward = targetForward;

            /* ----------------- Set Animator ----------------- */
            CharacterAnimator.SetFloat(CharacterAnimationString.Forward, _interpolatedSpeedFactor);

            CmdRemoteForwardSet(targetForward, _interpolatedSpeedFactor);
        }

        [Command]
        private void CmdRemoteForwardSet(Vector3 targetForward, float interpolatedSpeedFactor) {
            _remoteForward = targetForward;
            _remoteInterpolatedSpeedFactor = interpolatedSpeedFactor;
        }

        
        private void ApplyOtherPlayerForward() {
            ModelHandlerTransform.forward = _remoteForward;
            CharacterAnimator.SetFloat(CharacterAnimationString.Forward, _remoteInterpolatedSpeedFactor);
        }


        private void PlayerTransform() {
            // Change the Velocity
            Vector3 realTimeVelocity = new Vector3(
                PlayerInputProcessing.RealSceneVelocity.x * _interpolatedSpeedFactor + _thrustVec.x,
                Rigidbody.velocity.y + _thrustVec.y,
                PlayerInputProcessing.RealSceneVelocity.z * _interpolatedSpeedFactor + _thrustVec.z
            );

            _thrustVec = Vector3.zero;
            
            // Rigidbody.velocity = realTimeVelocity;
            transform.position += realTimeVelocity * Time.fixedDeltaTime;

            /* ----------------- Set Animator ----------------- */
            CharacterAnimator.SetFloat(CharacterAnimationString.YVelocity, realTimeVelocity.y);
        }


        private void PlayerJump() {
            if (CharacterStates.WantToJump) {
                CharacterAnimator.SetTrigger(CharacterAnimationString.JumpTrigger);
                CmdPlayerJumpInvoke();
            }
        }
        
        [Command]
        private void CmdPlayerJumpInvoke() {
            CharacterAnimator.SetTrigger(CharacterAnimationString.JumpTrigger);
            Rigidbody.velocity += Vector3.up * CharacterProperties.JumpForce;
        }
        
        private void RotateTotalHandler() {
            Vector3 modelEuler = ModelHandler.transform.eulerAngles;
            Vector3 playerInfoHandlerEuler = PlayerInfoHandler.transform.eulerAngles;

            TotalHandler.transform.Rotate(
                Vector3.up,
                PlayerInputProcessing.CameraHorizontalInput * CharacterProperties.CameraSensitivity * Time.deltaTime
            );

            _tempEulerX -= PlayerInputProcessing.CameraVerticalInput * CharacterProperties.CameraSensitivity * Time.deltaTime;

            CameraHandler.transform.eulerAngles = new Vector3(
                Mathf.Clamp(_tempEulerX, -30, 40),
                CameraHandler.transform.eulerAngles.y,
                0
            );

            ModelHandler.transform.eulerAngles = modelEuler;
            PlayerInfoHandler.transform.eulerAngles = playerInfoHandlerEuler;
        }

        private void ReversePosition() {
            if (CharacterStates.WantToBackToField) {
                transform.position = new Vector3(0, 1.5f, 0);
                LocalInstance.HealthUpdate(-10, isClientOnly);
                ClearVelocity();
            }
        }
        
        [Command]
        private void ClearVelocity() {
            Rigidbody.velocity = Vector3.zero;
        }
        
        private void GunBitFire() {
            if (GunBitStates.WantToAttack) {
                CmdGunBitFireRpc();
            }
        }

        [Command]
        private void CmdGunBitFireRpc() {
            BitFire.FireBullet(
                worldInitialPosition: GunBitModel.transform.position,
                gunBitPointer: GunBitModel.transform.forward,
                gunBitProperties: GunBitProperties
            );
        }
        
        private void GunBitReload() {
            if (GunBitStates.WantToReload)
                CmdGunBitReloadRpc();
        }

        [Command]
        private void CmdGunBitReloadRpc() {
            BitFire.GunBitReload();
        }

        private void QuitGame() {
            if (CharacterStates.WantToQuitGame) {
                #if (UNITY_EDITOR)
                    UnityEditor.EditorApplication.isPlaying = false;
                #elif (UNITY_STANDALONE)
                    Application.Quit();
                #endif
            }
        }
        
        #endregion

        void Update() {
            if (IsLocalPlayer) {
                // Player
                PlayerForward();
                PlayerJump();
                ReversePosition();
                RotateTotalHandler();

                // GunBit
                GunBitFire();
                GunBitReload();
            }
            else {
                ApplyOtherPlayerForward();
            }

            QuitGame();
        }

        private void FixedUpdate() {
            if (IsLocalPlayer) {
                PlayerTransform();
            }
        }
    }
}