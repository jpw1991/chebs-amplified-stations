using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
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
        public const string PluginVersion = "1.1.2";
        
        public static ConfigEntry<bool> DaisyChainEnabled;
        
        private readonly Harmony _harmony = new(PluginGuid);

        private ArtisanTable _artisanTable = new();
        private BlackForge _blackForge = new();
        private Cauldron _cauldron = new();
        private Forge _forge = new();
        private Workbench _workbench = new();
        private Stonecutter _stonecutter = new();
        

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
                         _stonecutter.PrefabName,
                     })
            {
                CraftingStations.CraftingStation.SetRange(prefabName);
            }
        }

        private void CreateConfigValues()
        {
            Config.SaveOnConfigSet = true;
            
            DaisyChainEnabled = Config.Bind("DaisyChain", "DaisyChainEnabled",
                true, new ConfigDescription(
                    "Whether or not daisy-chaining is enabled (connect the effects of the workstations that are within range of each other).", null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));

            _artisanTable.CreateConfigs(this);
            _blackForge.CreateConfigs(this);
            _cauldron.CreateConfigs(this);
            _forge.CreateConfigs(this);
            _workbench.CreateConfigs(this);
            _stonecutter.CreateConfigs(this);
        }
    }
}