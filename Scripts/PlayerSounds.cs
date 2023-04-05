using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private PlayerController Player;
    private float _footstepTimer;
    private float _footstepTimerMax = 0.1f;
    private void Awake()
    {
      Player =  GetComponent<PlayerController>();  
    }

    private void Update()
    {
        _footstepTimer -= Time.deltaTime;
        if (_footstepTimer < 0f)
        {
            _footstepTimer = _footstepTimerMax;
            if (Player.IsWalking())
            {
                float volume = 1f;
                SoundManager.Instance.PlayFootStepsSound(Player.transform.position, volume);
            }
            
        }
    }
}
