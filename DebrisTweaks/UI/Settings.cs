using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.MenuButtons;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using UnityEngine;

namespace DebrisTweaks.UI
{
    internal class DTFlow : FlowCoordinator
    {
        DTMainView mainView = null;
        DTSideView sideView = null;

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            SetTitle("Debris Tweaks");
            showBackButton = true;

            if (mainView == null)
                mainView = BeatSaberUI.CreateViewController<DTMainView>();
            if (sideView == null)
                sideView = BeatSaberUI.CreateViewController<DTSideView>();

            ProvideInitialViewControllers(mainView, null, sideView);
        }
        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Horizontal);
        }

        private void ShowFlow()
        {
            var _parentFlow = BeatSaberUI.MainFlowCoordinator.YoungestChildFlowCoordinatorOrSelf();
            BeatSaberUI.PresentFlowCoordinator(_parentFlow, this);
        }

        static DTFlow flow = null;
        static MenuButton menuButton;

        public static void Initialise()
        {
            MenuButtons.instance.RegisterButton(menuButton ??= new MenuButton("Debris Tweaks", "Adds a few tweaks to debris", () =>
            {
                if (flow == null)
                    flow = BeatSaberUI.CreateFlowCoordinator<DTFlow>();
                flow.ShowFlow();
            }, true));
        }

        public static void Deinit()
        {
            if (menuButton != null)
                MenuButtons.instance.UnregisterButton(menuButton);
        }
    }

    [HotReload(RelativePathToLayout = @"./MainView.bsml")]
    [ViewDefinition("DebrisTweaks.UI.MainView.bsml")]
    internal class DTMainView : BSMLAutomaticViewController
    {
        Config config = Config.Instance;

        [UIComponent("ModToggleID")] private ToggleSetting modToggle;
        [UIComponent("VelocityMultID")] private SliderSetting velocityMult;
        [UIComponent("DragMultID")] private SliderSetting dragMult;
        [UIComponent("GravityToggleID")] private ToggleSetting gravityToggle;
        [UIComponent("RotationToggleID")] private ToggleSetting rotationToggle;

        [UIValue("ModToggle")]
        private bool ModToggle
        {
            get => config.ModToggle;
            set => config.ModToggle = value;
        }

        [UIValue("VelocityMult")]
        private float VelocityMultiplier
        {
            get => config.VelocityMultiplier;
            set => config.VelocityMultiplier = value;
        }

        [UIValue("DragMult")]
        private float DragMultiplier
        {
            get => config.DragMultiplier;
            set => config.DragMultiplier = value;
        }

        [UIValue("GravityToggle")]
        private bool GravityToggle
        {
            get => config.GravityToggle;
            set => config.GravityToggle = value;
        }

        [UIValue("RotationToggle")]
        private bool RotationToggle
        {
            get => config.RotationToggle;
            set => config.RotationToggle = value;
        }

        [UIAction("RunTestLevel")]
        private void TestLevel()
        {
            SimpleLevelStarter[] simpleLevelStarters = Resources.FindObjectsOfTypeAll<SimpleLevelStarter>();
            foreach (SimpleLevelStarter starter in simpleLevelStarters)
            {
                if (starter.gameObject.name.Contains("PerformanceTestLevelButton"))
                {
                    starter.StartLevel();
                    return;
                }
            }
        }

        [UIAction("ResetPhysics")]
        private void ResetPhysics()
        {
            config.ModToggle = true;
            config.VelocityMultiplier = 1f;
            config.DragMultiplier = 1f;
            config.GravityToggle = true;
            config.RotationToggle = false;

            modToggle.Value = config.ModToggle;
            velocityMult.Value = config.VelocityMultiplier;
            dragMult.Value = config.DragMultiplier;
            gravityToggle.Value = config.GravityToggle;
            rotationToggle.Value = config.RotationToggle;

        }
    }

    [HotReload(RelativePathToLayout = @"./SideView.bsml")]
    [ViewDefinition("DebrisTweaks.UI.SideView.bsml")]
    internal class DTSideView : BSMLAutomaticViewController
    {
        Config config = Config.Instance;

        [UIComponent("MonochromeToggleID")] private ToggleSetting monochromeToggle;
        [UIComponent("DebrisScaleID")] private SliderSetting debrisScale;
        [UIComponent("DebrisLifetimeToggleID")] private ToggleSetting debrisLifetimeToggle;
        [UIComponent("DebrisLifetimeID")] private SliderSetting debrisLifetime;

        [UIValue("MonochromeToggle")]
        private bool MonochromeToggle
        {
            get => config.MonochromeToggle;
            set => config.MonochromeToggle = value;
        }

        [UIValue("DebrisScale")]
        private float DebrisScale
        {
            get => config.DebrisScale;
            set => config.DebrisScale = value;
        }

        [UIValue("DebrisLifetimeToggle")]
        private bool LifetimeToggle
        {
            get => config.DebrisLifetimeToggle;
            set => config.DebrisLifetimeToggle = value;
        }

        [UIValue("DebrisLifetime")]
        private float Lifetime
        {
            get => config.DebrisLifetime;
            set => config.DebrisLifetime = value;
        }

        [UIAction("ResetCosmetics")]
        private void ResetCosmetics()
        {
            config.MonochromeToggle = false;
            config.DebrisScale = 1f;
            config.DebrisLifetimeToggle = false;
            config.DebrisLifetime = 1f;

            monochromeToggle.Value = config.MonochromeToggle;
            debrisScale.Value = config.DebrisScale;
            debrisLifetimeToggle.Value = config.DebrisLifetimeToggle;
            debrisLifetime.Value = config.DebrisLifetime;
        }
    }

    public static class BsmlWrapper
    {
        public static void EnableUI()
        {
            DTFlow.Initialise();
        }
        public static void DisableUI()
        {
            DTFlow.Deinit();
        }
    }
}
