namespace UI {
    public class StartUpUIModel : UIModelBase {
        private int stagePointer = 0;
        private string[] stageDescription = {
            "Version Checking", 
            "Resource Checking",
            "Resource Downloading",
            "Nearly Done..."
        };
        
        public StartUpUIModel(UIHandler uiHandler) : base(uiHandler) { }
        
        public void Step2NextStage(object value = null) {
            if (value is null) stagePointer++;
            else stagePointer = (int)value;
            // ProgressValue = 0.0f; // Reset Loading Bar
        }
        
        public string GenerateProgressInfo() {
            return stageDescription[stagePointer] + "..." + ProgressValue + "%";
        }
    }
}