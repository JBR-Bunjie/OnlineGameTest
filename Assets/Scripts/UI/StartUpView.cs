using OnlineGameTest;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class StartUpView : UIViewBase {
        private Slider _progress;
        private TMPro.TMP_Text _loadingText;
        private Animator _stageTransition;
        
        public StartUpView(UIHandler uiHandler) : base(uiHandler) {
            _loadingText = UIEntityTransform.GetChild(0).GetChild(1).GetComponent<TMPro.TMP_Text>();
            _progress = UIEntityTransform.GetChild(0).GetChild(2).GetComponent<Slider>();
            _stageTransition = UIEntityTransform.GetChild(0).GetChild(3).GetComponent<Animator>();
        }

        public override void RemoveUIEntities() {
            base.RemoveUIEntities();
            
            for (int childCount = 0; childCount < UIEntityTransform.childCount; childCount++) {
                Object.Destroy(UIEntityTransform.GetChild(childCount).gameObject);
            }
        }

        public void UpdateUIElement(string text, float progressValue) {
            _loadingText.text = text;
            _progress.value = progressValue;
        }
        
        public void StageTrans() {
            _stageTransition.SetTrigger("RefreshDesc");
        }
    }
}