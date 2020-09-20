using UnityEngine;
using Verse;

namespace SurvivalTools
{
    public class BaseMod : Mod
    {
        public static BaseMod thisMod;
        // public Settings settings;

        public BaseMod(ModContentPack content) : base(content)
        {
            thisMod = this;
            // settings = GetSettings<Settings>();
        }

        public override string SettingsCategory() => "SurvivalTools".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            GetSettings<Settings>().DoWindowContents(inRect);
        }
        public override void WriteSettings()
        {
            base.WriteSettings();
            Dictionaries.SetDictionaries();
        }
    }
}
