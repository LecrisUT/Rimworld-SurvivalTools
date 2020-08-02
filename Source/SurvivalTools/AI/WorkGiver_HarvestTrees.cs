using RimWorld;
using Verse;
using Verse.AI;

namespace SurvivalTools
{
	public class WorkGiver_HarvestTrees : RimWorld.WorkGiver_GrowerHarvest
	{
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Plant plant = c.GetPlant(pawn.Map);
			if (plant == null || !plant.def.plant.IsTree)
				return false;
			return base.HasJobOnCell(pawn, c, forced);
		}
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Job job = base.JobOnCell(pawn, c, forced);
			if (job.def == RimWorld.JobDefOf.Harvest)
			{
				job.def = JobDefOf.HarvestTree;
				return job;
			}
			Log.ErrorOnce("ST_BaseMessage".Translate() + "ST_Error_WGHarvestTrees".Translate(job.def), 547869153);
			return job;
		}
	}
}