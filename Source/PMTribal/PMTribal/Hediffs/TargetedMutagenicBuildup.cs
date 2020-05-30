// TargetedMutagenicBuildup.cs created by Iron Wolf for PMTribal on 05/30/2020 8:44 AM
// last updated 05/30/2020  8:44 AM

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Pawnmorph;
using Pawnmorph.Hediffs;
using Pawnmorph.TfSys;
using Verse;

namespace PMTribal.Hediffs
{
    public class TargetedMutagenicBuildup : MutagenicBuildup
    {
        private List<MorphDef> _targets = new List<MorphDef>();

        private readonly HashSet<MutationDef> _allowedMutations = new HashSet<MutationDef>();

        /// <summary>
        /// Gets a value indicating whether this transformation hediff blocks the race checking
        /// </summary>
        /// <value>
        ///   <c>true</c> if this transformation hediff blocks the race checking; otherwise, <c>false</c>.
        /// </value>
        public override bool BlocksRaceCheck => false; 
        public IReadOnlyList<MorphDef> Targets => _targets;

        /// <summary>Called when the stage changes.</summary>
        /// <param name="currentStage">The last stage.</param>
        protected override void OnStageChanged(HediffStage currentStage)
        {
            base.OnStageChanged(currentStage);

            if (CurStageIndex == def.stages.Count - 1)
            {
                DoTransformation(); 
            }

        }

        private void DoTransformation()
        {
            var target = Targets.RandomElementWithFallback();
            if (target == null) return;
            var kind = DefDatabase<PawnKindDef>.AllDefs.FirstOrDefault(k => k.race == target.race);
            if (kind == null) return;
            var request = new TransformationRequest(kind, pawn);
            MutagenDefOf.defaultMutagen.MutagenCached.Transform(request); 
        }

        /// <summary>Gets the available the mutations from the given stage.</summary>
        /// <param name="currentStage">The current stage.</param>
        /// <returns></returns>
        protected override IEnumerable<MutationEntry> GetAvailableMutations(HediffStage currentStage)
        {
            return base.GetAvailableMutations(currentStage).Where(m => _allowedMutations.Contains(m.mutation)); 
        }

        public void AddTarget(MorphDef target)
        {
            if (_targets.Contains(target)) return;

            _targets.Add(target);

            foreach (MutationDef targetAllAssociatedMutation in target.AllAssociatedMutations)
            {
                _allowedMutations.Add(targetAllAssociatedMutation); 
            }

            ResetMutationCaches();
        }
        /// <summary>Exposes the data.</summary>
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref _targets, "targets", LookMode.Def);

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                foreach (MutationDef mutationDef in _targets.SelectMany(m => m.AllAssociatedMutations))
                {
                    _allowedMutations.Add(mutationDef); 
                }
            }

        }
    }
}