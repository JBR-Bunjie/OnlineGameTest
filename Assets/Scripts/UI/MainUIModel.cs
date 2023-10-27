using NetworkProcessor;

namespace UI {
    public class MainUIModel : UIModelBase{
        public string Acc { get; private set; }
        public string Pwd { get; private set; }

        public MainUIModel(UIHandler uiHandler) : base(uiHandler) {
            Acc = NetworkConfig.Account;
            Pwd = NetworkConfig.Password;
        }

        public void UpdateAccountInput(string acc) {
            Acc = acc;
        }
        
        public void UpdatePasswordInput(string pwd) {
            Pwd = pwd;
        }
    }
}