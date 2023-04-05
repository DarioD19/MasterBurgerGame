using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeScriptableObj : ScriptableObject
{
    public List<KitchenObjectScriptableObj> kitchenObjectScriptableObjList;
    public string recipeName;
}
