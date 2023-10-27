using System.Collections.Generic;
using NetworkProcessor;
using OnlineGameTest;
using UnityEngine;
using UnityEngine.Events;

namespace UI {
    public class MainUIHandler : UIHandler{
        private MainUIModel _mainUIModel;
        private MainView _mainView;
        
        public MainUIHandler(UIHandler uiHandler) : base(uiHandler) {
            _mainUIModel = new(this);
            _mainView = new(this, _mainUIModel.Acc, _mainUIModel.Pwd);
        }

        #region UI Setup Functions

        public override void InjectSceneSpecifiedUIFunctionToEventPool() {
            base.InjectSceneSpecifiedUIFunctionToEventPool();
            
            _mainView.ActiveLoginPanel();
            _mainView.Acc.onValueChanged.AddListener(_mainUIModel.UpdateAccountInput);
            _mainView.Pwd.onValueChanged.AddListener(_mainUIModel.UpdatePasswordInput);
            _mainView.LoginConfirmButton.onClick.AddListener(Login);
            _mainView.BackMain();
        }

        public override void RemoveSceneSpecifiedUIFunctionFromEventPool() {
            base.RemoveSceneSpecifiedUIFunctionFromEventPool();
            
            _mainView.ActiveLoginPanel();
            _mainView.Acc.onValueChanged.RemoveAllListeners();
            _mainView.Pwd.onValueChanged.RemoveAllListeners();
            _mainView.LoginConfirmButton.onClick.RemoveAllListeners();
            _mainView.BackMain();
        }

        public override void RemoveCurrentSceneUIEntityComponents() {
            base.RemoveCurrentSceneUIEntityComponents();
            
            _mainView.RemoveUIEntities();
        }
        
        private void Login() {
            // Save New Login Info
            NetworkConfig.Account = _mainUIModel.Acc;
            NetworkConfig.Password = _mainUIModel.Pwd;
            // Trigger Event
            EventPool.Instance.TriggerEvent("Login", null);
        }


        public override void GraphicsFadeOut() {
            _mainView.SetGraphicsFadeOutTrigger();
        }

        public override void GraphicsFadeIn() {
            _mainView.SetGraphicsFadeInTrigger();
        }
        
        #endregion
    }
}