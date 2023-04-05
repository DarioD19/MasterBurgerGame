using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class CuttingRecipeScriptableObj : ScriptableObject
{
    public KitchenObjectScriptableObj Input;
    public KitchenObjectScriptableObj Output;
    public int cuttingProgressMax;
}
