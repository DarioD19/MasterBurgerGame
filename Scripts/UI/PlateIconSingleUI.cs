using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconSingleUI : MonoBehaviour
{
    [SerializeField] private Image image;
   public void SetKitchenObjectScriptableObj(KitchenObjectScriptableObj kitchenObjectScriptableObj)
    {
        image.sprite = kitchenObjectScriptableObj.sprite;
    }
}
