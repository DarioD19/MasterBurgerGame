using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private PlayerController PlayerController;
    private Animator _animator;
    private void Awake()
    {
        
        
       _animator= GetComponent<Animator>();
       
    }
    private void Update()
    {
        _animator.SetBool(IS_WALKING, PlayerController.IsWalking());
    }
}
