using Mirror;
using OnlineGameTest;
using OnlineGameTest;
using OnlineGameTest.Bit;
using UnityEngine;
using TMPro;

namespace OnlineGameTest{
    public class PlayerManager : Singleton<PlayerManager> {
        [SerializeField] public bool _isLocalPlayer;
        public bool IsLocalPlayer => isLocalPlayer;

        [Header("Script Set")]
        // Components Just in Script
        [SerializeField] private PlayerStatus.CharacterStates _characterStates;
        [SerializeField] private PlayerStatus.GunBitStates _gunBitStates;
        [SerializeField] private CharacterProperties _characterProperties;
        [SerializeField] private GunBitProperties _gunBitProperties;
        [SerializeField] private UserSettings _userSettings;
        [SerializeField] private ClientData _clientData;
        
        public PlayerStatus.CharacterStates CharacterStates => _characterStates;
        public PlayerStatus.GunBitStates GunBitStates => _gunBitStates;
        public CharacterProperties CharacterProperties => _characterProperties;
        public GunBitProperties GunBitProperties => _gunBitProperties;
        public UserSettings UserSettings => _userSettings;
        public ClientData ClientData => _clientData;

        // Components In Handler
        [SerializeField] private PlayerInputProcessing _playerInputProcessing;
        [SerializeField] private PlayerControllerRPC _playerControllerRPC;
        [SerializeField] private BitFire _bitFire;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _transform;
        [SerializeField] private NetworkIdentity _networkIdentity;

        public PlayerInputProcessing PlayerInputProcessing => _playerInputProcessing;
        public PlayerControllerRPC PlayerControllerRPC => _playerControllerRPC;
        public BitFire BitFire => _bitFire;
        public Rigidbody Rigidbody => _rigidbody;
        public Transform Transform => _transform;
        public NetworkIdentity NetworkIdentity => _networkIdentity;
        
        
        [Header("Inspector Set")]
        // GameObjects Structure Set By "Inspector"
        [SerializeField] private GameObject _totalHandler;
        [SerializeField] private GameObject _modelHandler;
        [SerializeField] private GameObject _characterModel;
        [SerializeField] private GameObject _gunBitModel;
        [SerializeField] private GameObject _cameraHandler;
        [SerializeField] private GameObject _cameraPos;
        [SerializeField] private GameObject _playerInfoHandler;
        [SerializeField] private TMP_Text _playerNameText;
        [SerializeField] private Animator _characterAnimator;
        [SerializeField] private Animator _gunBitAnimator;

        
        public GameObject TotalHandler => _totalHandler;
        public GameObject ModelHandler => _modelHandler;
        public GameObject CharacterModel => _characterModel;
        public GameObject GunBitModel => _gunBitModel;
        public GameObject CameraHandler => _cameraHandler;
        public GameObject CameraPos => _cameraPos;
        public GameObject PlayerInfoHandler => _playerInfoHandler;
        public TMP_Text PlayerNameText => _playerNameText;
        public Animator CharacterAnimator => _characterAnimator;
        public Animator GunBitAnimator => _gunBitAnimator;

        protected override void Awake() {
            // Base work
            base.Awake();
            
            // Set up components
            _userSettings = new UserSettings();
            _characterStates = new PlayerStatus.CharacterStates();
            _gunBitStates = new PlayerStatus.GunBitStates();
            _characterProperties = new CharacterProperties();
            _gunBitProperties = new GunBitProperties();
            
            // Get components
            _playerInputProcessing = GetComponent<PlayerInputProcessing>();
            _playerControllerRPC = GetComponent<PlayerControllerRPC>();
            _bitFire = GetComponent<BitFire>();
            
            _rigidbody = GetComponent<Rigidbody>();
            _transform = GetComponent<Transform>();
            _networkIdentity = GetComponent<NetworkIdentity>();
            _clientData = GetComponent<ClientData>();
            
            
            // initialize some components' data
            _clientData.RefreshClientGuid();
            
            CharacterPropertiesInit();
            
            RemoteGlobalValues.PlayerManagers.Add(_clientData.ClientId, Instance);
        }


        private void CharacterPropertiesInit() {
            _characterProperties.PlayerName = "True - UnityChan";
            _characterProperties.WalkSpeed = 1.4f;
            _characterProperties.RunSpeed =  3.0f;
            _characterProperties.JumpForce = 5.0f;
            _characterProperties.Health = 100;
            _characterProperties.MaxHealth = 100;
            _characterProperties.InputEnabled = true;
            _characterProperties.Damage = 10;
            _characterProperties.CameraSensitivity = 60f;
            _characterProperties.RunEnabled = true;
            // _characterProperties.JumpEnabled = ;
            // _characterProperties.GunBitAttackEnabled = ;
            // _characterProperties.GunBitAttackDamage = ;
        }

        private void Start() {
            if (IsLocalPlayer) {
                UIPanelHandler.PlayerReady = true;
                UIPanelHandler.TrackingTarget = this;
            }
        }

        private void Update() {
            // For test
            _isLocalPlayer = IsLocalPlayer;
        }
    }
}