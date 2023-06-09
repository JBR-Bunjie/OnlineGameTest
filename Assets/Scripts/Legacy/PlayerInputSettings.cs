using UnityEngine;

namespace OnlineGameTest.LocalLogic {
    public class PlayerInputSettings : MonoBehaviour {
        #region Control Settings

        [Header("Controller Method")] // "Controller Method"
        [SerializeField] private bool usingKeyboard = false;
        [SerializeField] private bool usingController = true;
        
        public bool UsingKeyboard {
            get => usingKeyboard;
            set => usingKeyboard = value;
        }
        
        public bool UsingController {
            get => usingController;
            set => usingController = value;
        }
        
        #endregion

        #region Flags

        [Header("Flags")]
        [SerializeField] private bool inputEnabled = true;
        [SerializeField] private bool runEnabled = false;

        public bool InputEnabled {
            get => inputEnabled;
            set => inputEnabled = value;
        }
        
        #endregion
        
        #region KeyBoard Input Settings
        
        [Header("KeyBoard With Mouse Input Settings")]
        [SerializeField] private string keyUp = "w";
        [SerializeField] private string keyDown = "s";
        [SerializeField] private string keyRight = "d";
        [SerializeField] private string keyLeft = "a";
        [SerializeField] private KeyCode jump = KeyCode.Space;
        [SerializeField] private KeyCode run = KeyCode.LeftShift;
        [SerializeField] private KeyCode attackMouse = KeyCode.Mouse0;
        [SerializeField] private KeyCode attackKey = KeyCode.F;
        // [SerializeField] private KeyCode Roll = "";
        [SerializeField] private string cameraLookUp = "up";
        [SerializeField] private string cameraLookDown = "down";
        [SerializeField] private string cameraLookLeft = "left";
        [SerializeField] private string cameraLookRight = "right";
        [SerializeField] private bool reverseCameraVertical = false;
        [SerializeField] private string mouseX = "Mouse X";
        [SerializeField] private string mouseY = "Mouse Y";

        public string KeyUp {
            get => keyUp;
            set => keyUp = value;
        }

        public string KeyDown {
            get => keyDown;
            set => keyDown = value;
        }

        public string KeyRight {
            get => keyRight;
            set => keyRight = value;
        }

        public string KeyLeft {
            get => keyLeft;
            set => keyLeft = value;
        }


        public KeyCode Jump {
            get => jump;
            set => jump = value;
        }

        public KeyCode Run {
            get => run;
            set => run = value;
        }

        public KeyCode AttackMouse {
            get => attackMouse;
            set => attackMouse = value;
        }
        
        public KeyCode AttackKey {
            get => attackKey;
            set => attackKey = value;
        }

        public string CameraLookUp {
            get => cameraLookUp;
            set => cameraLookUp = value;
        }
        
        public string CameraLookDown {
            get => cameraLookDown;
            set => cameraLookDown = value;
        }
        
        public string CameraLookLeft {
            get => cameraLookLeft;
            set => cameraLookLeft = value;
        }
        
        public string CameraLookRight {
            get => cameraLookRight;
            set => cameraLookRight = value;
        }
        
        public bool ReverseCameraVertical {
            get => reverseCameraVertical;
            set => reverseCameraVertical = value;
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
        [SerializeField] private KeyCode controllerStart = KeyCode.JoystickButton6;
        // Menu
        [SerializeField] private KeyCode controllerMenu = KeyCode.JoystickButton7;
        
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
            get => controllerMenu;
            set => controllerMenu = value;
        }
        
        public KeyCode ControllerStart {
            get => controllerStart;
            set => controllerStart = value;
        }

        #endregion
        
    }
}