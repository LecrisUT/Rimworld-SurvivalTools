using HarmonyLib;
using RimWorld;
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
        private static float noToolWorkFactor = 0.8f;
		private static float noToolWorkOffset = -0.2f;
		private static bool disableNoToolWork = false;

		public static bool alertColonistNeedsSurvivalTool = true;
		public static int alertColonistNeedsSurvivalTool_Delay = 250;
		public static bool DisableNotToolWork => noToolWorkPenalty && disableNoToolWork;
		public static float NoToolWorkFactor => noToolWorkPenalty ? disableNoToolWork ? 0.3f : noToolWorkFactor : 1f;
		public static float NoToolWorkOffset => noToolWorkPenalty ? disableNoToolWork ? -0.5f : noToolWorkOffset : 0f;
		public static bool NoToolWorkPenalty = true;

		private string Filter = "";
		private Vector2 scrollPosition = Vector2.zero;
		public void DoWindowContents(Rect wrect)
		{
			float days;
			float hours;
			// wrect.yMin += 15f;
			// wrect.yMax -= 15f;

			var options = new Listing_Standard();
			// 
			var outRect = new Rect(wrect.x, wrect.y, wrect.width, wrect.height);			// Size of viewed area
			var scrollRect = new Rect(0f, 0f, wrect.width - 16f, wrect.height * 1.5f);		// Size of content data
			var rect2 = new Rect(0f, 0f, wrect.width - 30f, 340f);							// Size of item list data
			Widgets.BeginScrollView(outRect, ref scrollPosition, scrollRect, true);

			options.Begin(scrollRect);

			Header(options, "Performance_Settings".Translate(), false);

			options.CheckboxLabeled("ST_alertColonistNeedsSurvivalTool".Translate(), ref alertColonistNeedsSurvivalTool, "ST_alertColonistNeedsSurvivalTool_tooltip".Translate());
			if (alertColonistNeedsSurvivalTool)
			{
				options.Gap();
				days = Mathf.FloorToInt((float)alertColonistNeedsSurvivalTool_Delay / GenDate.TicksPerDay);
				hours = ((float)alertColonistNeedsSurvivalTool_Delay - days * GenDate.TicksPerDay) / GenDate.TicksPerHour;
				options.Label("ST_alertColonistNeedsSurvivalTool_Delay".Translate() + $"\t{days} " + "DaysLower".Translate() +
					$"  {hours.ToString("F02")} " + "HoursLower".Translate(),
					tooltip: "ST_alertColonistNeedsSurvivalTool_Delay_tooltip".Translate());
				alertColonistNeedsSurvivalTool_Delay = Mathf.RoundToInt(options.Slider(alertColonistNeedsSurvivalTool_Delay, 1, GenDate.TicksPerDay));
			}

			Header(options, "Gameplay_Settings".Translate());

			options.CheckboxLabeled("ST_noToolWorkPenalty".Translate(), ref noToolWorkPenalty, "ST_noToolWorkPenalty_tooltip".Translate());
			if (noToolWorkPenalty)
			{
				options.Gap();
				options.CheckboxLabeled("ST_disableNoToolWork".Translate(), ref disableNoToolWork, "ST_disableNoToolWork_tooltip".Translate());
				if (!disableNoToolWork)
				{
					options.Gap();
					options.Label("ST_noToolWorkFactor".Translate() + $"\tx{noToolWorkFactor.ToString("F02")}", tooltip: "ST_noToolWorkFactor_tooltip".Translate());
					noToolWorkFactor = options.Slider(noToolWorkFactor, 0.3f, 1.0f);
					options.Gap();
					options.Label("ST_noToolWorkOffset".Translate() + $"\tx{noToolWorkOffset.ToString("F02")}", tooltip: "ST_noToolWorkOffset_tooltip".Translate());
					noToolWorkOffset = options.Slider(noToolWorkOffset, -0.5f, 0.0f);
				}
			}
			options.GapLine(12f);
			var toolTypes = DefDatabase<ToolType>.AllDefsListForReading;
			options.Label("ST_ToolTypes".Translate(toolTypes.Count));
			Filter = options.TextEntryLabeled("Filter".Translate(), Filter, 1);
#if V11
			var itemsWindow = options.BeginSection(340f);
#else
			var itemsWindow = options.BeginSection_NewTemp(340f);
#endif
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
					var toolType_Type = Dictionaries.SurvivalToolTypes[toolType] ? "ST_SurvivalTool".Translate() : "ST_NormalTool".Translate();
					Widgets.Label(rect3, toolType.label);
					Rect rect4 = new Rect(rect3.x + itemsWindow.ColumnWidth - 100f, rect3.y, 98f, 24f);
					Widgets.Dropdown(rect4, toolType, t => Dictionaries.SurvivalToolTypes[t], SurvivalToolType_GenerateMenu, toolType_Type);
				}
			}
			options.EndSection(itemsWindow);
			options.End();
			Widgets.EndScrollView();
            // Mod.GetSettings<Settings>().Write();
        }

		private void Header(Listing_Standard options, string text, bool GapLine = true)
        {
			Header();
			if (GapLine)
				options.GapLine();
			else
				options.Gap();
			options.Label(text);
			options.Gap();
			NormalText();
        }
		private void Header()
        {
			GUI.color = Color.cyan;
			Text.Anchor = TextAnchor.UpperCenter;
			Text.Font = GameFont.Medium;
		}
		private void NormalText()
		{
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
		}

		private static IEnumerable<Widgets.DropdownMenuElement<bool>> SurvivalToolType_GenerateMenu(ToolType toolType)
		{
			yield return new Widgets.DropdownMenuElement<bool>
			{
				option = new FloatMenuOption("ST_SurvivalTool".Translate(), () => Dictionaries.SurvivalToolTypes[toolType]=true),
				payload = true,
			};
			yield return new Widgets.DropdownMenuElement<bool>
			{
				option = new FloatMenuOption("ST_NormalTool".Translate(), () => Dictionaries.SurvivalToolTypes[toolType] = false),
				payload = false,
			};
		}
		private List<ToolType> ST_toolTypes_Keys = new List<ToolType>();
		private List<bool> ST_toolTypes_Values = new List<bool>();
		public override void ExposeData()
        {
			Scribe_Values.Look(ref alertColonistNeedsSurvivalTool, "alertColonistNeedsSurvivalTool", true);
			Scribe_Values.Look(ref alertColonistNeedsSurvivalTool_Delay, "alertColonistNeedsSurvivalTool_Delay", 250);

			Scribe_Values.Look(ref noToolWorkPenalty, "noToolWorkPenalty", true);
			Scribe_Values.Look(ref disableNoToolWork, "disableNoToolWork", false);
			Scribe_Values.Look(ref noToolWorkFactor, "noToolWorkFactor", 0.8f);
			Scribe_Values.Look(ref noToolWorkOffset, "noToolWorkOffset", -0.2f);
			Scribe_Collections.Look(ref Dictionaries.SurvivalToolTypes, "ST_toolTypes", LookMode.Def, LookMode.Value, ref ST_toolTypes_Keys, ref ST_toolTypes_Values);
		}
    }
}
