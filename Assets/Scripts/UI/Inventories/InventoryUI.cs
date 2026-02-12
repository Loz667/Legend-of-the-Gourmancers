using LotG.Inventories;
using System.Collections.Generic;
using UnityEngine;

namespace LotG.UI.Inventories
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] InventorySlotUI ItemSlotPrefab = null;

        List<InventorySlotUI> slotUIs = new List<InventorySlotUI>();
        

        public void InitializeUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                InventorySlotUI slotUI = 
                    Instantiate(ItemSlotPrefab, transform);
                slotUIs.Add(slotUI);
            }
        }

        public void UpdateData(int index, Sprite image, int quantity)
        {
            if (slotUIs.Count > index)
            {
                slotUIs[index].SetSlotData(image, quantity);
            }
        }

        public void ResetData(int index)
        {
            if (slotUIs.Count > index) 
            { 
                slotUIs[index].ResetSlotData(); 
            }
        }
    }
}
