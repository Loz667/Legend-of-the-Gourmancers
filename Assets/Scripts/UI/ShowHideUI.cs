using UnityEngine;

namespace LotG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
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

        private void Start()
        {
            uiContainer.SetActive(false);
        }

        private void Update()
        {
            if (controls.Player.Inventory.WasPressedThisFrame())
            {
                uiContainer.SetActive(!uiContainer.activeSelf);
            }
        }
    }
}
