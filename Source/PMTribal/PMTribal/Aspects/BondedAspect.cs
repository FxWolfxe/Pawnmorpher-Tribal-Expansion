// BondedAspect.cs created by Iron Wolf for PMTribal on 05/23/2020 12:42 PM
// last updated 05/23/2020  12:42 PM

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Pawnmorph;
using Pawnmorph.Hediffs;
using Pawnmorph.Utilities;
using RimWorld;
using Verse;

namespace PMTribal.Aspects
{
    public class BondedAspect : Aspect
    {
        private readonly LinkedList<DirectPawnRelation> _bondRelations = new LinkedList<DirectPawnRelation>();


        /// <summary> Called after this instance is added to the pawn. </summary>
        protected override void PostAdd()
        {
            base.PostAdd();

            var hdiff = Pawn.health.hediffSet.GetFirstHediffOfDef(Defs.Hediffs.TotemAspectHediff);
            if (hdiff == null)
            {
                hdiff = HediffMaker.MakeHediff(Defs.Hediffs.TotemAspectHediff,Pawn);
                Pawn.health.AddHediff(hdiff); 
            }

        }

        /// <summary> Called after this affinity is removed from the pawn. </summary>
        public override void PostRemove()
        {
            base.PostRemove();

            var hdiff = Pawn.health.hediffSet.GetFirstHediffOfDef(Defs.Hediffs.TotemAspectHediff);
            if (hdiff != null)
            {
                Pawn.health.RemoveHediff(hdiff); 
            }
        }

        [CanBeNull]
        public DirectPawnRelation FirstValidRelation
        {
            get
            {
                LinkedListNode<DirectPawnRelation> n = _bondRelations.First;
                while (n != null)
                {
                    LinkedListNode<DirectPawnRelation> nx = n.Next;
                    if (n.Value.otherPawn.Dead)
                    {
                        _bondRelations.Remove(n);
                        n = nx;
                    }
                    else if (!n.Value.otherPawn.Spawned || MorphUtilities.TryGetBestMorphOfAnimal(n.Value.otherPawn.def) == null)
                    {
                        n = nx;
                    }

                    break;
                }

                return n?.Value;
            }
        }

        /// <summary> Called after the base instance is initialize. </summary>
        protected override void PostInit()
        {
            base.PostInit();
            foreach (DirectPawnRelation directPawnRelation in (Pawn.relations?.DirectRelations).MakeSafe())
                if (directPawnRelation.def == PawnRelationDefOf.Bond)
                {
                    LinkedListNode<DirectPawnRelation> n = _bondRelations.First;
                    while (n != null)
                    {
                        LinkedListNode<DirectPawnRelation> nx = n.Next;
                        if (n.Value.startTicks > directPawnRelation.startTicks) break;

                        n = nx;
                    }

                    if (n != null)
                        _bondRelations.AddBefore(n, directPawnRelation);
                    else _bondRelations.AddLast(directPawnRelation);
                }
        }

        internal void Notify_BondedToAnimal(DirectPawnRelation relation)
        {
            _bondRelations.AddLast(relation);

            foreach (var tfHediff in Pawn.health.hediffSet.hediffs.OfType<MorphTf>())
            {
                tfHediff.ResetMutationCaches();
            }

        }
    }
}