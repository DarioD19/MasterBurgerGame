using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
   [SerializeField] private KitchenObjectScriptableObj _kitchenObjectScriptableObj;
    public override void Interact(PlayerController player)
    {
        if (!HasKitchenObject())
        {
            //There is no kitchen object here
            if (player.HasKitchenObject())
            {//Player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
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
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {//Player is holding a plate
                    
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectScriptableObj()))
                    {
                        GetKitchenObject().DestroySelf();
                    } 
                    
                }
                else
                {//Player is not carrying plate but something else
                    if (GetKitchenObject().TryGetPlate(out  plateKitchenObject))
                    {//Counter is holding a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectScriptableObj()))
                        {
                            player.GetKitchenObject().DestroySelf();
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
      

       
  
    
 

    

   
  
}
