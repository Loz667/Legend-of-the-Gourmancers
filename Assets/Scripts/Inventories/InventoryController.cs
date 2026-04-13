using LotG.Control;
using LotG.UI.Inventories;
using UnityEngine;

namespace LotG.Inventories
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventoryUI inventoryUI;
        [SerializeField] private InventorySO inventoryData;
        [SerializeField] private GameObject inventoryContainer;
        [SerializeField] private GameObject recipeContainer;
        [SerializeField] private GameObject controlContainer;
        [SerializeField] private GameObject questContainer;

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
            inventoryContainer.SetActive(false);
            recipeContainer.SetActive(false);
            questContainer.SetActive(false);
        }

        private void Update()
        {
            if (controls.Player.Inventory.WasPressedThisFrame())
            {
                inventoryContainer.SetActive(!inventoryContainer.activeSelf);
                controlContainer.SetActive(!controlContainer.activeSelf);
                recipeContainer.SetActive(false);
                questContainer.SetActive(false);
                transform.GetComponent<PlayerController>().enabled = !inventoryContainer.activeSelf;

                foreach (var item in inventoryData.GetCurrentState())
                {
                    inventoryUI.UpdateData(item.Key, item.Value.item.GetIcon(),
                        item.Value.quantity);
                }
            }

            if (controls.Player.RecipeBook.WasPressedThisFrame())
            {
                recipeContainer.SetActive(!recipeContainer.activeSelf);
                controlContainer.SetActive(!controlContainer.activeSelf);
                inventoryContainer.SetActive(false);
                questContainer.SetActive(false);
                transform.GetComponent<PlayerController>().enabled = !recipeContainer.activeSelf;
            }

            if (controls.Player.QuestLogToggle.WasPressedThisFrame())
            {
                questContainer.SetActive(!questContainer.activeSelf);
                controlContainer.SetActive(!controlContainer.activeSelf);
                inventoryContainer.SetActive(false);
                recipeContainer.SetActive(false);
                transform.GetComponent<PlayerController>().enabled = !questContainer.activeSelf;
            }
        }
    }
}
