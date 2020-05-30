// IngestionOutcomeDoer_MutagenicPemmican.cs created by Iron Wolf for PMTribal on 05/22/2020 3:15 PM
// last updated 05/22/2020  3:15 PM

using System.Collections;
using System.Collections.Generic;
using System.Text;
using Pawnmorph;
using PMTribal.Hediffs;
using RimWorld;
using UnityEngine;
using Verse;

namespace PMTribal
{
    
    public class IngestionOutcomeDoer_MutagenicFood : IngestionOutcomeDoer
    {
        public float addChance = 0.50f;
        public float severityPerServing = 0.007f; 
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
            float severityToAdd = ingested.stackCount * severityPerServing * Rand.Range(0.7f, 1.2f);
            severityToAdd = pawn.GetMutagenicBuildupMultiplier() * severityToAdd;
            var hediff = (TargetedMutagenicBuildup) pawn.health.hediffSet.GetFirstHediffOfDef(Defs.Hediffs.MutagenicFoodBuildup);
            if (hediff == null)
            {
                hediff = (TargetedMutagenicBuildup) HediffMaker.MakeHediff(Defs.Hediffs.MutagenicFoodBuildup, pawn);
                hediff.Severity = 0.01f;
                pawn.health.hediffSet.AddDirect(hediff); 
            }

            hediff.Severity += severityToAdd;
            
            foreach (KeyValuePair<MorphDef, int> keyValuePair in _scratchDict)
            {
                MorphDef morphDef = keyValuePair.Key;
                hediff.AddTarget(morphDef); 
            }

        }
    }
}