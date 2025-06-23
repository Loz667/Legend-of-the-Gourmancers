using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter counter;
    [SerializeField] private GameObject selectedVisual;

    private void Start()
    {
        PlayerController.Instance.OnSelectedCounterChanged += UpdateSelectedCounterVisual;
    }

    private void UpdateSelectedCounterVisual(object sender, PlayerController.OnSelectedCounterChangedEventArgs e)
    {
        if (e.SelectedCounter == counter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        selectedVisual.SetActive(true);
    }

    private void Hide()
    {
        selectedVisual.SetActive(false);
    }
}
