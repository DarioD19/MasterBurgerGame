using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{

    [SerializeField] private GameObject _hasProgressGameObject;
    [SerializeField] private Image _barImage;
    private IHasProgress HasProgress;

    private void Start()
    {
        HasProgress = _hasProgressGameObject.GetComponent<IHasProgress>();
        if (HasProgress == null)
        {
            Debug.LogError("GameObject " + _hasProgressGameObject+ " does not have component that implements IHasProgres!");
        }
        HasProgress.OnProgresChanged += HasProgress_OnProgresChanged;
        _barImage.fillAmount = 0f;
        Hide();
    }

    private void HasProgress_OnProgresChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        _barImage.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0f|| e.progressNormalized == 1f)
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
