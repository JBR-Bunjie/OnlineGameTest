using OnlineGameTest;
using UnityEngine;
using UnityEngine.Events;

namespace UI {
    public class StartUpUIHandler : UIHandler {
        private StartUpUIModel _startUpUIModel;
        private StartUpView _startUpView;

        public StartUpUIHandler() : base() {
            _startUpUIModel = new(this);
            _startUpView = new(this);
        }

        
        #region Scene Setup Functions

        public override void InjectSceneSpecifiedUIFunctionToEventPool() {
            base.InjectSceneSpecifiedUIFunctionToEventPool();
            
            EventPool.Instance.AddEventListener(StartUpLoadingProgress, RefreshProgress);
            EventPool.Instance.AddEventListener(StrippingNextStage, MoveToNextStage);
        }

        public override void RemoveSceneSpecifiedUIFunctionFromEventPool() {
            base.RemoveSceneSpecifiedUIFunctionFromEventPool();
            
            EventPool.Instance.RemoveEventListener(StartUpLoadingProgress, RefreshProgress);
            EventPool.Instance.RemoveEventListener(StrippingNextStage, MoveToNextStage);
        }

        public override void RemoveCurrentSceneUIEntityComponents() {
            base.RemoveCurrentSceneUIEntityComponents();
            
            _startUpView.RemoveUIEntities();
        }

        public override void GraphicsFadeOut() {
            _startUpView.SetGraphicsFadeOutTrigger();
        }

        public override void GraphicsFadeIn() {
            _startUpView.SetGraphicsFadeInTrigger();
        }

        #endregion 


        // Events Callback:
        void RefreshProgress(object value = null) {
            if (value is not null) _startUpUIModel.UpdateProgressValue((float)value * 100);
            _startUpView.UpdateUIElement(_startUpUIModel.GenerateProgressInfo(), _startUpUIModel.ProgressValue);
        }

        void MoveToNextStage(object value) {
            _startUpUIModel.Step2NextStage(value);
            _startUpView.StageTrans();
            
            RefreshProgress(0f);
        }
    }
}