using BepInEx;
using HarmonyLib;
using AzuExtendedPlayerInventory;
using BepInEx.Logging;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;


namespace WishboneSlot
{
    [BepInPlugin("thebrutalskull.Brutal_WishboneSlot", "Brutal_WishboneSlot", "1.0.1")]
    [BepInDependency("Azumatt.AzuExtendedPlayerInventory", BepInDependency.DependencyFlags.SoftDependency)]
    public class WishboneSlot : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("thebrutalskull.Brutal_WishboneSlot");
        private static readonly ManualLogSource PELogger;

        void Awake()
        {
            BepInEx.Logging.Logger.Sources.Add(PELogger);
            if(SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null)
            {
                Dbgl("This mod is client-side only and is not needed on a dedicated server. Plugin patches will not be applied.", true, LogLevel.Warning);
                return;
            }

            if (API.IsLoaded())
            {
                API.AddSlot("Wishbone", GetItem, isWishbone);
            }
            //Patch all the code below
            harmony.PatchAll();
        }

        private ItemDrop.ItemData GetItem(Humanoid player)
        {
            ItemDrop.ItemData wishboneSlot = player.GetInventory().GetEquippedItems().FirstOrDefault(i => i != null && i.m_dropPrefab && i.m_dropPrefab.name is "Wishbone");
            return wishboneSlot;
        }

        //Check if the item is the Wishbone
        private bool isWishbone(ItemDrop.ItemData item) 
        {
            return item != null && item.m_dropPrefab && item.m_dropPrefab.name is "Wishbone";
        }

        //Logger Tool
        internal static void Dbgl(string message, bool forceLog = false, LogLevel level = LogLevel.Info)
        {
            if (forceLog)
            {
                switch (level)
                {
                    case LogLevel.Error:
                        PELogger.LogError(message); 
                        break;
                    case LogLevel.Warning:
                        PELogger.LogWarning(message); 
                        break;
                    case LogLevel.Info:
                        PELogger.LogInfo(message); 
                        break;
                    case LogLevel.Message:
                        PELogger.LogMessage(message); 
                        break;
                    case LogLevel.Debug:
                        PELogger.LogDebug(message);
                        break;
                }
            }
        }
    }
}
