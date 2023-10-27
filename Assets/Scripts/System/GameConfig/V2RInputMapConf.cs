using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InputDetector {
    /// <summary>
    /// Config Virtual Keys 2 Real Keys And Virtual Axis to Real Axis Here.
    /// All Virtual Keys Has Been Exposed, Which Has 'V' Suffix
    /// </summary>
    public class V2RInputMapConf {
        // Virtual Keys
        // Keys
        public const string MoveForwardV = "MoveForward";
        public const string MoveBackV = "MoveBack";
        public const string MoveLeftV = "MoveLeft";
        public const string MoveRightV = "MoveRight";
        public const string JumpV = "Jump";
        public const string SprintV = "Sprint";
        public const string ChangeCameraFollowingStateV = "ChangeCameraFollowingState";
        public const string AimingV = "Aiming";
        // Axis
        public const string MouseXPositiveV = "MouseXPositive";
        public const string MouseXNegativeV = "MouseXNegative";
        public const string MouseYPositiveV = "MouseYPositive";
        public const string MouseYNegativeV = "MouseYNegative";
        // Special Keys
        public const string QuitGameKBV = "QuitGameKeyboard";
        public const string QuitGameJSV = "QuitGameGamePad";
        
        // Real Keys
        // Keys
        public string MoveForward;
        public string MoveBack;
        public string MoveLeft;
        public string MoveRight;
        public string Jump;
        public string Sprint;
        public string ChangeCameraFollowingState;
        public string Aiming;
        // Axis
        public string MouseXPositive;
        public string MouseXNegative;
        public string MouseYPositive;
        public string MouseYNegative;
        // Special Keys
        public string QuitGameKB;
        public string QuitGameJS;
        
        // Keys + Axis (Settings) == TwoWayBindingV2R
        public Dictionary<string, string> KeysSettings;
        public Dictionary<string, string> AxisSettings;
        public Dictionary<string, string> TwoWayBindingR2V;

        private void ReadKeyMapSettings() {
            // Read Previous Config:
            ChangeCameraFollowingState = PlayerPrefs.GetString(ChangeCameraFollowingStateV, "None");
            MoveForward = PlayerPrefs.GetString(MoveForwardV, "None");
            MoveBack = PlayerPrefs.GetString(MoveBackV, "None");
            MoveLeft = PlayerPrefs.GetString(MoveLeftV, "None");
            MoveRight = PlayerPrefs.GetString(MoveRightV, "None");
            Jump = PlayerPrefs.GetString(JumpV, "None");
            Sprint = PlayerPrefs.GetString(SprintV, "None");
            Aiming = PlayerPrefs.GetString(AimingV, "None");
            MouseXPositive = PlayerPrefs.GetString(MouseXPositiveV, "None");
            MouseXNegative = PlayerPrefs.GetString(MouseXNegativeV, "None");
            MouseYPositive = PlayerPrefs.GetString(MouseYPositiveV, "None");
            MouseYNegative = PlayerPrefs.GetString(MouseYNegativeV, "None");
            QuitGameKB = PlayerPrefs.GetString(QuitGameKBV, "None");
            QuitGameJS = PlayerPrefs.GetString(QuitGameJSV, "None");
        }

        private void ResetKeyMappers() {
            PlayerPrefs.SetString(MoveForwardV, "W");
            PlayerPrefs.SetString(MoveBackV, "S");
            PlayerPrefs.SetString(MoveLeftV, "A");
            PlayerPrefs.SetString(MoveRightV, "D");
            PlayerPrefs.SetString(JumpV, "Space");
            PlayerPrefs.SetString(SprintV, "LeftShift");
            PlayerPrefs.SetString(ChangeCameraFollowingStateV, "Tab");
            PlayerPrefs.SetString(AimingV, "Z");
            PlayerPrefs.SetString(QuitGameKBV, "Escape");
            PlayerPrefs.SetString(QuitGameJSV, "JoystickButton7");
            
            PlayerPrefs.SetString(MouseXPositiveV, "Mouse X Positive");
            PlayerPrefs.SetString(MouseXNegativeV, "Mouse X Negative");
            PlayerPrefs.SetString(MouseYPositiveV, "Mouse Y Positive");
            PlayerPrefs.SetString(MouseYNegativeV, "Mouse Y Negative");
        }

        public void SetNewV2RMapRule(string targetVirtualKey, KeyCode keyCode) {
            string inputRealKey = keyCode.ToString();

            string prevCorrespondingRk = KeysSettings[targetVirtualKey];
            if (prevCorrespondingRk != "None") {
                // this virtual key must have been bind to one real key:
                KeysSettings[targetVirtualKey] = "None";
                TwoWayBindingR2V.Remove(prevCorrespondingRk);
                // PlayerPrefs.SetString(targetVirtualKey, "None");
            }
            
            foreach (var rKey in TwoWayBindingR2V.Keys.Where(key => key == inputRealKey)) {
                // If there is one key equaling the inputRealKey
                var correspondingVk = TwoWayBindingR2V[rKey];
                
                // TODO: RECORD THIS UNBIND VIRTUAL KEY AND TRIGGER ALERT
                KeysSettings[correspondingVk] = "None";
                TwoWayBindingR2V.Remove(inputRealKey);
                PlayerPrefs.SetString(correspondingVk, "None");
            }
            
            // Then we should Add this key with its value in settings:
            KeysSettings[targetVirtualKey] = inputRealKey;
            TwoWayBindingR2V[inputRealKey] = targetVirtualKey;
            PlayerPrefs.SetString(targetVirtualKey, inputRealKey);
            // TODO: INJECT THIS FUNCTION INTO EVENT POOL AND TRIGGER REINIT OF INPUTHANDLER AS THIS FINISHED
        }
        
        public V2RInputMapConf() {
            // TODO: CHECK IF GAME IS FIRST TIME STARTING
            ResetKeyMappers();
            ReadKeyMapSettings();
            
            KeysSettings = new() {
                { ChangeCameraFollowingStateV, ChangeCameraFollowingState },
                { MoveForwardV, MoveForward },
                { MoveBackV,    MoveBack },
                { MoveLeftV,    MoveLeft },
                { MoveRightV,   MoveRight },
                { JumpV,        Jump },
                { SprintV,      Sprint },
                { AimingV,      Aiming },
                { QuitGameKBV,  QuitGameKB },
                { QuitGameJSV,  QuitGameJS }
            };

            AxisSettings = new() {
                { MouseXPositiveV, MouseXPositive },
                { MouseXNegativeV, MouseXNegative },
                { MouseYPositiveV, MouseYPositive },
                { MouseYNegativeV, MouseYNegative }
            };

            TwoWayBindingR2V = new Dictionary<string, string>() {
                { MouseXPositive, MouseXPositiveV },
                { MouseXNegative, MouseXNegativeV },
                { MouseYPositive, MouseYPositiveV },
                { MouseYNegative, MouseYNegativeV },
                { ChangeCameraFollowingState, ChangeCameraFollowingStateV },
                { MoveForward, MoveForwardV },
                { MoveBack, MoveBackV },
                { MoveLeft, MoveLeftV },
                { MoveRight, MoveRightV },
                { Jump, JumpV },
                { Sprint, SprintV },
                { Aiming, AimingV },
                { QuitGameKB, QuitGameKBV },
                { QuitGameJS, QuitGameJSV }
            };
        }
    }
}