using LotG.Inventories;
using UnityEngine;

namespace LotG.UI.Inventories
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] InventorySlotUI ItemSlotPrefab = null;

        Inventory playerInventory;

        private void Awake()
        {
            playerInventory = Inventory.GetPlayerInventory();
            playerInventory.InventoryUpdated += Redraw;
        }

        private void Start()
        {
            Redraw();
        }

        private void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < playerInventory.GetSize(); i++)
            {
                var slotUI = Instantiate(ItemSlotPrefab, transform);
                slotUI.Setup(playerInventory, i);
            }
        }
    }
}
