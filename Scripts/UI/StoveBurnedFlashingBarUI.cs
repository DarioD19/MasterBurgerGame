using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnedFlashingBarUI : MonoBehaviour
{
    [SerializeField] private StoveCounter StoveCounter;

    private const string IS_FLASHING = "IsFlashing";

    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StoveCounter.OnProgresChanged += StoveCounter_OnProgresChanged;
        _animator.SetBool(IS_FLASHING, false);
      
    }

    private void StoveCounter_OnProgresChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = 0.5f;
        bool show = StoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;
        _animator.SetBool(IS_FLASHING, show);
    }

}
