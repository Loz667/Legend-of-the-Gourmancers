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
        [SerializeField] List<InventorySlot> inventorySlots;
        [field: SerializeField] public int inventorySize { get; private set; } = 16;

        public event Action<Dictionary<int, InventorySlot>> OnInventoryUpdated;

        public void Init()
        {
            inventorySlots = new List<InventorySlot>();
            for (int i = 0; i < inventorySize; i++)
            {
                inventorySlots.Add(InventorySlot.GetEmptySlot());
            }
        }

        public Dictionary<int, InventorySlot> GetCurrentState()
        {
            Dictionary<int, InventorySlot> returnValue = 
                new Dictionary<int, InventorySlot>();
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].IsEmpty) continue;
                returnValue[i] = inventorySlots[i];
            }
            return returnValue;
        }

        public int AddItem(InventoryItemSO item, int quantity)
        {
            if (!item.IsStackable())
            {
                for (int i = 0; i < inventorySlots.Count; i++)
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

        public int AddItemToFirstEmptySlot(InventoryItemSO item, int quantity)
        {
            InventorySlot newItem = new InventorySlot
            {
                item = item,
                quantity = 1
            };

            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].IsEmpty)
                {
                    inventorySlots[i] = newItem;
                    return quantity;
                }
            }
            return 0;
        }

        private int AddStackableItem(InventoryItemSO item, int quantity)
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].IsEmpty) continue;

                if (inventorySlots[i].item.GetInstanceID() == item.GetInstanceID())
                {
                    int amountPossibleToTake =
                        inventorySlots[i].item.GetMaxStackSize() - inventorySlots[i].quantity;

                    if (quantity > amountPossibleToTake)
                    {
                        inventorySlots[i] =
                            inventorySlots[i].ChangeQuantity(inventorySlots[i].item.GetMaxStackSize());
                        quantity -= amountPossibleToTake;
                    }
                    else
                    {
                        inventorySlots[i] =
                            inventorySlots[i].ChangeQuantity(inventorySlots[i].quantity + quantity);
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

        public void AddItem(InventorySlot item)
        {
            AddItem(item.item, item.quantity);
        }

        public bool HasItem(InventoryItemSO item)
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (object.ReferenceEquals(inventorySlots[i].item, item))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasSpaceFor(InventoryItemSO item)
        {
            return FindSlot(item) >= 0;
        }

        public bool RemoveItem(InventoryItemSO item)
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (object.ReferenceEquals(inventorySlots[i].item, item))
                {
                    RemoveFromSlot(i, 1);
                    return true;
                }
            }
            return false;
        }

        public void RemoveFromSlot(int index, int quantity)
        {
            if (inventorySlots.Count > index)
            {
                if (inventorySlots[index].IsEmpty)
                    return;

                int remainder = inventorySlots[index].quantity - quantity;
                if (remainder <= 0)
                {
                    inventorySlots[index] = InventorySlot.GetEmptySlot();
                }
                else
                {
                    inventorySlots[index] = inventorySlots[index].ChangeQuantity(remainder);
                    NotifyInventoryUpdated();
                }
            }
        }

        private bool IsInventoryFull()
            => inventorySlots.Where(item => item.IsEmpty).Any() == false;

        private void NotifyInventoryUpdated() { OnInventoryUpdated?.Invoke(GetCurrentState()); }

        private int FindSlot(InventoryItemSO item)
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
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].item == null)
                {
                    return i;
                }
            }
            return -1;
        }

        private int FindStack(InventoryItemSO item)
        {
            if (!item.IsStackable())
            {
                return -1;
            }

            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (object.ReferenceEquals(inventorySlots[i].item, item))
                {
                    return i;
                }
            }
            return -1;
        }

        [Serializable]
        public struct InventorySlot
        {
            public InventoryItemSO item;
            public int quantity;

            public bool IsEmpty => item == null;

            public InventorySlot ChangeQuantity(int newQuantity)
            {
                return new InventorySlot { item = this.item, quantity = newQuantity };
            }

            public static InventorySlot GetEmptySlot() 
                => new InventorySlot { item = null, quantity = 0 };
        }
    }

}