using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LotG.Inventories
{
    [CreateAssetMenu(fileName = "New InventorySO", menuName = "Scriptable Objects/Inventories/InventorySO")]
    public class InventorySO : ScriptableObject
    {
        [Tooltip("Maximum number of inventory slots available.")]
        //[SerializeField] List<InventoryItem> inventoryItems;
        [SerializeField] InventoryItem[] inventoryItems;
        [field: SerializeField] public int inventorySize { get; private set; } = 16;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        public void InitializeInventory()
        {
            //inventoryItems = new InventoryItem[inventorySize];
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (!inventoryItems[i].IsEmpty) continue;
                inventoryItems[i] = InventoryItem.GetEmptyItem();
            }
        }

        public Dictionary<int, InventoryItem> GetCurrentState()
        {
            Dictionary<int, InventoryItem> returnValue = 
                new Dictionary<int, InventoryItem>();
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i].IsEmpty) continue;
                returnValue[i] = inventoryItems[i];
            }
            return returnValue;
        }

        public int AddItem(ItemSO item, int quantity)
        {
            if (!item.IsStackable())
            {
                for (int i = 0; i < inventoryItems.Length; i++)
                {
                    while (quantity > 0 && !IsInventoryFull())
                    {
                        quantity -= AddItemToFirstEmptySlot(item, 1);
                    }
                
                    NotifyInventoryUpdated();
                    return quantity;
                }
            }
            quantity = AddStackableItem(item, quantity);
            NotifyInventoryUpdated();
            return quantity;
        }

        public int AddItemToFirstEmptySlot(ItemSO item, int quantity)
        {
            InventoryItem newItem = new InventoryItem
            {
                item = item,
                quantity = 1
            };

            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }
            return 0;
        }

        private int AddStackableItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i].IsEmpty) continue;

                if (inventoryItems[i].item.GetInstanceID() == item.GetInstanceID())
                {
                    int amountPossibleToTake =
                        inventoryItems[i].item.GetMaxStackSize() - inventoryItems[i].quantity;

                    if (quantity > amountPossibleToTake)
                    {
                        inventoryItems[i] =
                            inventoryItems[i].ChangeQuantity(inventoryItems[i].item.GetMaxStackSize());
                        quantity -= amountPossibleToTake;
                    }
                    else
                    {
                        inventoryItems[i] =
                            inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);
                        NotifyInventoryUpdated();
                        return 0;
                    }
                }
            }

            while (quantity > 0 && !IsInventoryFull())
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.GetMaxStackSize());
                quantity -= newQuantity;
                AddItemToFirstEmptySlot(item, newQuantity); 
            }
            return quantity;
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item.item, item.quantity);
        }

        public InventoryItem GetItem(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        public bool HasItem(ItemSO item)
        {
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (object.ReferenceEquals(inventoryItems[i].item, item))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasSpaceFor(ItemSO item)
        {
            return FindSlot(item) >= 0;
        }

        public bool RemoveItem(ItemSO item)
        {
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (object.ReferenceEquals(inventoryItems[i].item, item))
                {
                    RemoveFromSlot(i, 1);
                    return true;
                }
            }
            return false;
        }

        public void RemoveFromSlot(int index, int quantity)
        {
            if (inventoryItems.Length > index)
            {
                if (inventoryItems[index].IsEmpty)
                    return;

                int remainder = inventoryItems[index].quantity - quantity;
                if (remainder <= 0)
                {
                    inventoryItems[index] = InventoryItem.GetEmptyItem();
                }
                else
                {
                    inventoryItems[index] = inventoryItems[index].ChangeQuantity(remainder);
                }

                NotifyInventoryUpdated();
            }
        }

        private bool IsInventoryFull()
            => inventoryItems.Where(item => item.IsEmpty).Any() == false;

        private void NotifyInventoryUpdated() { OnInventoryUpdated?.Invoke(GetCurrentState()); }

        private int FindSlot(ItemSO item)
        {
            int i = FindStack(item);
            if (i < 0)
            {
                i = FindEmptySlot();
            }
            return i;
        }

        private int FindEmptySlot()
        {
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i].item == null)
                {
                    return i;
                }
            }
            return -1;
        }

        private int FindStack(ItemSO item)
        {
            if (!item.IsStackable())
            {
                return -1;
            }

            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (object.ReferenceEquals(inventoryItems[i].item, item))
                {
                    return i;
                }
            }
            return -1;
        }       
    }

    [Serializable]
    public struct InventoryItem
    {
        public ItemSO item;
        public int quantity;

        public bool IsEmpty => item == null;

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem { item = this.item, quantity = newQuantity };
        }

        public static InventoryItem GetEmptyItem()
            => new InventoryItem { item = null, quantity = 0 };
    }
}