using BepInEx;
using ChebsAmplifiedStations.CraftingStations;
using Jotunn;
using Jotunn.Managers;
using Jotunn.Utils;

namespace ChebsAmplifiedStations
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class ChebsAmplifiedStations : BaseUnityPlugin
    {
        public const string PluginGuid = "com.chebgonaz.chebsamplifiedstations";
        public const string PluginName = "ChebsAmplifiedStations";
        public const string PluginVersion = "0.0.1";

        private ArtisanTable _artisanTable = new();
        private BlackForge _blackForge = new();
        private Cauldron _cauldron = new();
        private Forge _forge = new();
        private Workbench _workbench = new();
        

        private void Awake()
        {
            CreateConfigValues();

            PrefabManager.OnVanillaPrefabsAvailable += DoOnVanillaPrefabsAvailable;
        }

        private void DoOnVanillaPrefabsAvailable()
        {
            PrefabManager.OnVanillaPrefabsAvailable -= DoOnVanillaPrefabsAvailable;
            
            _artisanTable.SetRange();
            _blackForge.SetRange();
            _cauldron.SetRange();
            _forge.SetRange();
            _workbench.SetRange();
        }

        private void CreateConfigValues()
        {
            Config.SaveOnConfigSet = true;

            _artisanTable.CreateConfigs(this);
            _blackForge.CreateConfigs(this);
            _cauldron.CreateConfigs(this);
            _forge.CreateConfigs(this);
            _workbench.CreateConfigs(this);
        }
    }
}