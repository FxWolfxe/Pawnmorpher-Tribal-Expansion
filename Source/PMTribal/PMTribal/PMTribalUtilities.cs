// PMTribalUtilities.cs created by Iron Wolf for PMTribal on 05/31/2020 9:08 AM
// last updated 05/31/2020  9:08 AM

using System;
using JetBrains.Annotations;
using Pawnmorph;
using Verse;

namespace PMTribal
{
    public static class PMTribalUtilities
    {
        public static bool IsFurry([NotNull] this Pawn pawn)
        {
            if (pawn == null) throw new ArgumentNullException(nameof(pawn));
            var outlook = pawn.GetMutationOutlook();
            return outlook == MutationOutlook.Furry || outlook == MutationOutlook.PrimalWish; 
        }
    }
}