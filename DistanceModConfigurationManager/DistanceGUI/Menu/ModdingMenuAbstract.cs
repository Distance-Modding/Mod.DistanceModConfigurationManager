namespace DistanceModConfigurationManager.DistanceGUI.Menu
{
    public abstract class ModdingMenuAbstract : SuperMenu
    {
        public MenuPanelManager PanelManager => G.Sys.MenuPanelManager_;
        public abstract string Title { get; }
        public override string MenuTitleName_ => Title;
        public override string Name_ => "Mod Settings";
        public override bool DisplayInMenu(bool isPauseMenu) => true;
        protected ModdingMenuAbstract()
        {
            menuBlueprint_ = MenuSystem.MenuBlueprint;
        }
        public override void InitializeVirtual() { }
        public virtual void UpdateVirtual()
        {
            return;
        }
        public virtual void Update()
        {
            return;
        }
        public override void OnPanelPop() { }
    }
}
