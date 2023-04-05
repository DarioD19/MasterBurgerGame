using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class FryingRecipeScriptableObj : ScriptableObject
{
    public KitchenObjectScriptableObj Input;
    public KitchenObjectScriptableObj Output;
    public float fryingTimerMax;
}
