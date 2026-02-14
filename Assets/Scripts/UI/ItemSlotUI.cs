using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LotG.UI
{
    public class ItemSlotUI : MonoBehaviour
    {
        [SerializeField] private Image borderImage;
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text quantityText;

        public event Action<ItemSlotUI> OnItemClicked;

        int healValue;

        private bool isEmpty = true;

        private void Awake()
        {
            //ResetSlotData();
            DeselectSlot();
        }

        public void ResetSlotData()
        {
            itemImage.gameObject.SetActive(false);
            isEmpty = true;
        }

        public void DeselectSlot()
        {
            borderImage.enabled = false;
        }

        public void SetSlotData(Sprite sprite, int quantity, int heal)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            quantityText.text = quantity + "";
            healValue = heal;
            isEmpty = false;
        }

        public void SlotSelected()
        {
            borderImage.enabled = true;
        }

        public void OnPointerClick(BaseEventData data)
        {
            PointerEventData pointerData = (PointerEventData)data;
            OnItemClicked?.Invoke(this);
        }

        public int GetHealValue()
        {
            return healValue;
        }
    }
}
