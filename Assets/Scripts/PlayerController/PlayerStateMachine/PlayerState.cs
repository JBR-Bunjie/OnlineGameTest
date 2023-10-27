using UnityEngine;

namespace OnlineGameTest {
    public enum PlayerStatesEnum {
        State_OnGround,
        State_InAir,
        Empty // For clear
    }
    
    public abstract class PlayerState {
        protected readonly PlayerController PlayerController;
        protected PlayerCallbacks PlayerCallbacks => PlayerController.PlayerCallbacks;
        protected Rigidbody Rigidbody => PlayerController._rigidbody;
        private GameObject TotalHandler => PlayerController.TotalHandler;
        protected Transform TotalHandlerTransform => TotalHandler.transform;
        private GameObject ModelHandler => PlayerController.ModelHandler;
        protected Transform ModelHandlerTransform => ModelHandler.transform;
        private GameObject CameraHandler => PlayerController.CameraHandler;
        private Transform CameraHandlerTransform => CameraHandler.transform;
        protected Vector3 CameraHandlerForward => CameraHandlerTransform.forward;
        protected Vector3 CameraHandlerRight => CameraHandlerTransform.right;
        protected Animator Animator => PlayerController.CharacterAnimator;

        public PlayerStatesEnum CurrentState;
        
        protected PlayerState(PlayerController playerController) {
            PlayerController = playerController;
        }

        public abstract void ProcessingGraphics(float deltaTime);
        public abstract void ProcessingPhysics(float deltaTime);
        protected abstract void AnimatorParameterSetup();

        protected void CameraRotate(float deltaTime) {
            float mouseSensitive = 100f;
            float mouseMoveX = (PlayerCallbacks.MouseXPositiveVelocityMag + PlayerCallbacks.MouseXNegativeVelocityMag * -1) * mouseSensitive * deltaTime;
            float mouseMoveY = (PlayerCallbacks.MouseYPositiveVelocityMag + PlayerCallbacks.MouseYNegativeVelocityMag * -1) * mouseSensitive * deltaTime;
            PlayerController.tempEuler = Mathf.Clamp(PlayerController.tempEuler - mouseMoveY, -30, 45);

            RotateSingleHorizontalTransform(CameraHandlerTransform, mouseMoveX);
            RotateSingleVerticalTransform(CameraHandlerTransform, PlayerController.tempEuler);
        }

        private void RotateSingleHorizontalTransform(Transform transform, float mouseMoveX) {
            transform.Rotate(Vector3.up, mouseMoveX);
        }

        private void RotateSingleVerticalTransform(Transform transform, float tempEuler) {
            transform.eulerAngles = new Vector3(
                tempEuler,
                transform.eulerAngles.y,
                0
            );
        }
    }
}
