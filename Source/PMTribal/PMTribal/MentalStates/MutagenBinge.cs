// MutagenBinge.cs created by Iron Wolf for PMTribal on 05/31/2020 8:57 AM
// last updated 05/31/2020  8:57 AM

using System.Collections.Generic;
using System.Linq;
using Pawnmorph.Hediffs;
using PMTribal.DefExtensions;
using RimWorld;
using Verse;
using Verse.AI;

namespace PMTribal.MentalStates
{
    public class MutagenBinge : MentalState_Binging
    {
        private static readonly List<ChemicalDef> addictions = new List<ChemicalDef>();
        public ChemicalDef chemical;

        public DrugCategory drugCategory;

        private const int CHECK_PERIOD = 60;


        public override void MentalStateTick()
        {
            base.MentalStateTick();

            if (pawn.IsHashIntervalTick(CHECK_PERIOD))
            {
                if(pawn.health.hediffSet.hediffs.OfType<MorphTf>().Any())
                    RecoverFromState();
            }
        }

        public override string InspectLine => string.Format(base.InspectLine, chemical.label);

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref chemical, "chemical");
            Scribe_Values.Look(ref drugCategory, "drugCategory");
        }

        public override void PostStart(string reason)
        {
            base.PostStart(reason);
            ChooseRandomChemical();
            if (PawnUtility.ShouldSendNotificationAbout(pawn))
            {
                string str = "LetterLabelDrugBinge".Translate(chemical.label).CapitalizeFirst() + ": " + pawn.LabelShortCap;
                string text = "LetterDrugBinge".Translate(pawn.Label, chemical.label, pawn).CapitalizeFirst();
                if (!reason.NullOrEmpty()) text = text + "\n\n" + reason;
                Find.LetterStack.ReceiveLetter(str, text, LetterDefOf.ThreatSmall, pawn);
            }
        }

        public override void PostEnd()
        {
            base.PostEnd();
            if (PawnUtility.ShouldSendNotificationAbout(pawn))
                Messages.Message("MessageNoLongerBingingOnDrug".Translate(pawn.LabelShort, chemical.label, pawn), pawn,
                                 MessageTypeDefOf.SituationResolved);
        }


        private void ChooseRandomChemical()
        {
            addictions.Clear();
            List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
            for (var i = 0; i < hediffs.Count; i++)
            {
                var hediff_Addiction = hediffs[i] as Hediff_Addiction;
                if (hediff_Addiction != null && AddictionUtility.CanBingeOnNow(pawn, hediff_Addiction.Chemical, DrugCategory.Any))
                    addictions.Add(hediff_Addiction.Chemical);
            }

            if (addictions.Count > 0)
            {
                chemical = addictions.RandomElement();
                drugCategory = DrugCategory.Any;
                addictions.Clear();
                return;
            }

            chemical = DefDatabase<ChemicalDef>.AllDefsListForReading.Where(IsValidChemical).RandomElementWithFallback();
            if (chemical != null)
            {
                drugCategory = def.drugCategory;
                return;
            }

            chemical = DefDatabase<ChemicalDef>.AllDefsListForReading.Where(IsValidChemical).RandomElementWithFallback();
            if (chemical != null)
            {
                drugCategory = DrugCategory.Any;
                return;
            }

            chemical = DefDatabase<ChemicalDef>.AllDefsListForReading.RandomElement();
            drugCategory = DrugCategory.Any;
        }

        private bool IsValidChemical(ChemicalDef cDef)
        {
            return cDef.GetModExtension<MutagenicDrug>() != null;
        }
    }
}