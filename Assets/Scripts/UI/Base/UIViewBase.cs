using UnityEngine;
using OnlineGameTest;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI {
    public class UIViewBase {
        // Entity & Target
        public UIHandler UIHandler;
        public GameObject UIEntityTopLayer;
        public Transform UIEntityTopTransform;
        public GameObject UISceneTransLayer;
        public Transform UISceneTransTransform;
        public GameObject UIEntityLayer;
        public Transform UIEntityTransform; // We get the entity below the Canvas actually
        public Animator sceneTransition;
        // Functions Objects:
        
        protected UIViewBase(UIHandler uiHandler) {
            UIHandler = uiHandler;
            UISceneTransLayer = uiHandler.UISceneTransLayer;
            UISceneTransTransform = uiHandler.UISceneTransTransform;
            UIEntityTopLayer = uiHandler.UIEntityTopLayer;
            UIEntityTopTransform = uiHandler.UIEntityTopTransform;
            UIEntityLayer = UIHandler.UIEntityLayer;
            UIEntityTransform = UIHandler.UIEntityTransform;

            sceneTransition = UISceneTransLayer.GetComponent<Animator>();
        }

        public virtual void ActiveGameObject(GameObject go) {
            go.SetActive(true);
        }

        public virtual void RemoveUIEntities() { }

        public void SetGraphicsFadeOutTrigger() {
            UISceneTransLayer.SetActive(true);
            sceneTransition.SetTrigger("FadeOut");  
        }

        public void SetGraphicsFadeInTrigger() {
            UISceneTransLayer.SetActive(true);
            sceneTransition.SetTrigger("FadeIn");
        }
    }
}