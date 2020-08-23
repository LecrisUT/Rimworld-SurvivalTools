using System.Collections.Generic;
using System.Linq;
using ToolsFramework;
using UnityEngine;
using Verse;

namespace SurvivalTools
{
    public class Settings : ModSettings
    {
		private static bool noToolWorkPenalty = true;
        private static float noToolWorkSpeed = 0.3f;
		private static bool disableNoToolWork = false;
		public static bool DisableNotToolWork => noToolWorkPenalty && disableNoToolWork;
		public static float NoToolWorkSpeed => noToolWorkPenalty ? disableNoToolWork ? 0f : noToolWorkSpeed : 1f;
		public static Dictionary<ToolType, bool> ST_toolTypes = ToolType.allToolTypes.ToDictionary(t => t, t => Controller.defaultSurvivalTools.Contains(t));
		private static string Filter = "";
        public void DoWindowContents(Rect wrect)
        {
            Listing_Standard options = new Listing_Standard();
            Color defaultColor = GUI.color;
            options.Begin(wrect);
			Rect rect2 = new Rect(0f, 0f, wrect.width - 30f, wrect.height * 2f);

			GUI.color = defaultColor;
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;
			options.Gap();
			options.CheckboxLabeled("ST_noToolWorkPenalty".Translate(), ref noToolWorkPenalty, "ST_noToolWorkPenalty_tooltip".Translate());
			if (noToolWorkPenalty)
			{
				options.Gap();
				options.CheckboxLabeled("ST_disableNoToolWork".Translate(), ref disableNoToolWork, "ST_disableNoToolWork_tooltip".Translate());
				if (!disableNoToolWork)
				{
					options.Gap();
					options.Label("ST_noToolWorkSpeed".Translate() + $"\tx{noToolWorkSpeed.ToString("F02")}", tooltip: "ST_noToolWorkSpeed_tooltip".Translate());
					noToolWorkSpeed = options.Slider(noToolWorkSpeed, 0.3f, 0.9f);
				}
			}
			options.GapLine(12f);
			var toolTypes = DefDatabase<ToolType>.AllDefsListForReading;
			options.Label("ST_ToolTypes".Translate(toolTypes.Count));
			Filter = options.TextEntryLabeled("Filter".Translate(), Filter, 1);
			var itemsWindow = options.BeginSection_NewTemp(340f);
			itemsWindow.ColumnWidth = ((rect2.width - 50f) / 3f);
			foreach (var toolType in toolTypes)
			{
				string text = toolType.label.ToLower();
				ThingDef val4 = null;
				if (text.Contains(Filter.ToLower()))
				{
					float lineHeight = Text.LineHeight;
					Rect rect3 = itemsWindow.GetRect(lineHeight);
					TextAnchor anchor = Text.Anchor;
					// Text.Anchor = TextAnchor.MiddleLeft;
					var toolType_Type = ST_toolTypes[toolType] ? "ST_SurvivalTool".Translate() : "ST_NormalTool".Translate();
					Widgets.Label(rect3, toolType.label);
					Rect rect4 = new Rect(rect3.x + itemsWindow.ColumnWidth - 100f, rect3.y, 98f, 24f);
					Widgets.Dropdown(rect4, toolType, t => ST_toolTypes[t], SurvivalToolType_GenerateMenu, toolType_Type);
				}
			}
			options.EndSection(itemsWindow);
			options.End();

            Mod.GetSettings<Settings>().Write();
        }

		private static IEnumerable<Widgets.DropdownMenuElement<bool>> SurvivalToolType_GenerateMenu(ToolType toolType)
		{
			yield return new Widgets.DropdownMenuElement<bool>
			{
				option = new FloatMenuOption("ST_SurvivalTool".Translate(), () => ST_toolTypes[toolType]=true),
				payload = true,
			};
			yield return new Widgets.DropdownMenuElement<bool>
			{
				option = new FloatMenuOption("ST_NormalTool".Translate(), () => ST_toolTypes[toolType] = false),
				payload = false,
			};
		}
		public override void ExposeData()
        {
			var allToolTypes = ToolType.allToolTypes;
			var defaultSurvivalTools = allToolTypes.Select(t => Controller.defaultSurvivalTools.Contains(t)).ToList();
			Scribe_Values.Look(ref noToolWorkPenalty, "noToolWorkPenalty");
			Scribe_Values.Look(ref noToolWorkPenalty, "disableNoToolWork");
			Scribe_Values.Look(ref noToolWorkSpeed, "noToolWorkSpeed", 0.3f);
            Scribe_Collections.Look(ref ST_toolTypes, "ST_toolTypes", LookMode.Def, LookMode.Value, ref allToolTypes, ref defaultSurvivalTools);
		}
    }
}
