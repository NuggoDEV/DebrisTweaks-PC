using HarmonyLib;
using UnityEngine;

namespace DebrisTweaks.HarmonyPatches
{
    [HarmonyPatch(typeof(NoteDebris), nameof(NoteDebris.Init))]
    internal class NoteDebrisPatch
    {
        [HarmonyPostfix]
        public static void Postfix(NoteDebris __instance, ref Vector3 noteScale, ref Vector3 force, ref float ____lifeTime, ref MaterialPropertyBlockController ____materialPropertyBlockController, ref int ____colorID)
        {
            Config config = Config.Instance;

            if (!config.ModToggle) return;

            #region Physics

            ____lifeTime = config.DebrisLifetimeToggle ? config.DebrisLifetime : ____lifeTime;

            noteScale = Vector3.one * config.DebrisScale;

            force *= config.VelocityMultiplier;

            Rigidbody rb = __instance.GetComponent<Rigidbody>();
            
            rb.freezeRotation = config.RotationToggle;
            rb.drag = config.DragMultiplier;
            rb.useGravity = config.GravityToggle;

            #endregion

            #region Cosmetic

            Transform transform = __instance.transform;
            Renderer renderer = __instance.gameObject.GetComponentInChildren<Renderer>();

            if (renderer && config.MonochromeToggle)
            {
                ____materialPropertyBlockController.materialPropertyBlock.SetColor(____colorID, Color.gray);
                ____materialPropertyBlockController.ApplyChanges();
            }

            #endregion
        }
    }
}
