using UnityEngine;
using OnlineGameTest.LocalLogic.Bit;
using UnityEngine.Serialization;

namespace OnlineGameTest.LocalLogic {
    public class AnimataionController : MonoBehaviour {
        // Set By "GetComponent"
        private PlayerInputProcessing _playerInputProcessing;
        private PlayerProperties _playerProperties;
        private Rigidbody _rigidbody;
        private GunBitProperties _gunBitProperties;
        
        // Set By "Inspector"
        [FormerlySerializedAs("_totalHandler")] [SerializeField] private GameObject modelHandler;
        [SerializeField] private GameObject _characterModel;
        [SerializeField] private GameObject _gunBitModel;
        [SerializeField] private Animator _characterAnimator;
        [SerializeField] private Animator _gunBitAnimator;
        
        [SerializeField] private float _MovingFactorTransitionTime = 0.1f;
        [SerializeField] private float _AnimationTransitionTime = 0.1f;

        private float _interpolatedSpeedFactor;
        private Vector3 _thrustVec;

        public GameObject ModelHandler => modelHandler;
        public GameObject CharacterModel => _characterModel;
        public GameObject GunBitModel => _gunBitModel;

        #region Functions
        
        void PlayerJump() {
            /* ----------------- Set Animator ----------------- */
            // check and set jump trigger is true
            if (_playerInputProcessing.CharacterStates.WantToJump) {
                _characterAnimator.SetTrigger(CharacterAnimationString.JumpTrigger);
                _thrustVec = new Vector3(0, _playerProperties.JumpForce, 0);
            }
        }


        /// <summary>
        /// PlayerForward Only Change the Model's Direction, which will be used in Update.
        /// And the Velocity is changed in FixedUpdate()
        /// </summary>
        void PlayerForward() {
            // interpolate forward value and set it to animator
            float targetSpeedFactor = _playerInputProcessing.CharacterStates.Moving
                ? _playerInputProcessing.CharacterStates.Running
                    ? _playerProperties.RunSpeed
                    : _playerProperties.WalkSpeed
                : 0.0f;

            float currentSpeedFactor = _characterAnimator.GetFloat(CharacterAnimationString.Forward);

            _interpolatedSpeedFactor = Mathf.Lerp(currentSpeedFactor, targetSpeedFactor, _MovingFactorTransitionTime);


            // Set model transform.forward to change the player's direction
            if (_playerInputProcessing.CharacterStates.Moving)
                ModelHandler.transform.forward = Vector3.Slerp(
                    ModelHandler.transform.forward,
                    _playerInputProcessing.RealSceneVelocity * _interpolatedSpeedFactor,
                    _AnimationTransitionTime
                );

            /* ----------------- Set Animator ----------------- */
            _characterAnimator.SetFloat(CharacterAnimationString.Forward, _interpolatedSpeedFactor);
        }

        void GunBitOpenFire() {
            /* ----------------- Set Animator ----------------- */
            if (_playerInputProcessing.GunBitStates.WantToAttack) {
                // _characterAnimator.SetTrigger(AnimationString.AttackTrigger);

                // _gunBitAnimator.SetBool(GunBitAnimationString.Attack, GunBitAnimationString.BuiltInStates.WantToAttack);
                GunBitFire.FireBullet(
                    gunBitPointer:_gunBitModel.transform.forward, 
                    worldInitialPosition:_gunBitModel.transform.position,
                    gunBitProperties:_gunBitProperties
                );
            }

            // _characterAnimator.SetLayerWeight(
            //     _characterAnimator.GetLayerIndex(AnimationString.AttackLayer),
            //     _characterAnimator.GetBool(AnimationString.BuiltInStates.IsAttackingString) 
            //         ? 1.0f 
            //         : 0.0f
            //     );
        }
        
        void GunBitReload() {
            /* ----------------- Set Animator ----------------- */
            if (_playerInputProcessing.GunBitStates.WantToReload) {
                // _gunBitAnimator.SetTrigger(GunBitAnimationString.ReloadTrigger);
                GunBitFire.GunBitReload();
            }
        }

        #endregion
        
        private void Awake() {
            _playerInputProcessing = GetComponent<PlayerInputProcessing>();
            _playerProperties = GetComponent<PlayerProperties>();
            _rigidbody = GetComponent<Rigidbody>();
            _gunBitProperties = GetComponent<GunBitProperties>();
        }

        private void Update() {
            // Player Actions
            PlayerForward();
            PlayerJump();
            
            // Gun Bit Actions
            GunBitOpenFire();
            GunBitReload();
        }

        private void FixedUpdate() {
            // Change the Velocity
            Vector3 realTimeVelocity = new Vector3(
                _playerInputProcessing.RealSceneVelocity.x * _interpolatedSpeedFactor + _thrustVec.x,
                _rigidbody.velocity.y + _thrustVec.y,
                _playerInputProcessing.RealSceneVelocity.z * _interpolatedSpeedFactor + _thrustVec.z
            );
            _rigidbody.velocity = realTimeVelocity;
            _thrustVec = Vector3.zero;


            /* ----------------- Set Animator ----------------- */
            _characterAnimator.SetFloat(CharacterAnimationString.YVelocity, realTimeVelocity.y);
        }
    }
}