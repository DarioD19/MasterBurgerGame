using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseCounter : MonoBehaviour,IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlacedHere;

    public static void ResetStaticData()
    {
        OnAnyObjectPlacedHere = null;
    }
    [SerializeField] private Transform _counterTopPoint;

    private KitchenObject KitchenObject;
  public virtual void Interact(PlayerController player)
    {
        Debug.LogError("BaseCounter.Interact");
    }
    public virtual void InteractAlternate(PlayerController player)
    {
       // Debug.LogError("BaseCounter.InteractAlternate");
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _counterTopPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.KitchenObject = kitchenObject;
        if (KitchenObject != null)
        {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }                                                                       
    }
    public KitchenObject GetKitchenObject()
    {
        return KitchenObject;
    }
    public void ClearKitchenObject()
    {
        KitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return KitchenObject != null;
    }

}
