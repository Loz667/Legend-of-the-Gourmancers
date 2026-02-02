using StarterAssets;
using UnityEngine;

namespace LotG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] private GameObject uiContainer;

        StarterAssetsInputs inputs;

        private void Awake()
        {
            inputs = FindFirstObjectByType<StarterAssetsInputs>();
        }

        private void Start()
        {
            uiContainer.SetActive(false);
        }

        private void Update()
        {
            if (inputs.inventory)
            {
                uiContainer.SetActive(!uiContainer.activeSelf);
                inputs.InventoryInput(false);
            }
        }
    }
}
