using UnityEngine;

namespace Utility.Constants
{
    /// <summary>
    /// Layer masks available in the project.
    /// </summary>
    public static class Layers
    {
        public static readonly int Default          = LayerMask.NameToLayer("Default");
        public static readonly int TransparentFX    = LayerMask.NameToLayer("TransparentFX");
        public static readonly int IgnoreRaycast    = LayerMask.NameToLayer("Ignore Raycast");
        public static readonly int Platforms        = LayerMask.NameToLayer("Platforms");
        public static readonly int Water            = LayerMask.NameToLayer("Water");
        public static readonly int UI               = LayerMask.NameToLayer("UI");
        public static readonly int Boxes            = LayerMask.NameToLayer("Boxes");
        public static readonly int Springs          = LayerMask.NameToLayer("Springs");
        public static readonly int Ladders          = LayerMask.NameToLayer("Ladders");
        public static readonly int Player           = LayerMask.NameToLayer("Player");
        public static readonly int Allies           = LayerMask.NameToLayer("Allies");
        public static readonly int Enemies          = LayerMask.NameToLayer("Enemies");
        public static readonly int Neutral          = LayerMask.NameToLayer("Neutral");
        public static readonly int ParticleBlockers = LayerMask.NameToLayer("ParticleBlockers");
        public static readonly int Items            = LayerMask.NameToLayer("Items");
        public static readonly int Tokens           = LayerMask.NameToLayer("Tokens");

    }

    public static class LayerMasks
    {
            // Default.
            public static readonly LayerMask Default       = LayerMask.GetMask("Default");
            public static readonly LayerMask TransparentFX = LayerMask.GetMask("TransparentFX");
            public static readonly LayerMask IgnoreRaycast = LayerMask.GetMask("Ignore Raycast");
            public static readonly LayerMask Platforms     = LayerMask.GetMask("Platforms");
            public static readonly LayerMask Water         = LayerMask.GetMask("Water");
            public static readonly LayerMask UI            = LayerMask.GetMask("UI");
            public static readonly LayerMask Boxes         = LayerMask.GetMask("Boxes");
            public static readonly LayerMask Springs       = LayerMask.GetMask("Springs");
            public static readonly LayerMask Ladders       = LayerMask.GetMask("Ladders");
            public static readonly LayerMask Neutral       = LayerMask.GetMask("Neutral");
            public static readonly LayerMask Allies        = LayerMask.GetMask("Allies");
            public static readonly LayerMask Enemies       = LayerMask.GetMask("Enemies");
            public static readonly LayerMask Items         = LayerMask.GetMask("Items");
            public static readonly LayerMask Tokens        = LayerMask.GetMask("Tokens");
            
            // Groups.
            public static readonly LayerMask Teams        = Neutral | Allies | Enemies;
            public static readonly LayerMask Phases       = Neutral | Allies | Enemies;
            public static readonly LayerMask Collectibles = Items   | Tokens;
            public static readonly LayerMask All          = Default | TransparentFX | Platforms | Water | UI | Boxes | Springs | Ladders | Neutral | Allies | Enemies | Items | Tokens;
        }
}
