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
        public float fullHediffChance = 0.05f;
        public float addChance = 0.50f; 

        public IntRange partialCountRange = new IntRange(1, 1); 

        private static readonly Dictionary<MorphDef,int> _scratchDict = new Dictionary<MorphDef,int>(); 

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
        {
            Log.Message($"{pawn.Label} ingested {ingested.Label}");
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

            StringBuilder builder = new StringBuilder(); 

            foreach (KeyValuePair<MorphDef, int> keyValuePair in _scratchDict)
            {
                builder.AppendLine($"{keyValuePair.Key.defName}:");
                var totalAddChance = 1 - Mathf.Pow(1 - addChance, keyValuePair.Value * ingested.stackCount);
                builder.AppendLine($"{nameof(totalAddChance)}:{totalAddChance.ToStringByStyle(ToStringStyle.PercentOne)}"
                                      .Indented("|\t"));
                if (Rand.Value < totalAddChance)
                {
                    var fullTfChance = 1 - Mathf.Pow(1 - fullHediffChance, keyValuePair.Value * ingested.stackCount);
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

            if (builder.Length == 0) Log.Message($"cannot find morph hediff to add!");
            else Log.Message(builder.ToString()); 

        }
    }
}