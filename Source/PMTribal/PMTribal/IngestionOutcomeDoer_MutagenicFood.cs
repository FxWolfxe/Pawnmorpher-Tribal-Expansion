// IngestionOutcomeDoer_MutagenicPemmican.cs created by Iron Wolf for PMTribal on 05/22/2020 3:15 PM
// last updated 05/22/2020  3:15 PM

using System.Collections;
using System.Collections.Generic;
using Pawnmorph;
using RimWorld;
using UnityEngine;
using Verse;

namespace PMTribal
{
    
    public class IngestionOutcomeDoer_MutagenicFood : IngestionOutcomeDoer
    {
        public float fullHediffChance = 0.05f;
        public float addChance = 0.50f; 
        public bool additive; 

        public IntRange partialCountRange = new IntRange(1, 1); 

        private static readonly Dictionary<MorphDef,int> _scratchDict = new Dictionary<MorphDef,int>(); 

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
        {
            var comp = ingested.TryGetComp<CompIngredients>();
            if (comp?.ingredients == null) return; 
            _scratchDict.Clear();
            foreach (ThingDef thingDef in comp.ingredients)
            {
                if(!thingDef.IsMeat) return;
                foreach (MorphDef morphDef in MorphMeatUtilities.GetMorphsOfMeat(thingDef))
                {
                    if(morphDef.fullTransformation == null || morphDef.partialTransformation == null) continue;

                    _scratchDict[morphDef] = _scratchDict.TryGetValue(morphDef) + 1; 
                }
            }

            foreach (KeyValuePair<MorphDef, int> keyValuePair in _scratchDict)
            {
                var totalAddChance = 1- Mathf.Pow(1 - addChance, keyValuePair.Value);

                if (Rand.Value < totalAddChance)
                {
                    var fullTfChance = 1 - Mathf.Pow(1 - fullHediffChance, keyValuePair.Value);
                    HediffDef defToAdd;
                    if (Rand.Value < fullTfChance)
                    {
                        defToAdd = keyValuePair.Key.fullTransformation;
                    }
                    else
                        defToAdd = keyValuePair.Key.partialTransformation;

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