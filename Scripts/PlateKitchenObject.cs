using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectScriptableObj kitchenObjectScriptableObj;
    }
    [SerializeField] private List<KitchenObjectScriptableObj> _validKitchenObjectScriptableObjList;
    private List<KitchenObjectScriptableObj> kitchenObjectScriptableObjList;
    private void Awake()
    {
        kitchenObjectScriptableObjList = new List<KitchenObjectScriptableObj>();
    }
    public bool TryAddIngredient(KitchenObjectScriptableObj kitchenObjectScriptableObj)
    {
        if (!_validKitchenObjectScriptableObjList.Contains(kitchenObjectScriptableObj))
        {//Not valid ingridient
            return false;
        }
        if (kitchenObjectScriptableObjList.Contains(kitchenObjectScriptableObj))
        {//Aleeady has this type
            return false;
        }
        else
        {
            kitchenObjectScriptableObjList.Add(kitchenObjectScriptableObj);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectScriptableObj = kitchenObjectScriptableObj
            }); 
            return true;
        }
        
    }
    public List<KitchenObjectScriptableObj> GetKitchenObjectScriptableObjList()
    {
        return kitchenObjectScriptableObjList;
    }
}
