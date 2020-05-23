// DbgLogUtilities.cs created by Iron Wolf for PMTribal on 05/23/2020 10:40 AM
// last updated 05/23/2020  10:40 AM

using System.Linq;
using System.Text;
using HarmonyLib;
using Pawnmorph;
using Pawnmorph.Hediffs;
using PMTribal.Aspects;
using PMTribal.Hediffs;
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