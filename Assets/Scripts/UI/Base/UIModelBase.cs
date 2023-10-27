namespace UI {
    public class UIModelBase {
        public float ProgressValue { get; protected set; } = 0;

        protected UIHandler UIHandler;
        
        protected UIModelBase(UIHandler uiHandler) {
            UIHandler = uiHandler;
        }
        
        public void UpdateProgressValue(float value) {
            ProgressValue = value;
        }
    }
}