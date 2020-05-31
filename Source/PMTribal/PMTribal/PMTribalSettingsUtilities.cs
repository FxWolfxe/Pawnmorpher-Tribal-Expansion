// PMTribalSettingsUtilities.cs created by Iron Wolf for PMTribal on 05/31/2020 8:33 AM
// last updated 05/31/2020  8:33 AM

using Verse;

namespace PMTribal
{
    public static class PMTribalSettingsUtilities
    {
        public static PMTribalSettings Settings => LoadedModManager.GetMod<PMTribalMod>().Settings;

        public static float TotemAspectAddChance => Settings.totemAspectAddChance;

        public static bool MutagenBingesEnabled => Settings.enableMutagenBinges; 
    }
}