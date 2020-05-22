// ModInit.cs created by Iron Wolf for PMTribal on 05/22/2020 1:53 PM
// last updated 05/22/2020  1:53 PM

using System;
using HarmonyLib;
using Verse;

namespace PMTribal
{
    [StaticConstructorOnStartup]
    public static class PMTribalInit
    {
        public static string HARMONY_ID = "ironwolf.pawnmorpher.tribal";

        static PMTribalInit()
        {
            var har = new Harmony(HARMONY_ID);

            try
            {
                har.PatchAll();
            }
            catch (Exception e)
            {
                Log.Error($"caught {e.GetType().Name} while patching {HARMONY_ID}!\n{e}");
            }
        }
    }


    public class PMTribalMod : Mod
    {
        public PMTribalMod(ModContentPack content) : base(content)
        {
        }
    }
}