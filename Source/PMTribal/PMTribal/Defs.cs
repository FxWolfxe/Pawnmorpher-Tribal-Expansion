// Defs.cs created by Iron Wolf for PMTribal on 05/22/2020 2:02 PM
// last updated 05/22/2020  2:02 PM

using System.Security.Policy;
using Pawnmorph;
using RimWorld;
using Verse;

namespace PMTribal
{
    public static class Defs
    {

        [DefOf]
        public static class Things
        {
            static Things()
            {
                DefOfHelper.EnsureInitializedInCtor(typeof(DefOf));
            }
            public static ThingDef MutagenicPemmican; 
        }

        [DefOf]
        public static class Aspects
        {
            static Aspects()
            {
                DefOfHelper.EnsureInitializedInCtor(typeof(AspectDef));
            }

            public static AspectDef TotemAspect; 
        }

        [DefOf]
        public static class Hediffs
        {
            static Hediffs()
            {
                DefOfHelper.EnsureInitializedInCtor(typeof(HediffDef));
            }

            public static HediffDef TotemAspectHediff;
            public static HediffDef MutagenicFoodBuildup; 
        }   
    }
}