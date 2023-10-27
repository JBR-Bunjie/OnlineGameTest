namespace UI {
    public class BattleFieldUIHandler : UIHandler {
        private BattleFieldUIModel _battleFieldUIModel;
        private BattleFieldView _battleFieldView;

        public BattleFieldUIHandler(UIHandler uiHandler) : base(uiHandler) {
            _battleFieldUIModel = new(this);
            _battleFieldView = new(this);
        }

        public override void GraphicsFadeOut() {
            _battleFieldView.SetGraphicsFadeOutTrigger();
        }

        public override void GraphicsFadeIn() {
            _battleFieldView.SetGraphicsFadeInTrigger();
        }
    }
}