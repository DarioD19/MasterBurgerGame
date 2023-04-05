using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    [SerializeField]
    private GameObject _pickupEffect;
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            playerInventory.DiamondCollected();
            Instantiate(_pickupEffect, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }
}
