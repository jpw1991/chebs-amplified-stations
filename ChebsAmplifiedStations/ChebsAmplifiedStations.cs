using System.Collections.Generic;
using BepInEx;
using ChebsAmplifiedStations.CraftingStations;
using HarmonyLib;
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
        public const string PluginVersion = "0.0.2";
        
        private readonly Harmony _harmony = new(PluginGuid);

        private ArtisanTable _artisanTable = new();
        private BlackForge _blackForge = new();
        private Cauldron _cauldron = new();
        private Forge _forge = new();
        private Workbench _workbench = new();
        

        private void Awake()
        {
            CreateConfigValues();
            
            _harmony.PatchAll();

            PrefabManager.OnVanillaPrefabsAvailable += DoOnVanillaPrefabsAvailable;
        }

        private void DoOnVanillaPrefabsAvailable()
        {
            PrefabManager.OnVanillaPrefabsAvailable -= DoOnVanillaPrefabsAvailable;

            foreach (var prefabName in new List<string>()
                     {
                         _artisanTable.PrefabName,
                         _blackForge.PrefabName,
                         _cauldron.PrefabName,
                         _forge.PrefabName,
                         _workbench.PrefabName,
                     })
            {
                CraftingStations.CraftingStation.SetRange(prefabName);
            }
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