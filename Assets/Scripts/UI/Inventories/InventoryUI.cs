using System.Collections.Generic;
using UnityEngine;

namespace LotG.UI.Inventories
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] InventorySlotUI SlotPrefab = null;

        List<InventorySlotUI> slotUIs = new List<InventorySlotUI>();        

        public void InitializeUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                InventorySlotUI slotUI = 
                    Instantiate(SlotPrefab, transform);
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

        public void ResetData()
        {
            foreach (var slotUI in slotUIs)
            {
                slotUI.ResetSlotData();
            }
        }
    }
}
