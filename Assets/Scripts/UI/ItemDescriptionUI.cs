using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LotG.UI
{
    public class ItemDescriptionUI : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;

        public void Awake()
        {
            ResetDescription();
        }

        public void ResetDescription()
        {
            this.itemImage.gameObject.SetActive(false);
            this.titleText.text = string.Empty;
            this.descriptionText.text = string.Empty;
        }

        public void SetDescription(Sprite sprite, string itemName, string itemDescription)
        {
            this.itemImage.gameObject.SetActive(true);
            this.itemImage.sprite = sprite;
            this.titleText.text = itemName;
            this.descriptionText.text = itemDescription;
        }
    }
}
