using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace SurvivalTools
{
    public class WorkGiver_FellTrees : RimWorld.WorkGiver_PlantsCut
    {
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            foreach (Thing thing in base.PotentialWorkThingsGlobal(pawn))
                if (thing.def.plant.IsTree)
                    yield return thing;
        }
        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (t.def.category != ThingCategory.Plant || !t.def.plant.IsTree)
                return null;
            Job prevJob = base.JobOnThing(pawn, t, forced);
            if (prevJob == null)
                return null;
            if (prevJob.def == RimWorld.JobDefOf.HarvestDesignated)
            {
                prevJob.def = JobDefOf.HarvestTreeDesignated;
                return prevJob;
            }
            if (prevJob.def == RimWorld.JobDefOf.CutPlantDesignated)
            {
                prevJob.def = JobDefOf.FellTreeDesignated;
                return prevJob;
            }
            Log.ErrorOnce("ST_BaseMessage".Translate() + "ST_Error_WGFellTrees".Translate(prevJob.def), 4579456);
            return null;
        }
    }
}