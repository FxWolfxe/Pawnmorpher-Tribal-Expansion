// DbgLogUtilities.cs created by Iron Wolf for PMTribal on 05/23/2020 10:40 AM
// last updated 05/23/2020  10:40 AM

using System.Text;
using HarmonyLib;
using Pawnmorph;
using Verse;

namespace PMTribal.DebugUtilities
{
    static class DbgLogUtilities
    {
        private const string CATEGORY = "Pawnmorpher Tribal";

        [DebugOutput(category = CATEGORY)]
        static void LogMorphMeatInfo()
        {
            StringBuilder builder = new StringBuilder();
            foreach (MorphDef morphDef in MorphDef.AllDefs)
            {
                var str = morphDef.AllAssociatedMeats().Join(m => m.defName); 

                builder.Append($"{morphDef.defName}:[{str}]\n"); 

            }

            Log.Message(builder.ToString()); 
        }
    }
}