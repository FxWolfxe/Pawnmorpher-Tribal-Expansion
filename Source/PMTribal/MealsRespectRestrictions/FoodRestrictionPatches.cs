// FoodRestrictionPatches.cs created by Iron Wolf for MealsRespectRestrictions on 05/22/2020 3:45 PM
// last updated 05/22/2020  3:45 PM

using HarmonyLib;
using RimWorld;
using Verse;

namespace MealsRespectRestrictions
{
    [HarmonyPatch(typeof(FoodRestriction))]
    internal static class FoodRestrictionPatches
    {
        [HarmonyPatch(nameof(FoodRestriction.Allows), typeof(Thing))]
        [HarmonyPostfix]
        private static void AllowsPatch(ref bool __result, FoodRestriction __instance, Thing thing)
        {
            var ext = thing?.def?.GetModExtension<ShouldCountIngredients>();
            if (ext == null) return;
            if (!__result) return; 



            var compIngredient = thing.TryGetComp<CompIngredients>();
            if (compIngredient?.ingredients == null) return;
            foreach (ThingDef ingredient in compIngredient.ingredients)
            {  
                if(ext.careFilter?.Allows(ingredient) == false) continue;
                if (!__instance.Allows(ingredient))
                {
                    __result = false;
                    return;
                }

            }
        }
    }
}