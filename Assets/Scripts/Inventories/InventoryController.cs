using LotG.UI.Inventories;
using UnityEngine;

namespace LotG.Inventories
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventoryUI inventoryUI;
        [SerializeField] private InventorySO inventoryData;
        [SerializeField] private GameObject uiContainer;

        PlayerControls controls;

        private void Awake()
        {
            controls = new PlayerControls();
        }

        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }

        private void Start()
        {
            inventoryUI.InitializeUI(inventoryData.inventorySize);
            inventoryData.InitializeInventory();
            uiContainer.SetActive(false);
        }

        private void Update()
        {
            if (controls.Player.Inventory.WasPressedThisFrame())
            {
                uiContainer.SetActive(!uiContainer.activeSelf);

                foreach (var item in inventoryData.GetCurrentState())
                {
                    inventoryUI.UpdateData(item.Key, item.Value.item.GetIcon(),
                        item.Value.quantity);
                }
            }
        }
    }
}
