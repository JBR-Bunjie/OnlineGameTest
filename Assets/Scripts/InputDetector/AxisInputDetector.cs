using UnityEngine;
using System;
using System.Collections.Generic;


namespace InputDetector {
    public enum AxisDirection {
        Positive = 1,
        Negative = -1
    }

    public class AxisInputDetector : InputDetector {
        private static readonly Dictionary<string, AxisInputDetector> AxisInputDetectors;
        private float _deadZone;
        private AxisDirection _direction;
        private float _offset;

        
        #region Single Detector

        public static readonly AxisInputDetector AxisMouseXPositive = new AxisInputDetector("Mouse X", AxisDirection.Positive);
        public static readonly AxisInputDetector AxisMouseYPositive = new AxisInputDetector("Mouse Y", AxisDirection.Positive);
        public static readonly AxisInputDetector AxisMouseXNegative = new AxisInputDetector("Mouse X", AxisDirection.Negative);
        public static readonly AxisInputDetector AxisMouseYNegative = new AxisInputDetector("Mouse Y", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis1stPositive = new AxisInputDetector("1st axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis2ndPositive = new AxisInputDetector("2nd axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis3rdPositive = new AxisInputDetector("3rd axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis4thPositive = new AxisInputDetector("4th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis5thPositive = new AxisInputDetector("5th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis6thPositive = new AxisInputDetector("6th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis7thPositive = new AxisInputDetector("7th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis8thPositive = new AxisInputDetector("8th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis9thPositive = new AxisInputDetector("9th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis10thPositive = new AxisInputDetector("10th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis11thPositive = new AxisInputDetector("11th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis12thPositive = new AxisInputDetector("12th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis13thPositive = new AxisInputDetector("13th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis14thPositive = new AxisInputDetector("14th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis15thPositive = new AxisInputDetector("15th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis16thPositive = new AxisInputDetector("16th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis17thPositive = new AxisInputDetector("17th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis18thPositive = new AxisInputDetector("18th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis19thPositive = new AxisInputDetector("19th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis20thPositive = new AxisInputDetector("20th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis21stPositive = new AxisInputDetector("21st axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis22ndPositive = new AxisInputDetector("22nd axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis23rdPositive = new AxisInputDetector("23rd axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis24thPositive = new AxisInputDetector("24th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis25thPositive = new AxisInputDetector("25th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis26thPositive = new AxisInputDetector("26th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis27thPositive = new AxisInputDetector("27th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis28thPositive = new AxisInputDetector("28th axis", AxisDirection.Positive);
        public static readonly AxisInputDetector Axis1stNegative = new AxisInputDetector("1st axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis2ndNegative = new AxisInputDetector("2nd axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis3rdNegative = new AxisInputDetector("3rd axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis4thNegative = new AxisInputDetector("4th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis5thNegative = new AxisInputDetector("5th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis6thNegative = new AxisInputDetector("6th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis7thNegative = new AxisInputDetector("7th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis8thNegative = new AxisInputDetector("8th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis9thNegative = new AxisInputDetector("9th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis10thNegative = new AxisInputDetector("10th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis11thNegative = new AxisInputDetector("11th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis12thNegative = new AxisInputDetector("12th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis13thNegative = new AxisInputDetector("13th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis14thNegative = new AxisInputDetector("14th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis15thNegative = new AxisInputDetector("15th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis16thNegative = new AxisInputDetector("16th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis17thNegative = new AxisInputDetector("17th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis18thNegative = new AxisInputDetector("18th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis19thNegative = new AxisInputDetector("19th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis20thNegative = new AxisInputDetector("20th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis21stNegative = new AxisInputDetector("21st axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis22ndNegative = new AxisInputDetector("22nd axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis23rdNegative = new AxisInputDetector("23rd axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis24thNegative = new AxisInputDetector("24th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis25thNegative = new AxisInputDetector("25th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis26thNegative = new AxisInputDetector("26th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis27thNegative = new AxisInputDetector("27th axis", AxisDirection.Negative);
        public static readonly AxisInputDetector Axis28thNegative = new AxisInputDetector("28th axis", AxisDirection.Negative);
        
        #endregion
        
        #region Detector Dictionary
        
        static AxisInputDetector() {
            AxisInputDetectors = new Dictionary<string, AxisInputDetector>(61);
            AxisInputDetectors["Mouse X Positive"] = AxisMouseXPositive;
            AxisInputDetectors["Mouse Y Positive"] = AxisMouseYPositive;
            AxisInputDetectors["Mouse X Negative"] = AxisMouseXNegative;
            AxisInputDetectors["Mouse Y Negative"] = AxisMouseYNegative;
            AxisInputDetectors["Axis1st Positive"] = Axis1stPositive;
            AxisInputDetectors["Axis2nd Positive"] = Axis2ndPositive;
            AxisInputDetectors["Axis3rd Positive"] = Axis3rdPositive;
            AxisInputDetectors["Axis4th Positive"] = Axis4thPositive;
            AxisInputDetectors["Axis5th Positive"] = Axis5thPositive;
            AxisInputDetectors["Axis6th Positive"] = Axis6thPositive;
            AxisInputDetectors["Axis7th Positive"] = Axis7thPositive;
            AxisInputDetectors["Axis8th Positive"] = Axis8thPositive;
            AxisInputDetectors["Axis9th Positive"] = Axis9thPositive;
            AxisInputDetectors["Axis10th Positive"] = Axis10thPositive;
            AxisInputDetectors["Axis11th Positive"] = Axis11thPositive;
            AxisInputDetectors["Axis12th Positive"] = Axis12thPositive;
            AxisInputDetectors["Axis13th Positive"] = Axis13thPositive;
            AxisInputDetectors["Axis14th Positive"] = Axis14thPositive;
            AxisInputDetectors["Axis15th Positive"] = Axis15thPositive;
            AxisInputDetectors["Axis16th Positive"] = Axis16thPositive;
            AxisInputDetectors["Axis17th Positive"] = Axis17thPositive;
            AxisInputDetectors["Axis18th Positive"] = Axis18thPositive;
            AxisInputDetectors["Axis19th Positive"] = Axis19thPositive;
            AxisInputDetectors["Axis20th Positive"] = Axis20thPositive;
            AxisInputDetectors["Axis21st Positive"] = Axis21stPositive;
            AxisInputDetectors["Axis22nd Positive"] = Axis22ndPositive;
            AxisInputDetectors["Axis23rd Positive"] = Axis23rdPositive;
            AxisInputDetectors["Axis24th Positive"] = Axis24thPositive;
            AxisInputDetectors["Axis25th Positive"] = Axis25thPositive;
            AxisInputDetectors["Axis26th Positive"] = Axis26thPositive;
            AxisInputDetectors["Axis27th Positive"] = Axis27thPositive;
            AxisInputDetectors["Axis28th Positive"] = Axis28thPositive;
            AxisInputDetectors["Axis1st Negative"] = Axis1stNegative;
            AxisInputDetectors["Axis2nd Negative"] = Axis2ndNegative;
            AxisInputDetectors["Axis3rd Negative"] = Axis3rdNegative;
            AxisInputDetectors["Axis4th Negative"] = Axis4thNegative;
            AxisInputDetectors["Axis5th Negative"] = Axis5thNegative;
            AxisInputDetectors["Axis6th Negative"] = Axis6thNegative;
            AxisInputDetectors["Axis7th Negative"] = Axis7thNegative;
            AxisInputDetectors["Axis8th Negative"] = Axis8thNegative;
            AxisInputDetectors["Axis9th Negative"] = Axis9thNegative;
            AxisInputDetectors["Axis10th Negative"] = Axis10thNegative;
            AxisInputDetectors["Axis11th Negative"] = Axis11thNegative;
            AxisInputDetectors["Axis12th Negative"] = Axis12thNegative;
            AxisInputDetectors["Axis13th Negative"] = Axis13thNegative;
            AxisInputDetectors["Axis14th Negative"] = Axis14thNegative;
            AxisInputDetectors["Axis15th Negative"] = Axis15thNegative;
            AxisInputDetectors["Axis16th Negative"] = Axis16thNegative;
            AxisInputDetectors["Axis17th Negative"] = Axis17thNegative;
            AxisInputDetectors["Axis18th Negative"] = Axis18thNegative;
            AxisInputDetectors["Axis19th Negative"] = Axis19thNegative;
            AxisInputDetectors["Axis20th Negative"] = Axis20thNegative;
            AxisInputDetectors["Axis21st Negative"] = Axis21stNegative;
            AxisInputDetectors["Axis22nd Negative"] = Axis22ndNegative;
            AxisInputDetectors["Axis23rd Negative"] = Axis23rdNegative;
            AxisInputDetectors["Axis24th Negative"] = Axis24thNegative;
            AxisInputDetectors["Axis25th Negative"] = Axis25thNegative;
            AxisInputDetectors["Axis26th Negative"] = Axis26thNegative;
            AxisInputDetectors["Axis27th Negative"] = Axis27thNegative;
            AxisInputDetectors["Axis28th Negative"] = Axis28thNegative;
        }
        
        #endregion
        

        public AxisDirection Direction => _direction;

        public float Offset => _offset;

        private AxisInputDetector(string name, AxisDirection direction, float deadZone = 0.15f) {
            _name = name;
            _direction = direction;
            _deadZone = deadZone;
        }

        public override void Refresh() {
            _offset = Input.GetAxis(_name);
            _isPressed = false;
            _isReleased = false;
            velocityMag = _offset * (int)_direction - _deadZone;
            bool flag = velocityMag > 0;
            velocityMag = flag ? velocityMag : 0f; // we push the joystick to another side
            if (_isHeld) {
                if (!flag) {
                    _isReleased = true;
                    _isHeld = false;
                }
            }
            else {
                if (flag) {
                    _isPressed = true;
                    _isHeld = true;
                    _lastPressedTime = Time.time;
                }
            }
            // Debug.Log("Name: " + _name + "; Offset:  " + _offset + "; Velocity: " + velocityMag + "; isHeld: " + _isHeld);
        }

        public static AxisInputDetector ToAxisInputDetector(string name) {
            return AxisInputDetectors.TryGetValue(name, out var value) ? value : null;
        }


        public static bool operator ==(AxisInputDetector lhs, AxisInputDetector rhs) {
            return lhs._name.Equals(rhs._name);
        }

        public static bool operator !=(AxisInputDetector lhs, AxisInputDetector rhs) {
            return !lhs._name.Equals(rhs._name);
            // !lhs._name.Equals(rhs._name, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}