﻿// Defs.cs created by Iron Wolf for PMTribal on 05/22/2020 2:02 PM
// last updated 05/22/2020  2:02 PM

using RimWorld;
using Verse;

namespace PMTribal
{
    public static class Defs
    {
        [DefOf]
        public static class Hediffs
        {
            static Hediffs()
            {
                DefOfHelper.EnsureInitializedInCtor(typeof(HediffDef));
            }

            public static HediffDef TotemAspectHediff; 
        }   
    }
}