// MorphMeatUtilities.cs created by Iron Wolf for PMTribal on 05/22/2020 4:10 PM
// last updated 05/22/2020  4:10 PM

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Pawnmorph;
using Pawnmorph.Utilities;
using Verse;

namespace PMTribal
{
    [StaticConstructorOnStartup]
    public static class MorphMeatUtilities
    {
        private static readonly Dictionary<ThingDef, List<MorphDef>> _meatLookupDict = new Dictionary<ThingDef, List<MorphDef>>();

        static MorphMeatUtilities()
        {
            foreach (MorphDef morph in MorphDef.AllDefs)
            {
                ThingDef raceMeat = morph.race.race?.meatDef;
                if (raceMeat == null) continue;

                if (!_meatLookupDict.TryGetValue(raceMeat, out List<MorphDef> lst))
                {
                    lst = new List<MorphDef>();
                    _meatLookupDict[raceMeat] = lst;
                }

                lst.Add(morph);

                foreach (ThingDef race in morph.associatedAnimals.MakeSafe())
                {
                    var rMeat = race.race?.meatDef; 
                    if(rMeat == null) continue;
                    if (!_meatLookupDict.TryGetValue(rMeat, out  lst))
                    {
                        lst = new List<MorphDef>();
                        _meatLookupDict[rMeat] = lst;
                    }

                    lst.Add(morph); 

                }

            }
        }

        public static IEnumerable<ThingDef> AllAssociatedMeats([NotNull] this MorphDef morph)
        {
            foreach (KeyValuePair<ThingDef, List<MorphDef>> kvp in _meatLookupDict)
            {
                if (kvp.Value.Contains(morph)) yield return kvp.Key;
            }
        }

        public static IReadOnlyList<MorphDef> GetMorphsOfMeat([NotNull] ThingDef meat)
        {
            if (meat == null) throw new ArgumentNullException(nameof(meat));

            return (IReadOnlyList<MorphDef>) _meatLookupDict.TryGetValue(meat) ?? Array.Empty<MorphDef>();
        }
    }
}