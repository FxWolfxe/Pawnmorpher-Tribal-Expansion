// Giver_MutagenBinge.cs created by Iron Wolf for PMTribal on 05/31/2020 9:00 AM
// last updated 05/31/2020  9:00 AM

using System;
using PMTribal.MentalStates;
using RimWorld;
using Verse;
using Verse.AI;

namespace PMTribal.Jobs
{
    public class Giver_MutagenBinge : JobGiver_Binge
    {
        protected override Thing BestIngestTarget(Pawn pawn)
        {
            ChemicalDef chemical = GetChemical(pawn);
            DrugCategory drugCategory = GetDrugCategory(pawn);
            if (chemical == null)
            {
                Log.ErrorOnce("Tried to binge on null chemical.", 1393746152);
                return null;
            }

            Hediff overdose = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose);
            Predicate<Thing> validator = delegate(Thing t)
            {
                if (!IgnoreForbid(pawn) && t.IsForbidden(pawn)) return false;
                if (!pawn.CanReserve(t)) return false;
                var compDrug = t.TryGetComp<CompDrug>();
                if (compDrug.Props.chemical != chemical) return false;
                if (overdose != null
                 && compDrug.Props.CanCauseOverdose
                 && overdose.Severity + compDrug.Props.overdoseSeverityOffset.max >= 0.786f) return false;
                if (!pawn.Position.InHorDistOf(t.Position, 60f)
                 && !t.Position.Roofed(t.Map)
                 && !pawn.Map.areaManager.Home[t.Position]
                 && t.GetSlotGroup() == null) return false;
                return t.def.ingestible.drugCategory.IncludedIn(drugCategory) ? true : false;
            };
            return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Drug),
                                                    PathEndMode.OnCell, TraverseParms.For(pawn), 9999f, validator);
        }

        protected override int IngestInterval(Pawn pawn)
        {
            return 120; 
        }

        private ChemicalDef GetChemical(Pawn pawn)
        {
            return ((MutagenBinge) pawn.MentalState).chemical;
        }

        private DrugCategory GetDrugCategory(Pawn pawn)
        {
            return ((MutagenBinge) pawn.MentalState).drugCategory;
        }
    }
}