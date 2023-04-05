using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectScritpableObj_GameObject
    {
        public KitchenObjectScriptableObj KitchenObjectScriptableObj;
        public GameObject gameObject;
    }
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectScritpableObj_GameObject> kitchenObjectScritpableObj_GameObjectList;
    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        foreach (KitchenObjectScritpableObj_GameObject kitchenObjectScriptableObjGameObj in kitchenObjectScritpableObj_GameObjectList)
        {
           
            
                kitchenObjectScriptableObjGameObj.gameObject.SetActive(false);
            
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectScritpableObj_GameObject kitchenObjectScriptableObjGameObj in kitchenObjectScritpableObj_GameObjectList)
        {
            if (kitchenObjectScriptableObjGameObj.KitchenObjectScriptableObj == e.kitchenObjectScriptableObj)
            {
                kitchenObjectScriptableObjGameObj.gameObject.SetActive(true);
            }
        }
        
    }
}
