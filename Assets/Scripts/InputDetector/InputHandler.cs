using UnityEngine;
using System;
using System.Collections.Generic;

namespace InputDetector {
    public class InputHandler {
        private V2RInputMapConf _inputConfig;
        
        
        private Dictionary<string, KeysInputDetector> _keysMapping;
        private Dictionary<string, AxisInputDetector> _axisMapping;

        
        public delegate void OnPressingKeys();
        public delegate void OnReleasingKeys();
        public delegate void OnHoldingKeys();
        public delegate void OnPressingAxis(float velocityMag);
        public delegate void OnReleasingAxis(float velocityMag);
        public delegate void OnHoldingAxis(float velocityMag);
        

        private bool _isInControl;
        private Dictionary<string, OnPressingKeys> _onPressingKeysDic = new();
        private Dictionary<string, OnReleasingKeys> _onReleasingKeysDic = new();
        private Dictionary<string, OnHoldingKeys> _onHoldingKeysDic = new();
        private Dictionary<string, OnPressingAxis> _onPressingAxisDic = new();
        private Dictionary<string, OnReleasingAxis> _onReleasingAxisDic = new();
        private Dictionary<string, OnHoldingAxis> _onHoldingAxisDic = new();
        private Dictionary<string, InputDetector> _entireInput2DetectorMapper = new();

        public InputHandler() {
            _keysMapping = new ();
            _axisMapping = new ();
            
            _inputConfig = new ();
            foreach (var key in _inputConfig.KeysSettings.Keys) 
                _keysMapping[key] = KeysInputDetector.ToKeysInputDetector(_inputConfig.KeysSettings[key]);

            foreach (var key in _inputConfig.AxisSettings.Keys) 
                _axisMapping[key] = AxisInputDetector.ToAxisInputDetector(_inputConfig.AxisSettings[key]);
            
            Reflect();
        }

        private void Reflect() {
            // _inputMap.Clear();
            foreach (var pair in _keysMapping) {
                Remap(pair.Key, pair.Value);
            }
            foreach (var pair in _axisMapping) {
                Remap(pair.Key, pair.Value);
            }
        }

        private void Remap(string keyName, InputDetector detector) {
            // we couldn't just throw an error, but for now, we leave this code here
            if (_entireInput2DetectorMapper.ContainsKey(keyName)) 
                throw new UnityException("Already Contains Input Named [" + keyName + "] !");
            
            _entireInput2DetectorMapper[keyName] = detector;
        }

        public void GetInput() {
            Refresh();
            foreach (var keyName in _onPressingKeysDic.Keys) {
                if (_entireInput2DetectorMapper[keyName].IsPressed) _onPressingKeysDic[keyName].Invoke();
            }
            foreach (var keyName in _onReleasingKeysDic.Keys) {
                if (_entireInput2DetectorMapper[keyName].IsReleased) _onReleasingKeysDic[keyName].Invoke();
            }
            foreach (var keyName in _onHoldingKeysDic.Keys) {
                if (_entireInput2DetectorMapper[keyName].IsHeld) _onHoldingKeysDic[keyName].Invoke();
            }
            foreach (var keyName in _onPressingAxisDic.Keys) {
                if (_entireInput2DetectorMapper[keyName].IsPressed) _onPressingAxisDic[keyName].Invoke(_entireInput2DetectorMapper[keyName].velocityMag);
            }
            foreach (var keyName in _onReleasingAxisDic.Keys) {
                if (_entireInput2DetectorMapper[keyName].IsReleased) _onReleasingAxisDic[keyName].Invoke(_entireInput2DetectorMapper[keyName].velocityMag);
            }
            foreach (var keyName in _onHoldingAxisDic.Keys) {
                // if (_entireInput2DetectorMapper[keyName].IsHeld) 
                _onHoldingAxisDic[keyName].Invoke(_entireInput2DetectorMapper[keyName].velocityMag);
            }
        }

        private void Refresh() {
            foreach (var detector in _entireInput2DetectorMapper.Values) {
                if (detector == null) {
                    break;
                }
                detector.Refresh();
            }
        }

        #region Binding Functions
        
        // All we change here is the delegate itself,
        // we should never touch any code about the reflection between virtual key and original key
        // Keys:
        public bool BindDelegateOnPressKeys(string virtualKey, OnPressingKeys pressDelegate) {
            bool hasBeenBind = _onPressingKeysDic.ContainsKey(virtualKey);
            _onPressingKeysDic[virtualKey] = pressDelegate;
            return hasBeenBind;
        }

