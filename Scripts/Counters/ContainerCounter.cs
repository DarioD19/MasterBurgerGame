using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectScriptableObj KitchenObjectScriptableObj;
   
    public override void Interact(PlayerController player)
    {
        if (!player.HasKitchenObject())
        {//Player is not carrying anything
            KitchenObject.SpawnKitchenObject(KitchenObjectScriptableObj, player);
            
        }
     
       
    }
        
        
       
   
}
