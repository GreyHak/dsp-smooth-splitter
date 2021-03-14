//
// Copyright (c) 2021, Aaron Shumate
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE.txt file in the root directory of this source tree. 
//
// Dyson Sphere Program is developed by Youthcat Studio and published by Gamera Game.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
namespace DSPSmoothSplitter
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    [BepInProcess("DSPGAME.exe")]
    public class DSPSmoothSplitter : BaseUnityPlugin
    {
        public const string pluginGuid = "greyhak.dysonsphereprogram.smoothsplitter";
        public const string pluginName = "DSP Smooth Splitter";
        public const string pluginVersion = "1.0.0";
        new internal static ManualLogSource Logger;
        new internal static BepInEx.Configuration.ConfigFile Config;
        Harmony harmony;
        public const int slotLength = 10;
        public const int cargoSlotLookAhead = 20 * slotLength;  // 15 isn't big enough

        public void Awake()
        {
            Logger = base.Logger;  // "C:\Program Files (x86)\Steam\steamapps\common\Dyson Sphere Program\BepInEx\LogOutput.log"
            Config = base.Config;  // "C:\Program Files (x86)\Steam\steamapps\common\Dyson Sphere Program\BepInEx\config\"
            Config.SaveOnConfigSet = false;

            harmony = new Harmony(pluginGuid);
            harmony.PatchAll(typeof(DSPSmoothSplitter));
        }

        // At the time of writing, this routine is only used by CargoTraffic.UpdateSplitter
        [HarmonyPostfix, HarmonyPatch(typeof(CargoPath), "TestBlankAtHead")]
        public static void CargoPath_TestBlankAtHead_Postfix(CargoPath __instance, ref bool __result)
        {
            if (__result)
            {
                return;
            }

            // See if we can make room...
            // Each slot takes up 10 bytes in the 'buffer'.
            // 'index' already contains the *10.
            // First 4 "bytes" are reserved for something.
            //    'index' is always >= 4
            //    'buffer' has no bytes for these 4
            // If an item isn't present, all ten bytes are zero.
            // If an item is present:
            //    [4 + index + 0-4] is 246, 247, 248, 249, 250
            //    [4 + index + 5,6,7,8] is the cargo number
            //    [4 + index + 9] is byte.MaxValue

            /*if (__instance.TryInsertCargo(4, 2000))
            {
                Array.Clear(__instance.buffer, 0, 10);
                Logger.LogDebug("Splitter insert upgraded.");
                __result = true;
            }*/
            // else do nothing

            List<int> cargoIds = new List<int>();
            int zeroCount = 0;
            for (int index10 = 0; true; index10 += slotLength)
            {
                if (index10 >= cargoSlotLookAhead)
                {
                    if (zeroCount == 0)
                    {
                        //Logger.LogDebug("Stop at artificial limit");
                    }
                    else if (zeroCount > slotLength)
                        Logger.LogDebug("Stop at artificial limit.  HAVE SPACE.");
                    else
                    {
                        //Logger.LogDebug($"Stop at artificial limit with {zeroCount} zeros");
                    }
                    break;
                }
                if (__instance.buffer[index10] == 0)
                {
                    for (; index10 < __instance.bufferLength && __instance.buffer[index10] == 0; ++index10)
                    {
                        zeroCount++;
                        if (zeroCount == slotLength)
                        {
                            Array.Clear(__instance.buffer, 0, index10);
                            int index = 4 + slotLength;
                            foreach (int cargoId_insert in cargoIds)
                            {
                                __instance.InsertCargoDirect(index, cargoId_insert);
                                index += slotLength;
                            }
                            if (index != index10 + 5)
                            {
                                Logger.LogError($"Splitter insert upgraded error. index10={index10}, index={index}");
                            }
                            else
                            {
                                Logger.LogMessage($"Splitter insert upgraded. index10={index10}, index={index}");
                            }
                            __result = true;
                            return;
                        }
                    }
                }
                if (index10 + 9 >= __instance.bufferLength)
                {
                    Logger.LogDebug("Short buffer");
                    break;
                }
                /*if (__instance.buffer[index10 + 9] == 0)
                {
                    Logger.LogDebug($"[{index10} + 9] is 0 showing the slot is empty");
                    break;
                }*/
                if (__instance.buffer[index10 + 0] != 246)
                {
                    Logger.LogDebug($"[{index10} + 0] not 246 = {__instance.buffer[index10 + 0]},{__instance.buffer[index10 + 1]},{__instance.buffer[index10 + 2]},{__instance.buffer[index10 + 3]},{__instance.buffer[index10 + 4]},{__instance.buffer[index10 + 5]},{__instance.buffer[index10 + 6]},{__instance.buffer[index10 + 7]},{__instance.buffer[index10 + 8]},{__instance.buffer[index10 + 9]}");
                    break;
                }
                if (__instance.buffer[index10 + 1] != 247)
                {
                    Logger.LogDebug($"[{index10} + 1] not 247");
                    break;
                }
                if (__instance.buffer[index10 + 2] != 248)
                {
                    Logger.LogDebug($"[{index10} + 2] not 248");
                    break;
                }
                if (__instance.buffer[index10 + 3] != 249)
                {
                    Logger.LogDebug($"[{index10} + 3] not 249");
                    break;
                }
                if (__instance.buffer[index10 + 4] != 250)
                {
                    Logger.LogDebug($"[{index10} + 4] not 250");
                    break;
                }
                if (__instance.buffer[index10 + 9] != byte.MaxValue)
                {
                    Logger.LogDebug($"[{index10} + 9] not {byte.MaxValue}");
                    break;
                }

                int cargoId_extract = (int)
                    (__instance.buffer[index10 + 5] - 1 +
                    (__instance.buffer[index10 + 6] - 1) * 100) +
                    (int)(__instance.buffer[index10 + 7] - 1) * 10000 +
                    (int)(__instance.buffer[index10 + 8] - 1) * 1000000;
                cargoIds.Add(cargoId_extract);
            }
        }
    }
}
