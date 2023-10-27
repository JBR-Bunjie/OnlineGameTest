using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI {
    public class Alert {
        // Entity
        public Transform UITopTransform;
        // protected Stack<GameObject> clones;
        protected GameObject AlertPanelPrefab;
        protected GameObject AlertPanelClone;
        protected TMPro.TMP_Text AlertTextClone;
        protected Button alertConfirm;
        protected Button alertCancel;
        
        public Alert(Transform uiTopTransform) {
            AlertPanelPrefab = Resources.Load<GameObject>(StrResLocs.StartUp_Alert);
            UITopTransform = uiTopTransform;
        }
        
        // To Make sure the buttons are always interactive, we need to put that at the top side.
        public virtual void TriggerAlert(string text, UnityAction confirm = null, UnityAction cancel = null) {
            if (AlertPanelClone is null) {
                AlertPanelClone = Object.Instantiate<GameObject>(AlertPanelPrefab, UITopTransform);
                
                AlertTextClone = AlertPanelClone.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                alertConfirm = AlertPanelClone.transform.GetChild(1).GetComponent<Button>();
                alertCancel = AlertPanelClone.transform.GetChild(2).GetComponent<Button>();
            }
            else {
                AlertPanelClone.SetActive(true);
            }
            
            AlertTextClone.text = text;
            if (confirm is not null) alertConfirm.onClick.AddListener(confirm);
            if (cancel is not null) alertCancel.onClick.AddListener(cancel);
            
            alertConfirm.onClick.AddListener(() => {
                AlertPanelClone.SetActive(false);
                alertConfirm.onClick.RemoveAllListeners();
            });
            alertCancel.onClick.AddListener(() => {
                AlertPanelClone.SetActive(false);
                alertCancel.onClick.RemoveAllListeners();
            });
        }
    }
}