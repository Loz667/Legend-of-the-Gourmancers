using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LotG.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text quantityText;

        private bool isEmpty = true;

        private void OnEnable()
        {
            ResetSlotData();
        }
        
        public void ResetSlotData()
        {
            itemImage.gameObject.SetActive(false);
            isEmpty = true;
        }

        public void SetSlotData(Sprite sprite, int quantity)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            quantityText.text = quantity + "";
            isEmpty = false;
        }
    }
}
