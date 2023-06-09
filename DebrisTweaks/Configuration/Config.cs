﻿using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using UnityEngine;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace DebrisTweaks
{
    internal class Config
    {
        public static Config Instance { get; set; }

        // Physics VC
        public bool ModToggle { get; set; } = true;
        public float VelocityMultiplier { get; set; } = 1f;
        public float DragMultiplier { get; set; } = 1f;
        public bool GravityToggle { get; set; } = true;
        public bool RotationToggle { get; set; } = false;

        // Cosmetics VC
        public Color LeftColour { get; set; } = Color.red;
        public Color RightColour { get; set; } = Color.blue;
        public bool CustomColourToggle { get; set; } = false;
        public float DebrisScale { get; set; } = 1f;
        public bool DebrisLifetimeToggle { get; set; } = false;
        public float DebrisLifetime {  get; set; } = 1f;
    }
}
