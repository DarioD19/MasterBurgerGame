using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter,IHasProgress
{
    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }
    public event EventHandler <IHasProgress.OnProgressChangedEventArgs>OnProgresChanged;
  
    [SerializeField] private CuttingRecipeScriptableObj[] CuttingRecipeScriaptableObjArray;

    private int _cuttingProgress; 
    public override void Interact(PlayerController player)
    {

        if (!HasKitchenObject())
        {
            //There is no kitchen object here
            if (player.HasKitchenObject())
            {//Player is carrying something
                if (HasRecipeWithInpuut(player.GetKitchenObject().GetKitchenObjectScriptableObj()))
                {//Player carrying something thath can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    _cuttingProgress = 0;

                    CuttingRecipeScriptableObj cuttingRecipeScriptableObj = GetCuttingRecipeScriptableObjWithInput(GetKitchenObject().GetKitchenObjectScriptableObj());
                    OnProgresChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)_cuttingProgress / cuttingRecipeScriptableObj.cuttingProgressMax
                    });
                }
              
            }
            else
            {//Player not carriying anything

            }
        }
        else
        {
            //There is a kitchen object
            if (player.HasKitchenObject())
            {//Player is carrying something
                if (HasKitchenObject())
                {
                    if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                    {//Player is holding a plate

                        if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectScriptableObj()))
                        {
                            GetKitchenObject().DestroySelf();
                        }

                    }
                }
            }
            else
            {//Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
               
            }
        }
    }
    public override void InteractAlternate(PlayerController player)
    {
        if (HasKitchenObject()&& HasRecipeWithInpuut(GetKitchenObject().GetKitchenObjectScriptableObj()))
        {//There is a KitchenObject here and it can be cut
            _cuttingProgress++;
           
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeScriptableObj cuttingRecipeScriptableObj = GetCuttingRecipeScriptableObjWithInput(GetKitchenObject().GetKitchenObjectScriptableObj());

            OnProgresChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)_cuttingProgress / cuttingRecipeScriptableObj.cuttingProgressMax
            });

            if (_cuttingProgress >= cuttingRecipeScriptableObj.cuttingProgressMax)
            {
                KitchenObjectScriptableObj outputKitchenObjecSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectScriptableObj());
                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjecSO, this);
            }
           
           
        }
    }
    private bool HasRecipeWithInpuut(KitchenObjectScriptableObj inputKitchenObjectScriptableObj)
    {
        CuttingRecipeScriptableObj cuttingRecipeScriptableObj = GetCuttingRecipeScriptableObjWithInput(inputKitchenObjectScriptableObj);
        return cuttingRecipeScriptableObj != null;
    }
    private KitchenObjectScriptableObj GetOutputForInput(KitchenObjectScriptableObj inputKitchenObjectScriptableObj)
    {
        CuttingRecipeScriptableObj cuttingRecipeScriptableObj = GetCuttingRecipeScriptableObjWithInput(inputKitchenObjectScriptableObj);
        if (cuttingRecipeScriptableObj != null)
        {
         return cuttingRecipeScriptableObj.Output;
        }
        else
        {
            return null;
        }
      
    }
    private CuttingRecipeScriptableObj GetCuttingRecipeScriptableObjWithInput(KitchenObjectScriptableObj inputKitchenObjectScriptableObj)
    {
        foreach (CuttingRecipeScriptableObj cuttingRecipeSO in CuttingRecipeScriaptableObjArray)
        {
            if (cuttingRecipeSO.Input == inputKitchenObjectScriptableObj)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
