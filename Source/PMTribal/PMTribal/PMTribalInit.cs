// ModInit.cs created by Iron Wolf for PMTribal on 05/22/2020 1:53 PM
// last updated 05/22/2020  1:53 PM

using System;
using HarmonyLib;
using UnityEngine;
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

        static void DoPatches(Harmony har)
        {
            //bill patches 




        }



    }


    public class PMTribalMod : Mod
    {
        public PMTribalSettings Settings { get; }

        public PMTribalMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<PMTribalSettings>(); 
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var ls = new Listing_Standard(); 
            ls.Begin(inRect);
            ls.Label("TotemAspectAddChanceLabel".Translate(Settings.totemAspectAddChance
                                                                   .ToStringByStyle(ToStringStyle.PercentOne)));
            Settings.totemAspectAddChance = ls.Slider(Settings.totemAspectAddChance, 0, 1); 
            ls.End();
            base.DoSettingsWindowContents(inRect);

        }

        public override string SettingsCategory()
        {
            return "PMTribalModName".Translate(); 
        }

        public override void WriteSettings()
        {
            base.WriteSettings();

        }
    }

    public class PMTribalSettings : ModSettings
    {
        public float totemAspectAddChance = 0.2f; 
    }
}