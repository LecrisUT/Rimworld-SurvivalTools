using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using ToolsFramework;
using Verse;

namespace SurvivalTools
{
    public class Alert_ColonistNeedsSurvivalTool : Alert
    {
        public Alert_ColonistNeedsSurvivalTool()
        {
            defaultPriority = AlertPriority.Medium;
        }

        private List<Pawn> culpritsResult = new List<Pawn>();

        private List<Pawn> ToollessWorkers
        {
            get
            {
                culpritsResult.Clear();
                foreach (Pawn item in PawnsFinder.AllMaps_FreeColonistsSpawned)
                {
                    if (WorkingToolless(item))
                    {
                        culpritsResult.Add(item);
                    }
                }
                return culpritsResult;
            }
        }

        private static bool WorkingToolless(Pawn pawn)
        {
            if (!pawn.CanUseTools(out var tracker))
                return false;
            var bestTools = tracker.UsedHandler.BestTool;
            if (tracker.NecessaryToolTypes.Any(t => Dictionaries.SurvivalToolTypes[t] && bestTools[t] == null))
                return true;
            return false;
        }

        private static string ToollessWorkTypesString(Pawn pawn)
        {
            var types = new List<string>();
            var bestTools = pawn.GetComp<Pawn_ToolTracker>().UsedHandler.BestTool;
            pawn.GetComp<Pawn_ToolTracker>().NecessaryToolTypes.DoIf(t => Dictionaries.SurvivalToolTypes[t] && bestTools[t] == null, t => types.Add(t.LabelCap));
            return GenText.ToCommaList(types);
        }

        public override TaggedString GetExplanation()
        {
            string result = "ColonistNeedsSurvivalToolDesc".Translate() + ":\n";
            foreach (Pawn pawn in ToollessWorkers)
                result += ("\n    " + pawn.LabelShort + " (" + ToollessWorkTypesString(pawn) + ")");
            return result;
        }

        public override string GetLabel()
            => "ColonistNeedsSurvivalTool".Translate();

        private AlertReport cachedReport = false;
        private int nextAlertTick = 0;
        private AlertReport Report
        {
            get
            {
                if (Find.TickManager.TicksGame > nextAlertTick)
                {
                    cachedReport = AlertReport.CulpritsAre(ToollessWorkers);
                    nextAlertTick = Find.TickManager.TicksGame + Settings.alertColonistNeedsSurvivalTool_Delay;
                }
                return cachedReport;
            }
        }
        public override AlertReport GetReport()
            => Settings.alertColonistNeedsSurvivalTool ? Report : false;
    }
}