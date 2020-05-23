// TotemStage.cs created by Iron Wolf for PMTribal on 05/23/2020 12:55 PM
// last updated 05/23/2020  12:55 PM

using System.Collections.Generic;
using System.Linq;
using Pawnmorph;
using Pawnmorph.Hediffs;
using PMTribal.Aspects;
using Verse;

namespace PMTribal.Hediffs
{
    public class TotemStage : TransformationStageBase
    {
        private static Dictionary<MorphDef, List<MutationEntry>> _entriesCache = new Dictionary<MorphDef, List<MutationEntry>>();

        MutationEntry CreateEntry(MutationDef mDef)
        {
            return new MutationEntry
            {
                addChance = 0.3f,
                blocks = false,
                mutation = mDef
            };
        }



        /// <summary>Gets the entries for the given pawn</summary>
        /// <param name="pawn">The pawn.</param>
        /// <param name="source"></param>
        /// <returns></returns>
        public override IEnumerable<MutationEntry> GetEntries(Pawn pawn, Hediff source)
        {
            var totemAspect = pawn.GetAspectTracker()?.Aspects?.OfType<BondedAspect>()?.FirstOrDefault();
            if (totemAspect == null) return Enumerable.Empty<MutationEntry>();

            var fRelation = totemAspect.FirstValidRelation; 
            if(fRelation == null) return Enumerable.Empty<MutationEntry>();

            var mDef = MorphUtilities.TryGetBestMorphOfAnimal(fRelation.otherPawn.def); 
            if(mDef == null) return Enumerable.Empty<MutationEntry>();

            if (!_entriesCache.TryGetValue(mDef, out var lst))
            {
                lst = mDef.AllAssociatedMutations.Select(CreateEntry).ToList();
                _entriesCache[mDef] = lst; 
            }

            return lst; 
        }
    }
}