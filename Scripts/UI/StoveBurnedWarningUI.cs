using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnedWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter StoveCounter;

    private void Start()
    {
        StoveCounter.OnProgresChanged += StoveCounter_OnProgresChanged;
        Hide();
    }

    private void StoveCounter_OnProgresChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = 0.5f;
        bool show = StoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;
        if (show)
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
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
