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
            if (thing?.def?.GetModExtension<ShouldCountIngredients>() == null) return;

            var compIngredient = thing.TryGetComp<CompIngredients>();
            if (compIngredient?.ingredients == null) return;
            foreach (ThingDef ingredient in compIngredient.ingredients)
                if (!__instance.Allows(ingredient))
                {
                    __result = false;
                    return;
                }
        }
    }
}