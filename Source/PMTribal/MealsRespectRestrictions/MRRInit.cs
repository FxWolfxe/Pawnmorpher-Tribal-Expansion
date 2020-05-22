// MRRInit.cs created by Iron Wolf for MealsRespectRestrictions on 05/22/2020 3:27 PM
// last updated 05/22/2020  3:27 PM

using System;
using HarmonyLib;
using Verse;

namespace MealsRespectRestrictions
{
    [StaticConstructorOnStartup]
    public static class MRRInit
    {
        public const string HARMONY_ID = "ironwolf.meals_respect_restrictions";

        static MRRInit()
        {
            var har = new Harmony(HARMONY_ID); 
            try
            {
                har.PatchAll(); 
            }
            catch (Exception e)
            {
                Log.Error($"caught {e.GetType().Name} while patching {HARMONY_ID}\n{e}");
            }
        }
    }
}