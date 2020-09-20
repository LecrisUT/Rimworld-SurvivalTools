using HugsLib;

namespace SurvivalTools
{
    internal class Controller : ModBase
    {
        public Settings settings;
        public override void DefsLoaded()
        {
            settings = BaseMod.thisMod.GetSettings<Settings>();
            Dictionaries.SetDictionaries();
        }
    }
}
