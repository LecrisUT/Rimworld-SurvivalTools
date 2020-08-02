using UnityEngine;
using Verse;

namespace SurvivalTools
{
    public class SurvivalTools : Mod
    {
        public static SurvivalTools thisMod;
        // public Settings settings;

        public SurvivalTools(ModContentPack content) : base(content)
        {
            thisMod = this;
            // settings = GetSettings<Settings>();
        }

        public override string SettingsCategory() => "SurvivalTools".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            GetSettings<Settings>().DoWindowContents(inRect);
        }
    }
}
