using LotG.Inventories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LotG.UI.Inventories
{
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        [SerializeField] GameObject textContainer = null;
        [SerializeField] TextMeshProUGUI itemNumber = null;

        public void SetItem(ItemSO item)
        {
            SetItem(item, 0);
        }

        public void SetItem(ItemSO item, int number)
        {
            Image image = GetComponent<Image>();
            if (item == null)
            {
                image.enabled = false;
            }
            else
            {
                image.enabled = true;
                image.sprite = item.GetIcon();
            }

            if (itemNumber)
            {
                if (number <= 1)
                {
                    textContainer.SetActive(false);
                }
                else
                {
                    textContainer.SetActive(true);
                    itemNumber.text = number.ToString();
                }
            }
        }
    }
}
