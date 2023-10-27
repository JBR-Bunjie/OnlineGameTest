using UnityEngine;
using UnityEngine.Events;

namespace UI {
    public class UIHandler {
        // Target
        protected GameObject UICanvas;
        // Entity GameObjects
        public GameObject UISceneTransLayer;
        public Transform UISceneTransTransform;
        public GameObject UIEntityTopLayer;
        public Transform UIEntityTopTransform;
        public GameObject UIEntityLayer;
        public Transform UIEntityTransform; // We get the entity below the Canvas actually
        // Function objects
        private Alert _alert;
        
        protected UIHandler() {
            UICanvas = GameObject.Find("UICanvas");
            MonoSystem.Instance.DoNotDestroySth(UICanvas);
            
            UISceneTransTransform = UICanvas.transform.GetChild(SceneTransLayerIndex);
            UISceneTransLayer = UISceneTransTransform.gameObject;
            UIEntityTopTransform = UICanvas.transform.GetChild(EntityTopLayerIndex);
            UIEntityTopLayer = UIEntityTopTransform.gameObject;
            UIEntityTransform = UICanvas.transform.GetChild(EntityLayerIndex);
            UIEntityLayer = UIEntityTransform.gameObject;

            _alert = new Alert(UIEntityTopTransform);
        }
        
        protected UIHandler(UIHandler uiHandler) {
            UICanvas = uiHandler.UICanvas;
            
            UISceneTransLayer = uiHandler.UISceneTransLayer;
            UISceneTransTransform = uiHandler.UISceneTransTransform;
            UIEntityTopLayer = uiHandler.UIEntityTopLayer;
            UIEntityTopTransform = uiHandler.UIEntityTopTransform;
            UIEntityLayer = uiHandler.UIEntityLayer;
            UIEntityTransform = uiHandler.UIEntityTransform;

            _alert = uiHandler._alert;
        }
        
        public virtual void InjectSceneSpecifiedUIFunctionToEventPool() { }
        
        public virtual void RemoveSceneSpecifiedUIFunctionFromEventPool() { }

        public virtual void RemoveCurrentSceneUIEntityComponents() { }
        
        
        /* Specified Functions For Entire UI System */
        public virtual void GraphicsFadeOut() {}
        public virtual void GraphicsFadeIn() {}

        public virtual void DoAlert(string text, UnityAction confirm = null, UnityAction cancel = null) {
            _alert.TriggerAlert(text, confirm, cancel);
        }
        
        /* CONST VARS*/
        public const string StartUpLoadingProgress = "StartUpLoadingProgress";
        public const string StrippingNextStage = "StrippingNextStage";
        // public const string Alert = "Alert";
        private const int SceneTransLayerIndex = 2;
        private const int EntityTopLayerIndex = 1;
        private const int EntityLayerIndex = 0;
    }
}