using System.Collections.Generic;
using UnityEngine;

namespace LotG.Inventories
{
    [CreateAssetMenu(fileName = "New InventoryItemSO", menuName = "Scriptable Objects/Inventories/InventoryItemSO")]
    public class InventoryItemSO : ScriptableObject, ISerializationCallbackReceiver
    {
        [Tooltip("Auto-generated UUID of the inventory item.")]
        [SerializeField] string ItemID = null;
        [Tooltip("The display name of the inventory item.")]
        [SerializeField] string ItemName = null;
        [Tooltip("The description of the inventory item.")]
        [SerializeField][TextArea] string ItemDescription = null;
        [Tooltip("The icon representing the inventory item.")]
        [SerializeField] Sprite ItemIcon = null;
        [Tooltip("The value of health this meal will restore")]
        [SerializeField] int HealthRestoreValue = 0;
        [Tooltip("If true, multiiple items of same type can be stacked in same slot.")]
        [SerializeField] bool Stackable = false;
        [Tooltip("The maximum stack size for this item if it is stackable. Ignored if Stackable is false.")]
        [SerializeField] int MaxStackSize = 99;

        static Dictionary<string, InventoryItemSO> itemLookupCache;

        public static InventoryItemSO GetFromID(string itemID)
        {
            if (itemLookupCache == null)
            {
                itemLookupCache = new Dictionary<string, InventoryItemSO>();
                var itemList = Resources.LoadAll<InventoryItemSO>("");
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

        public string GetItemName()
        {
            return ItemName;
        }

        public string GetItemDescription()
        {
            return ItemDescription;
        }

        public int GetHealthRestoreValue()
        {
            return HealthRestoreValue;
        }

        public bool IsStackable()
        {
            return Stackable;
        }

        public int GetMaxStackSize()
        {
            return MaxStackSize;
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
