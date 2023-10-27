using UnityEngine;

namespace OnlineGameTest {
    public class PlayerStateOnGround : PlayerState {
        public PlayerStateOnGround(PlayerController playerController) : base(playerController) {
            CurrentState = PlayerStatesEnum.State_OnGround;
        }
        
        // store locally, so that we can make sure transform can always be set correctly
        private Vector3 _realTimeVelocity;
        private float _interpolatedSpeedFactor;
        private float _MovingFactorTransitionTime => PlayerController._MovingFactorTransitionTime;
        private float _AnimationTransitionTime => PlayerController._AnimationTransitionTime;

        
        private void PlayerForward() {
            float targetSpeedFactor = PlayerCallbacks.VelocityValue * PlayerCallbacks.TargetForwardValue;
        
            float currentSpeedFactor = Animator.GetFloat(CharacterAnimationString.Forward);
        
            // ----------------------------------------------------------------------
            // TODO: Find out why the Factor is not same as max targetForward Value!
            // ----------------------------------------------------------------------
            _interpolatedSpeedFactor = Mathf.Lerp(currentSpeedFactor, targetSpeedFactor, _MovingFactorTransitionTime);
        
            if (PlayerCallbacks.CameraLock) {
                ModelHandlerTransform.forward = new Vector3(CameraHandlerForward.x, 0, CameraHandlerForward.z);
            }
            else {
                // 记几个游戏的插值思路：
                // 卡拉彼丘是射击游戏，并且由于这个游戏的射击触发的一些原因，会希望角色是和Cam方向一致的
                // 在这里应该是对ModelHandlerTransform.forward向CameraHandlerTransform.Forward做插值，无关输入方向
                // 原神是类RPG的游戏，这里不要求特别高的精确性，我们还需要尽量展示角色的各类动作，保证玩家能从各个角度观察角色
                // 这里就是根据用户输入，基于CameraHandlerTransform.forward和.right做插值
                Vector3 targetForward = ModelHandlerTransform.forward;
                
                // Genshin like
                // targetForward = Vector3.Slerp(
                //     a: targetForward,
                //     b: PlayerAbilities.RealSceneVelocity * _interpolatedSpeedFactor,
                //     _AnimationTransitionTime
                // );
                // Calabiyau like
                targetForward = Vector3.Slerp(
                    a: targetForward,
                    b: new Vector3(CameraHandlerForward.x, 0, CameraHandlerForward.z) * _interpolatedSpeedFactor,
                    _AnimationTransitionTime
                );
                
                ModelHandlerTransform.forward = targetForward.normalized;
            }
        }

        
        private void PlayerTransform() {
            Vector3 forwardRestrict = new Vector3(CameraHandlerForward.x, 0, CameraHandlerForward.z);
            Vector3 rightRestrict = new Vector3(CameraHandlerRight.x, 0, CameraHandlerRight.z);
            
            PlayerCallbacks.Move(forwardRestrict, rightRestrict);
            
            // Change the Velocity
            _realTimeVelocity = new Vector3(
                PlayerCallbacks.RealSceneVelocity.x * _interpolatedSpeedFactor,
                Rigidbody.velocity.y,
                PlayerCallbacks.RealSceneVelocity.z * _interpolatedSpeedFactor
            );
            
            Rigidbody.velocity = _realTimeVelocity;
            // TotalHandlerTransform.position += _realTimeVelocity * Time.fixedDeltaTime;
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
            Animator.SetFloat(CharacterAnimationString.YVelocity, _realTimeVelocity.y);
            Animator.SetFloat(CharacterAnimationString.Forward, _interpolatedSpeedFactor);
        }
    }
}