// MutagenBinge_Worker.cs created by Iron Wolf for PMTribal on 05/31/2020 9:08 AM
// last updated 05/31/2020  9:08 AM

using System.Collections.Generic;
using PMTribal.DefExtensions;
using RimWorld;
using Verse;
using Verse.AI;

namespace PMTribal.MentalStates
{
    public class MutagenBinge_BreakWorker : MentalBreakWorker_BingingDrug
    {
        public override bool BreakCanOccur(Pawn pawn)
        {
            return pawn.IsFurry() && base.BreakCanOccur(pawn); 
        }
    }

    public class MutagenBinge_StateWorker : MentalStateWorker_BingingDrug
    {
        bool CanOccurBase(Pawn pawn)
        {
            if (!this.def.unspawnedCanDo && !pawn.Spawned || !this.def.prisonersCanDo && pawn.HostFaction != null || this.def.colonistsOnly && pawn.Faction != Faction.OfPlayer)
                return false;
            for (int index = 0; index < this.def.requiredCapacities.Count; ++index)
            {
                if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[index]))
                    return false;
            }
            return true;
        }
        public bool CanBingeOnNow(Pawn pawn, ChemicalDef chemical, DrugCategory drugCategory)
        {
            List<Thing> thingList = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Drug);
            for (int index = 0; index < thingList.Count; ++index)
            {
                if (!thingList[index].Position.Fogged(thingList[index].Map) && (drugCategory == DrugCategory.Any || thingList[index].def.ingestible.drugCategory == drugCategory) && thingList[index].TryGetComp<CompDrug>().Props.chemical == chemical && (thingList[index].Position.Roofed(thingList[index].Map) || thingList[index].Position.InHorDistOf(pawn.Position, 45f)) && pawn.CanReach((LocalTargetInfo)thingList[index], PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
                    return true;
            }
            return false;
        }
        public override bool StateCanOccur(Pawn pawn)
        {
            if (!CanOccurBase(pawn) || !pawn.Spawned || !pawn.IsFurry())
                return false;
            List<ChemicalDef> defsListForReading = DefDatabase<ChemicalDef>.AllDefsListForReading;
            foreach (ChemicalDef chem in defsListForReading)
            {
                var mutagenChem = chem.GetModExtension<MutagenicDrug>();
                if(mutagenChem == null) continue;
                if(!CanBingeOnNow(pawn, chem, def.drugCategory)) continue;
                return true; 
            }
            return false;
        }

    }
}