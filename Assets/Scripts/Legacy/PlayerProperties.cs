using UnityEngine;
using UnityEngine.Serialization;

namespace OnlineGameTest.LocalLogic {
    public class PlayerProperties : MonoBehaviour {
        
        #region Player Properties

        [SerializeField] private string _playerName = "True - UnityChan";
        [SerializeField] private float _walkSpeed = 1.4f;
        [SerializeField] private float _runSpeed = 3.0f;
        [SerializeField] private float _jumpForce = 5.0f;
        [SerializeField] private float _rollForce = 4.0f;
        [SerializeField] private int _health = 100;
        [SerializeField] private int _maxHealth = 100;

        
        public string PlayerName {
            get => _playerName;
            set => _playerName = value;
        }
        
        public float WalkSpeed {
            get => _walkSpeed;
            set => _walkSpeed = value;
        }

        public float RunSpeed {
            get => _runSpeed;
            set => _runSpeed = value;
        }

        public float JumpForce {
            get => _jumpForce;
            set => _jumpForce = value;
        }

        public float RollForce {
            get => _rollForce;
            set => _rollForce = value;
        }

        public int Health {
            get => _health;
            set => _health = value;
        }

        public int MaxHealth {
            get => _maxHealth;
            set => _maxHealth = value;
        }

        #endregion

        #region Camera Properties

        [SerializeField] private float cameraSensitive = 60f;
        
        public float CameraSensitive {
            get => cameraSensitive;
            set => cameraSensitive = value;
        }

        #endregion
    }
}