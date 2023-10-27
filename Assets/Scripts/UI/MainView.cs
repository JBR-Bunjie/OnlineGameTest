using OnlineGameTest;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class MainView : UIViewBase {
        private GameObject Title;
        private Transform TitleTransform;
        private GameObject Main;
        private Transform MainTransform;
        private GameObject Login;
        private Transform LoginTransform;

        // Main Page
        public Button MainLoginButton;
        public Button MainQuickStartButton;
        public Button MainConfigButton;
        public Button MainQuitButton;

        // Login Page
        public TMP_InputField Acc;
        public TMP_InputField Pwd;
        public Button LoginConfirmButton;
        public Button RegisterConfirmButton;
        public Button BackToMainButton;


        public MainView(UIHandler uiHandler, string accText, string pwdText) : base(uiHandler) {
            Title = Resources.Load<GameObject>(StrResLocs.Main_Title);
            Main = Resources.Load<GameObject>(StrResLocs.Main_Main);
            Login = Resources.Load<GameObject>(StrResLocs.Main_Login);

            Title = Object.Instantiate(Title, UIEntityTransform);
            Main = Object.Instantiate(Main, UIEntityTopTransform);
            Login = Object.Instantiate(Login, UIEntityTopTransform);
            MainTransform = Main.transform;
            LoginTransform = Login.transform;

            MainLoginButton = MainTransform.GetChild(0).GetComponent<Button>();
            MainQuickStartButton = MainTransform.GetChild(1).GetComponent<Button>();
            MainConfigButton = MainTransform.GetChild(2).GetComponent<Button>();
            MainQuitButton = MainTransform.GetChild(3).GetComponent<Button>();

            Acc = LoginTransform.GetChild(0).GetChild(1).GetComponent<TMP_InputField>();
            Pwd = LoginTransform.GetChild(1).GetChild(1).GetComponent<TMP_InputField>();
            Acc.text = accText;
            Pwd.text = pwdText;
            RegisterConfirmButton = LoginTransform.GetChild(2).GetComponent<Button>();
            LoginConfirmButton = LoginTransform.GetChild(3).GetComponent<Button>();
            BackToMainButton = LoginTransform.GetChild(4).GetComponent<Button>();

            // Basic Views Change 
            BindBasicViewChangeCallback();
            
            // Finally Set Disactive
            Login.SetActive(false);
        }

        private void BindBasicViewChangeCallback() {
            MainLoginButton.onClick.AddListener(ActiveLoginPanel);
            BackToMainButton.onClick.AddListener(BackMain);
            MainQuitButton.onClick.AddListener(() => { EventPool.Instance.TriggerEvent(SystemBehaviour.QuitGameEvent, null); });
        }

        private void UnbindBasicViewChangeCallback() {
            MainLoginButton.onClick.RemoveAllListeners();
            BackToMainButton.onClick.RemoveAllListeners();
            MainQuitButton.onClick.RemoveAllListeners();
        }

        public void ActiveLoginPanel() {
            Login.SetActive(true);
            Main.SetActive(false);
        }

        public void BackMain() {
            Login.SetActive(false);
            Main.SetActive(true);
        }

        public override void RemoveUIEntities() {
            base.RemoveUIEntities();
            UnbindBasicViewChangeCallback();
            
            Object.Destroy(Title);
            Main.SetActive(true);
            Object.Destroy(Main);
            Login.SetActive(true);
            Object.Destroy(Login);
        }
    }
}