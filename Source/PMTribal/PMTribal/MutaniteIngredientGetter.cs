// MutaniteIngredientGetter.cs created by Iron Wolf for PMTribal on 05/22/2020 3:02 PM
// last updated 05/22/2020  3:02 PM

using Pawnmorph;
using RimWorld;
using Verse;

namespace PMTribal
{
    public class MutaniteIngredientGetter : IngredientValueGetter
    {
        public override float ValuePerUnitOf(ThingDef t)
        {
            var n = t.GetStatValueAbstract(StatDefOf.Nutrition);

            if (t.StatBaseDefined(PMStatDefOf.MutaniteConcentration))
                return t.GetStatValueAbstract(PMStatDefOf.MutaniteConcentration);
            return n; 
        }

        public const string MUTANITE_RECIPE_REQ = "MuaniteRecipeReqDescription"; 
        
        public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
        {
            var amount = ing.GetBaseCount();

            return MUTANITE_RECIPE_REQ.Translate(amount.ToStringByStyle(ToStringStyle.FloatMaxTwo)) + $"({ing.Summary})";
        }
    }
}