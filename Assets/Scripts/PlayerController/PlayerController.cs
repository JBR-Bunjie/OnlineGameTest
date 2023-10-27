using System;
using InputDetector;
using UnityEngine;

namespace OnlineGameTest {
    public class PlayerController : MonoBehaviour {
        private PlayerState _playerState;
        private PlayerStateOnGround _groundState;
        private PlayerStateInAir _airState;
        private bool _Aiming;
        
        public Rigidbody _rigidbody;
        
        public PlayerCallbacks PlayerCallbacks;
        // public PlayerInputMapping _playerInputMapping;
        public GameObject TotalHandler;
        public GameObject ModelHandler;
        public GameObject CharacterModel;
        public GameObject GunBitModel;
        public GameObject CameraHandler;
        public GameObject PlayerInfoHandler;
        public Animator CharacterAnimator;
        public Transform GroundSensorTransform;

        // Set in inspector
        public float _MovingFactorTransitionTime = 0.05f;
        public float _AnimationTransitionTime = 0.2f;
        private LayerMask GroundLayerMask;
        
        // Dynamic Value
        public bool IsGrounded;
        public float tempEuler = 0;
        
        private void Start() {
            PlayerCallbacks = new PlayerCallbacks(this);
            _playerState = new PlayerStateOnGround(this);
            _rigidbody = GetComponent<Rigidbody>();

            _groundState = new PlayerStateOnGround(this);
            _airState = new PlayerStateInAir(this);
            
            GroundLayerMask = LayerMask.GetMask("Ground");
            
            // Lock cursor to center and hide it
            Cursor.lockState = CursorLockMode.Locked;

            PlayerBehaviourInject();
        }
            
        private void PlayerBehaviourInject() {
            // TODO: Save Player Settings Into an Archive 
            // Callback Registering
            /* Keyboards */
            // Keys:
            MonoSystem.Instance.GlobalInputHandler.BindDelegateOnPressKeys(V2RInputMapConf.MoveForwardV, PlayerCallbacks.KeyboardPressMoveForward);
            MonoSystem.Instance.GlobalInputHandler.BindDelegateOnPressKeys(V2RInputMapConf.MoveBackV, PlayerCallbacks.KeyboardPressMoveBack);
            MonoSystem.Instance.GlobalInputHandler.BindDelegateOnPressKeys(V2RInputMapConf.MoveRightV, PlayerCallbacks.KeyboardPressMoveRight);
            MonoSystem.Instance.GlobalInputHandler.BindDelegateOnPressKeys(V2RInputMapConf.MoveLeftV, PlayerCallbacks.KeyboardPressMoveLeft);
            MonoSystem.Instance.GlobalInputHandler.BindDelegateOnReleaseKeys(V2RInputMapConf.MoveForwardV, PlayerCallbacks.KeyboardReleaseMoveForward);
            MonoSystem.Instance.GlobalInputHandler.BindDelegateOnReleaseKeys(V2RInputMapConf.MoveBackV, PlayerCallbacks.KeyboardReleaseMoveBack);
            MonoSystem.Instance.GlobalInputHandler.BindDelegateOnReleaseKeys(V2RInputMapConf.MoveRightV, PlayerCallbacks.KeyboardReleaseMoveRight);
            MonoSystem.Instance.GlobalInputHandler.BindDelegateOnReleaseKeys(V2RInputMapConf.MoveLeftV, PlayerCallbacks.KeyboardReleaseMoveLeft);
            
            MonoSystem.Instance.GlobalInputHandler.BindDelegateOnPressKeys(V2RInputMapConf.SprintV, PlayerCallbacks.KeyboardPressSprint);
            MonoSystem.Instance.GlobalInputHandler.BindDelegateOnReleaseKeys(V2RInputMapConf.SprintV, PlayerCallbacks.KeyboardReleaseSprint);
            
            MonoSystem.Instance.GlobalInputHandler.BindDelegateOnPressKeys(V2RInputMapConf.JumpV, PlayerCallbacks.KeyboardJump);

            MonoSystem.Instance.GlobalInputHandler.BindDelegateOnPressKeys(V2RInputMapConf.ChangeCameraFollowingStateV, PlayerCallbacks.CameraStateChange);
            // Axis:
            MonoSystem.Instance.GlobalInputHandler.BindDelegateOnHoldAxis(V2RInputMapConf.MouseXPositiveV, PlayerCallbacks.MouseXPositive);
            MonoSystem.Instance.GlobalInputHandler.BindDelegateOnHoldAxis(V2RInputMapConf.MouseYPositiveV, PlayerCallbacks.MouseYPositive);
            MonoSystem.Instance.GlobalInputHandler.BindDelegateOnHoldAxis(V2RInputMapConf.MouseXNegativeV, PlayerCallbacks.MouseXNegative);
            MonoSystem.Instance.GlobalInputHandler.BindDelegateOnHoldAxis(V2RInputMapConf.MouseYNegativeV, PlayerCallbacks.MouseYNegative);
        }
        

        // Physics.OverlapCapsule也可以做到同样效果，但是使用CheckSphere可以以更少的资源去实现相同的内容
        private void OnGroundCheck() {
            IsGrounded = Physics.CheckSphere(GroundSensorTransform.position, 0.1f, GroundLayerMask);
        }
        
        
        // The State Change Occurs because of two situations:
        // One is the physics state changes, like fall from cliff and then touch the ground again
        // One is the command made by player, like player jump
        void PassiveStateSwitch() {
            if (!_Aiming) {
                if (IsGrounded == false && _playerState.CurrentState != PlayerStatesEnum.State_InAir) {
                    InitiativeStateSwitch(PlayerStatesEnum.State_InAir);
                }
                else if (IsGrounded && _playerState.CurrentState != PlayerStatesEnum.State_OnGround) {
                    InitiativeStateSwitch(PlayerStatesEnum.State_OnGround);
                }
            }
        }

        void InitiativeStateSwitch(PlayerStatesEnum playerStatesEnum) {
            if (playerStatesEnum != _playerState.CurrentState)
                switch (playerStatesEnum) {
                    case PlayerStatesEnum.Empty:
                        break;
                    case PlayerStatesEnum.State_InAir:
                        _airState.ReInit(_rigidbody.velocity, PlayerCallbacks.jumpTriggered);
                        _playerState = _airState;
                        _Aiming = true;
                        break;
                    case PlayerStatesEnum.State_OnGround:
                        _playerState = _groundState;
                        _Aiming = true;
                        break;
                }
        }
        
        void Update() {
            InitiativeStateSwitch(PlayerCallbacks.NextState);
            _playerState.ProcessingGraphics(Time.deltaTime);
        }

        private void FixedUpdate() {
            OnGroundCheck();
            
            PassiveStateSwitch();
            _playerState.ProcessingPhysics(Time.fixedDeltaTime);
            
            TriggerReset();
        }

        void TriggerReset() {
            PlayerCallbacks.jumpTriggered = false;
            PlayerCallbacks.NextState = PlayerStatesEnum.Empty;
            _Aiming = false;
        }
    }
}