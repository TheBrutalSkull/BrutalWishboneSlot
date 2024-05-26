using BepInEx;
using HarmonyLib;
using AzuExtendedPlayerInventory;
using BepInEx.Logging;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;


namespace WishboneSlot
{
    [BepInPlugin("thebrutalskull.Brutal_WishboneSlot", "Brutal_WishboneSlot", "1.0.2")]
    [BepInDependency("Azumatt.AzuExtendedPlayerInventory", BepInDependency.DependencyFlags.HardDependency)]
    public class WishboneSlot : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("thebrutalskull.Brutal_WishboneSlot");
        public static readonly ManualLogSource WishboneSlotLogger = BepInEx.Logging.Logger.CreateLogSource("Brutal_WishboneSlot");

        void Awake()
        {
            
            if(SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null)
            {
                WishboneSlotLogger.LogWarning("This mod is client-side only and is not needed on a dedicated server. Plugin patches will not be applied.");
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
    }
}
