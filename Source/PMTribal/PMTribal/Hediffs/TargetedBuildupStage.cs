// TargetedBuildupStage.cs created by Iron Wolf for PMTribal on 05/30/2020 8:48 AM
// last updated 05/30/2020  8:48 AM

using System.Collections.Generic;
using System.Linq;
using Pawnmorph;
using Pawnmorph.Hediffs;
using Verse;

namespace PMTribal.Hediffs
{
    public class TargetedBuildupStage : TransformationStageBase
    {
        private static readonly Dictionary<MorphDef, List<MutationEntry>> _entryLookup =
            new Dictionary<MorphDef, List<MutationEntry>>();

        public int period = 60;


        /// <summary>Gets the entries for the given pawn</summary>
        /// <param name="pawn">The pawn.</param>
        /// <param name="source"></param>
        /// <returns></returns>
        public override IEnumerable<MutationEntry> GetEntries(Pawn pawn, Hediff source)
        {
            int seed;
            unchecked
            {
                seed = Find.TickManager.TicksGame / period + pawn.HashOffset();
            }

            if (!(source is TargetedMutagenicBuildup tMb)) return Enumerable.Empty<MutationEntry>();

            if (tMb.Targets.Count == 0) return Enumerable.Empty<MutationEntry>();

            Rand.PushState(seed);
            try
            {
                MorphDef morph = tMb.Targets.RandomElement();

                return GetEntriesFor(morph);
            }
            finally
            {
                Rand.PopState();
            }
        }

        private IEnumerable<MutationEntry> GetEntriesFor(MorphDef morph)
        {
            if (_entryLookup.TryGetValue(morph, out List<MutationEntry> lst)) return lst;
            lst = new List<MutationEntry>();
            foreach (MutationDef morphMutation in morph.AllAssociatedMutations)
            {
                var entry = new MutationEntry
                {
                    mutation = morphMutation,
                    addChance = 0.75f,
                    blocks = false
                };
                lst.Add(entry);
            }

            _entryLookup[morph] = lst;
            return lst;
        }
    }
}