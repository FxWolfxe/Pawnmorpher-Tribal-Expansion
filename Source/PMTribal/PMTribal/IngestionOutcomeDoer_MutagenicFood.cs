// IngestionOutcomeDoer_MutagenicPemmican.cs created by Iron Wolf for PMTribal on 05/22/2020 3:15 PM
// last updated 05/22/2020  3:15 PM

using System.Collections;
using System.Collections.Generic;
using System.Text;
using Pawnmorph;
using RimWorld;
using UnityEngine;
using Verse;

namespace PMTribal
{
    
    public class IngestionOutcomeDoer_MutagenicFood : IngestionOutcomeDoer
    {
        public float addChance = 0.50f;

        public SimpleCurve fullHediffChanceCurve; 

        public IntRange partialCountRange = new IntRange(1, 1); 

        private static readonly Dictionary<MorphDef,int> _scratchDict = new Dictionary<MorphDef,int>(); 

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
        {
            var comp = ingested.TryGetComp<CompIngredients>();
            if (comp?.ingredients == null)
            {
                Log.Warning($"could not get comp ingredient on {ingested.Label}!");
                
                return;
            } 
            _scratchDict.Clear();
            foreach (ThingDef thingDef in comp.ingredients)
            {
                if(!thingDef.IsMeat) continue;
                foreach (MorphDef morphDef in MorphMeatUtilities.GetMorphsOfMeat(thingDef))
                {
                    if(morphDef.fullTransformation == null || morphDef.partialTransformation == null) continue;

                    _scratchDict[morphDef] = _scratchDict.TryGetValue(morphDef) + 1; 
                }
            }

            float c = _scratchDict.Count; 

            foreach (KeyValuePair<MorphDef, int> keyValuePair in _scratchDict)
            {
                MorphDef morphDef = keyValuePair.Key;
                var totalAddChance = 1 - Mathf.Pow(1 - addChance/c, keyValuePair.Value * ingested.stackCount);
                if (Rand.Value < totalAddChance)
                {
                    Hediff partialHediff = pawn.health.hediffSet.GetFirstHediffOfDef(morphDef.partialTransformation);

                    var singleComp = partialHediff?.TryGetComp<HediffComp_Single>();
                    int stacks = singleComp?.stacks ?? 0;

                    var fullMorphChance = fullHediffChanceCurve?.Evaluate(stacks) ?? 0;
                    fullMorphChance = 1 - Mathf.Pow(1 - fullMorphChance, keyValuePair.Value * ingested.stackCount); 
                    HediffDef defToAdd = Rand.Value < fullMorphChance ? 
                        morphDef.fullTransformation : morphDef.partialTransformation;

                    var hediff = HediffMaker.MakeHediff(defToAdd, pawn);
                    var compSingle = hediff.TryGetComp<HediffComp_Single>();
                    if (compSingle != null)
                    {
                        compSingle.stacks = partialCountRange.RandomInRange;
                    }

                    pawn.health.AddHediff(hediff); 
                }
            }

            
        }
    }
}