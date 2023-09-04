using BepInEx;
using BepInEx.Configuration;
using Jotunn;
using Jotunn.Managers;

namespace ChebsAmplifiedStations.CraftingStations
{
    public class CraftingStation
    {
        /// <summary>
        /// How much the range will increase per level of the crafting station.
        /// </summary>
        public static ConfigEntry<float> LevelRangeIncrease;
        
        /// <summary>
        /// The base range of the crafting station.
        /// </summary>
        public static ConfigEntry<float> BaseRange;

        public virtual string PrefabName => "";

        public virtual void CreateConfigs(BaseUnityPlugin plugin)
        {
            var serverSynced = $"{GetType().Name} (Server Synced)";

            LevelRangeIncrease = plugin.Config.Bind(serverSynced, "LevelRangeIncrease",
                10f, new ConfigDescription(
                    "How much the range will increase per level of the crafting station.", null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));
            
            BaseRange = plugin.Config.Bind(serverSynced, "BaseRange",
                20f, new ConfigDescription(
                    "How much the range will increase per level of the crafting station.", null,
                    new ConfigurationManagerAttributes { IsAdminOnly = true }));
        }

        public static void SetRange(string prefabName)
        {
            var prefab = PrefabManager.Instance.GetPrefab(prefabName);
            if (prefab == null)
            {
                Logger.LogError($"Failed to get {prefabName} prefab.");
                return;
            }

            if (!prefab.TryGetComponent(out global::CraftingStation craftingStation))
            {
                Logger.LogError($"Failed to get CraftingStation script.");
                return;
            }

            craftingStation.m_rangeBuild = BaseRange.Value;
            craftingStation.m_extraRangePerLevel = LevelRangeIncrease.Value;
        }
    }
}