        public bool BindDelegateOnReleaseKeys(string virtualKey, OnReleasingKeys releaseDelegate) {
            bool hasBeenBind = _onReleasingKeysDic.ContainsKey(virtualKey);
            _onReleasingKeysDic[virtualKey] = releaseDelegate;
            return hasBeenBind;
        }

        public bool BindDelegateOnHoldKeys(string virtualKey, OnHoldingKeys holdDelegate) {
            bool hasBeenBind = _onHoldingKeysDic.ContainsKey(virtualKey);
            _onHoldingKeysDic[virtualKey] = holdDelegate;
            return hasBeenBind;
        }

        public OnPressingKeys GetBindDelegateOnPressKeys(string virtualKey) {
            if (_onPressingKeysDic.TryGetValue(virtualKey, out var @delegate)) {
                return @delegate;
            }

            return null;
        }
        
        public OnReleasingKeys GetBindDelegateOnReleaseKeys(string virtualKey) {
            if (_onReleasingKeysDic.TryGetValue(virtualKey, out var @delegate)) {
                return @delegate;
            }

            return null;
        }
        
        public OnHoldingKeys GetBindDelegateOnHoldKeys(string virtualKey) {
            if (_onHoldingKeysDic.TryGetValue(virtualKey, out var @delegate)) {
                return @delegate;
            }

            return null;
        }

        public bool UnbindDelegateOnPressKeys(string virtualKey) {
            if (_onPressingKeysDic.ContainsKey(virtualKey)) {
                _onPressingKeysDic.Remove(virtualKey);
                return true;
            }

            return false;
        }
        
        public bool UnbindDelegateOnReleaseKeys(string virtualKey) {
            if (_onReleasingKeysDic.ContainsKey(virtualKey)) {
                _onReleasingKeysDic.Remove(virtualKey);
                return true;
            }

            return false;
        }
        
        public bool UnbindDelegateOnHoldKeys(string virtualKey) {
            if (_onHoldingKeysDic.ContainsKey(virtualKey)) {
                _onHoldingKeysDic.Remove(virtualKey);
                return true;
            }

            return false;
        }

        // Axis:
        public bool BindDelegateOnPressAxis(string virtualKey, OnPressingAxis pressDelegate) {
            bool hasBeenBind = _onPressingAxisDic.ContainsKey(virtualKey);
            _onPressingAxisDic[virtualKey] = pressDelegate;
            return hasBeenBind;
        }

        public bool BindDelegateOnReleaseAxis(string virtualKey, OnReleasingAxis releaseDelegate) {
            bool hasBeenBind = _onReleasingAxisDic.ContainsKey(virtualKey);
            _onReleasingAxisDic[virtualKey] = releaseDelegate;
            return hasBeenBind;
        }

        public bool BindDelegateOnHoldAxis(string virtualAxis, OnHoldingAxis holdDelegate) {
            bool hasBeenBind = _onHoldingAxisDic.ContainsKey(virtualAxis);
            _onHoldingAxisDic[virtualAxis] = holdDelegate;
            return hasBeenBind;
            
        }

        public OnPressingAxis GetBindDelegateOnPressAxis(string virtualKey) {
            if (_onPressingAxisDic.TryGetValue(virtualKey, out var @delegate)) {
                return @delegate;
            }

            return null;
        }
        
        public OnReleasingAxis GetBindDelegateOnReleaseAxis(string virtualKey) {
            if (_onReleasingAxisDic.TryGetValue(virtualKey, out var @delegate)) {
                return @delegate;
            }

            return null;
        }
        
        public OnHoldingAxis GetBindDelegateOnHoldAxis(string virtualKey) {
            if (_onHoldingAxisDic.TryGetValue(virtualKey, out var @delegate)) {
                return @delegate;
            }

            return null;
        }

        public bool UnbindDelegateOnPressAxis(string virtualKey) {
            if (_onPressingAxisDic.ContainsKey(virtualKey)) {
                _onPressingAxisDic.Remove(virtualKey);
                return true;
            }

            return false;
        }
        
        public bool UnbindDelegateOnReleaseAxis(string virtualKey) {
            if (_onReleasingAxisDic.ContainsKey(virtualKey)) {
                _onReleasingAxisDic.Remove(virtualKey);
                return true;
            }

            return false;
        }
        
        public bool UnbindDelegateOnHoldAxis(string virtualKey) {
            if (_onHoldingAxisDic.ContainsKey(virtualKey)) {
                _onHoldingAxisDic.Remove(virtualKey);
                return true;
            }

            return false;
        }

        #endregion

        public InputDetector this[string name] {
            get {
                if (!_entireInput2DetectorMapper.ContainsKey(name)) {
                    throw new UnityException();
                }
        
                return _entireInput2DetectorMapper[name];
            }
            set => Remap(name, value);
        }
    }
}