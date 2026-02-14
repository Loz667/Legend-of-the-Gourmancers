using System;
using System.Collections.Generic;
using UnityEngine;
using LotG.Battle;

namespace LotG.UI
{
    public class ItemInventoryUI : MonoBehaviour
    {
        [SerializeField] RectTransform contentPanel;
        [SerializeField] ItemSlotUI itemPrefab = null;
        [SerializeField] ItemDescriptionUI itemDescription = null;

        public event Action<int> OnDescriptionRequested;

        BattleSystem battleSystem;

        List<ItemSlotUI> itemUIs = new List<ItemSlotUI>();

        private void Awake()
        {
            battleSystem = FindFirstObjectByType<BattleSystem>();
            itemDescription.ResetDescription();
        }

        public void Initialize(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                ItemSlotUI slotUI =
                    Instantiate(itemPrefab, contentPanel);
                itemUIs.Add(slotUI);

                slotUI.OnItemClicked += HandleItemClicked;
            }
        }

        private void HandleItemClicked(ItemSlotUI obj)
        {
            battleSystem.SelectItem(obj.GetHealValue());
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity, int itemHeal)
        {
            if (itemUIs.Count > itemIndex)
            {
                itemUIs[itemIndex].SetSlotData(itemImage, itemQuantity, itemHeal);
            }
        }

        public void ResetData(int itemIndex)
        {
            if (itemUIs.Count > itemIndex)
            {
                itemUIs[itemIndex].ResetSlotData();
            }
        }
    }
}
