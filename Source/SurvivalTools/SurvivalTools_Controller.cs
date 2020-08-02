using HugsLib;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using UnityEngine;

namespace SurvivalTools
{
    internal class Controller : ModBase
    {
        public Settings settings;
        public override void DefsLoaded()
        {
            settings = SurvivalTools.thisMod.GetSettings<Settings>();
        }
    }
}
