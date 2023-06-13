using IPA;
using IPALogger = IPA.Logging.Logger;
using HarmonyLib;
using System.Reflection;
using DebrisTweaks.UI;
using IPA.Config.Stores;

namespace DebrisTweaks
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }
        internal static Harmony harmony;

        [Init]
        public Plugin(IPA.Config.Config conf, IPALogger logger)
        {
            Instance = this;
            Log = logger;
            harmony = new Harmony("NuggoDEV.BeatSaber.DebrisTweaks");
            Config.Instance = conf.Generated<Config>();
        }

        [OnEnable]
        public void OnEnable()
        {
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            BsmlWrapper.EnableUI();
        }

        [OnDisable]
        public void OnDisable()
        {
            harmony.UnpatchSelf();
            BsmlWrapper.DisableUI();
        }
    }
}
