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
    
    /*
     *   public static CraftingStation HaveBuildStationInRange(string name, Vector3 point)
          {
            foreach (CraftingStation allStation in CraftingStation.m_allStations)
            {
              if (!(allStation.m_name != name))
              {
                float stationBuildRange = allStation.GetStationBuildRange();
                point.y = allStation.transform.position.y;
                if ((double) Vector3.Distance(allStation.transform.position, point) < (double) stationBuildRange)
                  return allStation;
              }
            }
            return (CraftingStation) null;
          }
     */ 
    
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
                if (Vector3.Distance(craftingStation.transform.position, point) < craftingStation.m_rangeBuild)
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
                if (Vector3.Distance(craftingStation.transform.position, from.transform.position) < Mathf.Max(craftingStation.m_rangeBuild, from.m_rangeBuild))
                {
                    RecursiveAddNearbyStations(craftingStation, ref networkStations, allStations);
                }
            }
        }
        
        // Additional candidates to check:
        //
        /*
           public static void UpdateKnownStationsInRange(Player player)
          {
            Vector3 position = player.transform.position;
            foreach (CraftingStation allStation in CraftingStation.m_allStations)
            {
              if ((double) Vector3.Distance(allStation.transform.position, position) < (double) allStation.m_discoverRange)
                player.AddKnownStation(allStation);
            }
          }
          
            public float GetStationBuildRange()
          {
            float stationBuildRange = this.m_rangeBuild + (float) this.GetLevel() * this.m_extraRangePerLevel;
            if ((bool) (Object) this.m_areaMarker)
              this.m_areaMarkerCircle.m_radius = stationBuildRange;
            return stationBuildRange;
          }

          public static void FindStationsInRange(
            string name,
            Vector3 point,
            float range,
            List<CraftingStation> stations)
          {
            foreach (CraftingStation allStation in CraftingStation.m_allStations)
            {
              if (!(allStation.m_name != name) && (double) Vector3.Distance(allStation.transform.position, point) < (double) range)
                stations.Add(allStation);
            }
          }

          public static CraftingStation FindClosestStationInRange(string name, Vector3 point, float range)
          {
            CraftingStation closestStationInRange = (CraftingStation) null;
            float num1 = 99999f;
            foreach (CraftingStation allStation in CraftingStation.m_allStations)
            {
              if (!(allStation.m_name != name))
              {
                float num2 = Vector3.Distance(allStation.transform.position, point);
                if ((double) num2 < (double) range && ((double) num2 < (double) num1 || (Object) closestStationInRange == (Object) null))
                {
                  closestStationInRange = allStation;
                  num1 = num2;
                }
              }
            }
            return closestStationInRange;
          }
         */
        
        // Adjust build circle dotted-line to be more exact
        /*
         public static CraftingStation GetCraftingStation(Vector3 point)
        {
          if (CraftingStation.m_triggerMask == 0)
            CraftingStation.m_triggerMask = LayerMask.GetMask("character_trigger");
          foreach (Collider collider in Physics.OverlapSphere(point, 0.1f, CraftingStation.m_triggerMask, QueryTriggerInteraction.Collide))
          {
            if (collider.gameObject.CompareTag("StationUseArea"))
            {
              CraftingStation componentInParent = collider.GetComponentInParent<CraftingStation>();
              if ((Object) componentInParent != (Object) null)
                return componentInParent;
            }
          }
          return (CraftingStation) null;
        }
         */
        // [HarmonyPostfix]
        // [HarmonyPatch(nameof(CraftingStation.GetCraftingStation))]
        // static void GetCraftingStationPostfix(Vector3 point, CraftingStation __instance, ref CraftingStation __result)
        // {
        //     
        //     if (__result == null)
        //     {
        //         var dir = (__instance.transform.position - point).normalized;
        //         var comparePoint = point + dir;
        //         // nothing found, do another check
        //         if (CraftingStation.m_triggerMask == 0)
        //             CraftingStation.m_triggerMask = LayerMask.GetMask("character_trigger");
        //         foreach (var collider in Physics.OverlapSphere(comparePoint, .1f, CraftingStation.m_triggerMask, QueryTriggerInteraction.Collide))
        //         {
        //             if (collider.gameObject.CompareTag("StationUseArea"))
        //             {
        //                 CraftingStation componentInParent = collider.GetComponentInParent<CraftingStation>();
        //                 if (componentInParent != null)
        //                 {
        //                     __result = componentInParent;
        //                     return;
        //                 }
        //             }
        //         }
        //     }
        // }
    }
}

