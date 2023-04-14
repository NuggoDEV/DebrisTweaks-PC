using HarmonyLib;
using UnityEngine;
using static BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData;

namespace DebrisTweaks.HarmonyPatches
{
    [HarmonyPatch(typeof(NoteDebris), nameof(NoteDebris.Init))]
    internal class NoteDebrisPatch
    {
        [HarmonyPostfix]
        public static void Postfix(NoteDebris __instance, ref ColorType colorType, ref float ____lifeTime, ref MaterialPropertyBlockController ____materialPropertyBlockController, ref int ____colorID)
        {
            Config config = Config.Instance;
            if (!config.ModToggle) return;

            ____lifeTime = config.DebrisLifetimeToggle ? config.DebrisLifetime : ____lifeTime;

            Rigidbody rb = __instance.GetComponent<Rigidbody>();
            
            rb.freezeRotation = config.RotationToggle;
            rb.drag = config.DragMultiplier;
            rb.useGravity = config.GravityToggle;

            Renderer renderer = __instance.gameObject.GetComponentInChildren<Renderer>();

            if (renderer && config.CustomColourToggle)
            {
                if (colorType == ColorType.ColorA)
                    ____materialPropertyBlockController.materialPropertyBlock.SetColor(____colorID, config.LeftColour);
                else if (colorType == ColorType.ColorB)
                    ____materialPropertyBlockController.materialPropertyBlock.SetColor(____colorID, config.RightColour);

                ____materialPropertyBlockController.ApplyChanges();
            }
        }

        [HarmonyPrefix]
        public static void Prefix(ref Vector3 noteScale, ref Vector3 force)
        {
            Config config = Config.Instance;
            if (!config.ModToggle) return;

            noteScale = Vector3.one * config.DebrisScale;
            force *= config.VelocityMultiplier;
        }
    }
}
