using System.Collections.Generic;
using UnityEngine;

namespace LotG.Inventories
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "Scriptable Objects/Inventory/Item")]
    public class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        [Tooltip("Auto-generated UUID of the inventory item.")]
        [SerializeField] string ItemID = null;
        [Tooltip("The display name of the inventory item.")]
        [SerializeField] string ItemName = null;
        [Tooltip("The description of the inventory item.")]
        [SerializeField][TextArea] string ItemDescription = null;
        [Tooltip("The icon representing the inventory item.")]
        [SerializeField] Sprite ItemIcon = null;
        [Tooltip("If true, multiiple items of same type can be stacked in same slot.")]
        [SerializeField] bool IsStackable = false;

        static Dictionary<string, InventoryItem> itemLookupCache;

        public static InventoryItem GetFromID(string itemID)
        {
            if (itemLookupCache == null)
            {
                itemLookupCache = new Dictionary<string, InventoryItem>();
                var itemList = Resources.LoadAll<InventoryItem>("");
                foreach ( var item in itemList)
                {
                    if (itemLookupCache.ContainsKey(item.ItemID))
                    {
                        Debug.LogWarning($"Duplicate InventoryItem ID detected: {item.ItemID} in item {item.name}. Each InventoryItem must have a unique ID.");
                        continue;
                    }

                    itemLookupCache[item.ItemID] = item;
                }
            }

            if (itemID == null || !itemLookupCache.ContainsKey(itemID))
            {
                Debug.LogWarning($"InventoryItem with ID {itemID} not found.");
                return null;
            }
            return itemLookupCache[itemID];
        }

        public Sprite GetIcon()
        {
            return ItemIcon;
        }

        public string GetItemID()
        {
            return ItemID;
        }

        public bool IsItemStackable()
        {
            return IsStackable;
        }

        public string GetItemName()
        {
            return ItemName;
        }

        public string GetItemDescription()
        {
            return ItemDescription;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (string.IsNullOrWhiteSpace(ItemID))
            {
                ItemID = System.Guid.NewGuid().ToString();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            // Nothing needed here, just required by interface.
        }
    }
}
