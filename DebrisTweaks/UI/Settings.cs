using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
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
            MenuButtons.instance.RegisterButton(menuButton ??= new MenuButton("Debris Tweaks", "Adds tweaks to the debris!", () =>
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
    }

    [HotReload(RelativePathToLayout = @"./SideView.bsml")]
    [ViewDefinition("DebrisTweaks.UI.SideView.bsml")]
    internal class DTSideView : BSMLAutomaticViewController
    {
        Config config = Config.Instance;

        [UIValue("CustomColourToggle")]
        private bool CustomColourToggle
        {
            get => config.CustomColourToggle;
            set => config.CustomColourToggle = value;
        }

        [UIValue("LeftColour")]
        private Color LeftColour
        {
            get => config.LeftColour;
            set => config.LeftColour = value;
        }

        [UIValue("RightColour")]
        private Color RightColour
        {
            get => config.RightColour;
            set => config.RightColour = value;
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
