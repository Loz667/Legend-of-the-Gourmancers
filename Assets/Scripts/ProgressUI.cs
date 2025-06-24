using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter counter;
    [SerializeField] private Image fillImage;

    private void Start()
    {
        counter.OnProgressChanged += ProgressChanged;

        fillImage.fillAmount = 0f;

        Hide();
    }

    private void ProgressChanged(object sender, CuttingCounter.OnProgressChangedEventArgs e)
    {
        fillImage.fillAmount = e.progressNormalised;

        if (e.progressNormalised == 0f || e.progressNormalised == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
