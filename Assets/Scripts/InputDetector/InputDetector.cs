using UnityEngine;
using System;

namespace InputDetector {
    public abstract class InputDetector {
        protected string _name;
        protected bool _isPressed;
        protected bool _isReleased;
        protected bool _isHeld;
        protected float _lastPressedTime;
        public float velocityMag = 1f;

        public string Name {
            get => _name;
        }

        public bool IsPressed {
            get => _isPressed;
        }

        public bool IsReleased {
            get => _isReleased;
        }

        public bool IsHeld {
            get => _isHeld;
        }

        public float ChargeTime {
            get => Time.time - _lastPressedTime;
        }

        public abstract void Refresh();
    }
}