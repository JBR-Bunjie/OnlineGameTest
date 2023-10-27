using UnityEngine;

namespace OnlineGameTest {
    public class PlayerStateInAir : PlayerState {
        public PlayerStateInAir(PlayerController playerController) : base(playerController) {
            CurrentState = PlayerStatesEnum.State_InAir;
        }
        
        // store locally, so that we can make sure transform can always be set correctly
        private Vector3 _lastSprintVelocity;
        private bool _jumpTriggeredAnimation;
        private bool _boostVelocityInit;
        private Vector3 _thrustVec;
        private Vector3 _realTimeVelocity;

        public void ReInit(Vector3 velocityFromLastState, bool jumpTriggered) {
            _jumpTriggeredAnimation = jumpTriggered;
            _boostVelocityInit = false;
            _thrustVec = jumpTriggered ? new Vector3(0f, 5f, 0f) : Vector3.zero;
            _lastSprintVelocity = velocityFromLastState + _thrustVec;
            _thrustVec = Vector3.zero;
        }
        
        public override void ProcessingGraphics(float deltaTime) {
            CameraRotate(deltaTime);
            PlayerForward();
            AnimatorParameterSetup();
        }

        public override void ProcessingPhysics(float deltaTime) {
            PlayerTransform();
        }
        
        protected override void AnimatorParameterSetup() {
            Animator.SetBool(CharacterAnimationString.IsGrounded, PlayerController.IsGrounded);
            Animator.SetFloat(CharacterAnimationString.YVelocity, _lastSprintVelocity.y);
            if (_jumpTriggeredAnimation) {
                Animator.SetTrigger(CharacterAnimationString.JumpTrigger);
                _jumpTriggeredAnimation = false;
            }
        }
        
        private void PlayerForward() {
            if (PlayerCallbacks.CameraLock) {
                ModelHandlerTransform.forward = new Vector3(CameraHandlerForward.x, 0, CameraHandlerForward.z);
            }
        }
        
        private void PlayerTransform() {
            Vector3 forwardRestrict = new Vector3(CameraHandlerForward.x, 0, CameraHandlerForward.z);
            Vector3 rightRestrict = new Vector3(CameraHandlerRight.x, 0, CameraHandlerRight.z);
            
            PlayerCallbacks.Move(forwardRestrict, rightRestrict);
            
            if (!_boostVelocityInit) {
                Rigidbody.velocity = _lastSprintVelocity;
                _boostVelocityInit = true;
            }
            else _lastSprintVelocity = Rigidbody.velocity;
        }
    }
}