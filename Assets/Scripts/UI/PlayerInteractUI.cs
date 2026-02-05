using TMPro;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] GameObject UIContainer;
    [SerializeField] TMP_Text interactText;

    public void ShowInteractUI()
    {
        UIContainer.SetActive(true);
    }

    public void HideInteractUI()
    {
        UIContainer.SetActive(false);
    }
}
