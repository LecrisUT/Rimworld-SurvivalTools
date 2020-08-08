using System.Collections.Generic;
using ToolsFramework;
using Verse;

namespace SurvivalTools
{
    public class PawnCapacityWorker_SurvivalTool : PawnCapacityWorker
    {
        public override bool CanHaveCapacity(BodyDef body)
        {
            return true;
        }
        public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
        {
            var pawn = diffSet.pawn;
            if (!pawn.CanUseTools(out var tracker))
                return 0.0f;
            return 1.0f;
        }
    }
}
