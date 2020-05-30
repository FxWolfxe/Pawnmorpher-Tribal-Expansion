// DbgLogUtilities.cs created by Iron Wolf for PMTribal on 05/23/2020 10:40 AM
// last updated 05/23/2020  10:40 AM

using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using Pawnmorph;
using Pawnmorph.Hediffs;
using PMTribal.Aspects;
using PMTribal.Hediffs;
using RimWorld;
using Verse;

namespace PMTribal.DebugUtilities
{
    static class DbgLogUtilities
    {
        private const string CATEGORY = "Pawnmorpher Tribal";

        [DebugOutput(category = CATEGORY)]
        static void LogMorphMeatInfo()
        {
            StringBuilder builder = new StringBuilder();
            foreach (MorphDef morphDef in MorphDef.AllDefs)
            {
                var str = morphDef.AllAssociatedMeats().Join(m => m.defName); 

                builder.Append($"{morphDef.defName}:[{str}]\n"); 

            }

            Log.Message(builder.ToString()); 
        }

        [DebugAction(category = CATEGORY, actionType = DebugActionType.ToolMap)]
        static void SpawnMutagenicPemmican()
        {
            var pos = UI.MouseCell();
            if(!pos.IsValid) return;

            var options = MorphDef.AllDefs.Select(m => GetMutagenicPemmicanOption(m, pos)).ToList();
            Find.WindowStack.Add(new Dialog_DebugOptionListLister(options)); 

        }

        static DebugMenuOption GetMutagenicPemmicanOption(MorphDef mDef, IntVec3 iPos)
        {
            return new DebugMenuOption(mDef.Label, DebugMenuOptionMode.Action, () => CreateMutagenicPemmican(mDef,iPos));
        }

        static void CreateMutagenicPemmican(MorphDef mDef, IntVec3 pos)
        {
            var mp = Find.CurrentMap;
            var thing = ThingMaker.MakeThing(Defs.Things.MutagenicPemmican);
            var ingred = thing.TryGetComp<CompIngredients>();
            if (ingred == null) return; 
            ingred.ingredients = ingred.ingredients ?? new List<ThingDef>();
            ingred.ingredients.Add(mDef.race.race.meatDef);
            thing.stackCount = 75;
            GenSpawn.Spawn(thing, pos, mp);
        }




        [DebugOutput(category = CATEGORY, onlyWhenPlaying = true)]
        static void ListTotemAspectInfo()
        {
            string Selector(MutationEntry entry)
            {
                return
                    $"{entry.mutation.defName}:{entry.addChance.ToStringByStyle(ToStringStyle.PercentOne)} blocks:{entry.blocks}";
            }

            StringBuilder builder = new StringBuilder(); 
            foreach (Pawn pawn in Find.CurrentMap.mapPawns.AllPawns)
            {
                var aT = pawn.GetAspectTracker(); 
                if(aT == null) continue;
                var totemAspect = aT.Aspects.OfType<BondedAspect>().FirstOrDefault();
                if(totemAspect == null) continue;
                var hDiff = pawn.health.hediffSet.GetFirstHediffOfDef(Defs.Hediffs.TotemAspectHediff); 
                if(hDiff == null) continue;
                if (hDiff.CurStage is TotemStage tStage)
                {
                    builder.AppendLine($"{pawn.Name}:[{tStage.GetEntries(pawn, hDiff).Select(Selector).Join()}]");
                }
            }

            Log.Message(builder.ToString()); 
        }
    }
}