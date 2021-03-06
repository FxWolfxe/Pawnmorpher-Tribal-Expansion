﻿// RelationshipUtilsPatches.cs created by Iron Wolf for PMTribal on 05/23/2020 12:43 PM
// last updated 05/23/2020  12:43 PM

using System.Linq;
using HarmonyLib;
using Pawnmorph;
using Pawnmorph.Utilities;
using PMTribal.Aspects;
using RimWorld;
using Verse;

namespace PMTribal.Patches
{
    [HarmonyPatch(typeof(RelationsUtility))]
    static class RelationshipUtilsPatches
    {
        [HarmonyPatch(nameof(RelationsUtility.TryDevelopBondRelation))]
        static void SendMessages(bool __result, Pawn humanlike, Pawn animal, float baseChance)
        {
            if (__result)
            {
                var relation = humanlike.relations.DirectRelations.First(r => r.otherPawn == animal);
                bool any = false;
                AspectTracker aspectTracker = humanlike.GetAspectTracker();
                if (aspectTracker == null) return; 
                foreach (BondedAspect bondedAspect in aspectTracker.Aspects.OfType<BondedAspect>())
                {
                    any = true; 
                    bondedAspect.Notify_BondedToAnimal(relation); 
                }

                var mOutlook = humanlike.GetMutationOutlook();
                if (!any && (mOutlook == MutationOutlook.Furry || mOutlook == MutationOutlook.PrimalWish))
                {
                    if (Rand.Value < LoadedModManager.GetMod<PMTribalMod>().Settings.totemAspectAddChance)
                    {
                        aspectTracker.Add(Defs.Aspects.TotemAspect); 
                    } 
                }
            }
        }
    }
}