using UnityEngine;

namespace OnlineGameTest {
    public class UserSettings {
        #region Flags
        
        [Header("Flags")]
        // Control Method
        [SerializeField] private bool _usingKeyboard = false;
        [SerializeField] private bool _usingController = true;
        // Camera
        [SerializeField] private bool _reverseCameraVertical = false;
        
        public bool UsingKeyboard {
            get => _usingKeyboard;
            set {
                if (value) {
                    // only one of them can be true
                    _usingKeyboard = true;
                    _usingController = false;
                }
                else _usingKeyboard = false;
            }
        }
        
        public bool UsingController {
            get => _usingController;
            set {
                if (value) {
                    // only one of them can be true
                    _usingController = true;
                    _usingKeyboard = false;
                }
                else _usingController = false;
            }
        }
        
        public bool ReverseCameraVertical {
            get => _reverseCameraVertical;
            set => _reverseCameraVertical = value;
        }
        
        // public bool InputEnabled {
        //     get => inputEnabled;
        //     set => inputEnabled = value;
        // }
        // 
        // public bool RunEnabled {
        //     get => runEnabled;
        //     set => runEnabled = value;
        // }
        
        #endregion
        
        #region KeyBoard Input Settings
        
        [Header("KeyBoard With Mouse Input Settings")]
        [SerializeField] private KeyCode _keyW = KeyCode.W;
        [SerializeField] private KeyCode _keyS = KeyCode.S;
        [SerializeField] private KeyCode _keyD = KeyCode.D;
        [SerializeField] private KeyCode _keyA = KeyCode.A;
        [SerializeField] private KeyCode _keyF = KeyCode.F;
        [SerializeField] private KeyCode _keySpace = KeyCode.Space;
        [SerializeField] private KeyCode _keyLeftShift = KeyCode.LeftShift;
        [SerializeField] private KeyCode _mouse0 = KeyCode.Mouse0;
        // [SerializeField] private KeyCode Roll = "";
        [SerializeField] private string _cameraLookUp = "up";
        [SerializeField] private string _cameraLookDown = "down";
        [SerializeField] private string _cameraLookLeft = "left";
        [SerializeField] private string _cameraLookRight = "right";
        [SerializeField] private string _mouseX = "Mouse X";
        [SerializeField] private string _mouseY = "Mouse Y";

        public KeyCode KeyW {
            get => _keyW;
            set => _keyW = value;
        }

        public KeyCode KeyS {
            get => _keyS;
            set => _keyS = value;
        }

        public KeyCode KeyD {
            get => _keyD;
            set => _keyD = value;
        }

        public KeyCode KeyA {
            get => _keyA;
            set => _keyA = value;
        }


        public KeyCode KeySpace {
            get => _keySpace;
            set => _keySpace = value;
        }

        public KeyCode KeyLeftShift {
            get => _keyLeftShift;
            set => _keyLeftShift = value;
        }

        public KeyCode Mouse0 {
            get => _mouse0;
            set => _mouse0 = value;
        }
        
        public KeyCode KeyF {
            get => _keyF;
            set => _keyF = value;
        }

        public string CameraLookUp {
            get => _cameraLookUp;
            set => _cameraLookUp = value;
        }
        
        public string CameraLookDown {
            get => _cameraLookDown;
            set => _cameraLookDown = value;
        }
        
        public string CameraLookLeft {
            get => _cameraLookLeft;
            set => _cameraLookLeft = value;
        }
        
        public string CameraLookRight {
            get => _cameraLookRight;
            set => _cameraLookRight = value;
        }
        
        #endregion
        
        
        #region Controller Settings

        [Header("Controller Input Settings")]
        // 左摇杆
        [SerializeField] private string controllerLX = "Horizontal";
        [SerializeField] private string controllerLY = "Vertical";
        // 右摇杆
        [SerializeField] private string controllerRX = "HorizontalRight";
        [SerializeField] private string controllerRY = "VerticalRight";
        // A,B,X,Y
        [SerializeField] private KeyCode controllerA = KeyCode.JoystickButton0;
        [SerializeField] private KeyCode controllerB = KeyCode.JoystickButton1;
        [SerializeField] private KeyCode controllerX = KeyCode.JoystickButton2;
        [SerializeField] private KeyCode controllerY = KeyCode.JoystickButton3;
        // LB
        [SerializeField] private KeyCode controllerLB = KeyCode.JoystickButton4;
        // RB
        [SerializeField] private KeyCode controllerRB = KeyCode.JoystickButton5;
        // LT,RT
        [SerializeField] private string controllerLRT = "ControllerLRT";
        // START
        [SerializeField] private KeyCode _controllerStart = KeyCode.JoystickButton6;
        // Menu
        [SerializeField] private KeyCode _controllerMenu = KeyCode.JoystickButton7;
        
        public string ControllerLX {
            get => controllerLX;
            set => controllerLX = value;
        }
        
        public string ControllerLY {
            get => controllerLY;
            set => controllerLY = value;
        }
        
        public string ControllerRX {
            get => controllerRX;
            set => controllerRX = value;
        }
        
        public string ControllerRY {
            get => controllerRY;
            set => controllerRY = value;
        }
        
        public KeyCode ControllerA {
            get => controllerA;
            set => controllerA = value;
        }
        
        public KeyCode ControllerB {
            get => controllerB;
            set => controllerB = value;
        }
        
        public KeyCode ControllerX {
            get => controllerX;
            set => controllerX = value;
        }
        
        public KeyCode ControllerY {
            get => controllerY;
            set => controllerY = value;
        }
        
        public KeyCode ControllerLB {
            get => controllerLB;
            set => controllerLB = value;
        }
        
        public KeyCode ControllerRB {
            get => controllerRB;
            set => controllerRB = value;
        }
        
        public string ControllerLRT {
            get => controllerLRT;
            set => controllerLRT = value;
        }
        
        public KeyCode ControllerMenu {
            get => _controllerMenu;
            set => _controllerMenu = value;
        }
        
        public KeyCode ControllerStart {
            get => _controllerStart;
            set => _controllerStart = value;
        }

        #endregion
        
    }
}