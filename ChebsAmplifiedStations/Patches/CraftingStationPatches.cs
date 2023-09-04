using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace ChebsAmplifiedStations.Patches
{
    // This implementation has been copy & pasted from DaisyChain, with only a few minor tweaks/refinements to its
    // original implementation. You can find it here:
    // https://github.com/rolopogo/ValheimMods/blob/main/DaisyChain/DaisyChain/CraftingStation_Patch.cs
    //
    // Thanks to DaisyChain's developers.

    [HarmonyPatch(typeof(CraftingStation))] 
    public class CraftingStationPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(CraftingStation.HaveBuildStationInRange))]
        static void HaveBuildStationInRangePostfix(string name, Vector3 point, CraftingStation __instance, ref CraftingStation __result)
        {
            if (!ChebsAmplifiedStations.DaisyChainEnabled.Value) return;
            
            foreach (var craftingStation in CraftingStation.m_allStations)
            {
                if (Vector3.Distance(craftingStation.transform.position, point) < craftingStation.GetStationBuildRange())
                {
                    var networkStations = new List<CraftingStation>();
                    RecursiveAddNearbyStations(craftingStation, ref networkStations, CraftingStation.m_allStations);

                    foreach (var item in networkStations)
                    {
                        if (item.m_name == name)
                        {
                            __result = item;
                            craftingStation.ShowAreaMarker();
                        }
                    }
                }
            }

        }
        
        private static void RecursiveAddNearbyStations(CraftingStation from, ref List<CraftingStation> networkStations, List<CraftingStation> allStations)
        {
            if (networkStations.Contains(from)) return;

            networkStations.Add(from);

            foreach (var craftingStation in allStations)
            {
                if (Vector3.Distance(craftingStation.transform.position, from.transform.position) < Mathf.Max(craftingStation.GetStationBuildRange(), from.GetStationBuildRange()))
                {
                    RecursiveAddNearbyStations(craftingStation, ref networkStations, allStations);
                }
            }
        }
    }
}

