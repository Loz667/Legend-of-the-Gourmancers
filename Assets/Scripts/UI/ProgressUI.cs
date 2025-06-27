using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGO;
    [SerializeField] private Image fillImage;

    private IHasProgress _hasProgress;
    
    private void Start()
    {
        _hasProgress = hasProgressGO.GetComponent<IHasProgress>();
        if (_hasProgress == null){Debug.LogError("Attached GameObject does not implement IHasProgress");}
        
        _hasProgress.OnProgressChanged += ProgressChanged;

        fillImage.fillAmount = 0f;

        Hide();
    }

    private void ProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
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
