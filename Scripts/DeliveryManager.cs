using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
     public static DeliveryManager Instance { get; private set; }
    
    [SerializeField] private RecipeListScriptableObj recipeListScriptableObj;
    private List<RecipeScriptableObj> _waitingRecipeScriptableObjList;
    private float _spawnRecipeTimer;
    private float _spawnRecipeTimerMax = 4f;
    private int _waitingRecipesMax = 4;
    private int _succesfulRecipesAmount;


    private void Awake()
    {
        Instance = this;
        _waitingRecipeScriptableObjList = new List<RecipeScriptableObj>();

    }
    private void Update()
    {
        _spawnRecipeTimer -= Time.deltaTime;
        if (_spawnRecipeTimer <= 0f)
        {
            _spawnRecipeTimer = _spawnRecipeTimerMax;
            if (GameManager.Instance.IsGamePlaying() && _waitingRecipeScriptableObjList.Count < _waitingRecipesMax)
            {
                RecipeScriptableObj waitingRecipeScriptableObj = recipeListScriptableObj.recipeScriptableObjList[UnityEngine.Random.Range(0, recipeListScriptableObj.recipeScriptableObjList.Count)];
                _waitingRecipeScriptableObjList.Add(waitingRecipeScriptableObj);
                
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);

            }
         
        }
    }
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < _waitingRecipeScriptableObjList.Count; i++)
        {
            RecipeScriptableObj waitingRecipeScritpableObj = _waitingRecipeScriptableObjList[i];
            if (waitingRecipeScritpableObj.kitchenObjectScriptableObjList.Count == plateKitchenObject.GetKitchenObjectScriptableObjList().Count)
            {//Has the same number of ingredients
                bool plateContentMatchesRecipe = true;
                foreach (KitchenObjectScriptableObj recipeKitchenObjectScriptableObject in waitingRecipeScritpableObj.kitchenObjectScriptableObjList)
                {//Cycling trough all ingredients in the recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectScriptableObj plateKitchenObjectScriptableObject in plateKitchenObject.GetKitchenObjectScriptableObjList())
                    {//Cycling trough all ingredients in the plate
                        if (plateKitchenObjectScriptableObject == recipeKitchenObjectScriptableObject)
                        {//Ingredients matches!
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {//This Recipe ingredient was not found on the Plate
                        plateContentMatchesRecipe = false;
                    }
                }
                if (plateContentMatchesRecipe)
                {//Player delivered the correct recipe!
                   
                    _succesfulRecipesAmount++;
                    _waitingRecipeScriptableObjList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this,EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
                   
            }
        }
        //No matches found!
        //Player did not deliver the correct recipe
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        

    }
    public List<RecipeScriptableObj> GetWaitingRecipeScriptableObjList()
    {
        return _waitingRecipeScriptableObjList;
    }
    public int GetSuccesfulRecipeAmount()
    {
        return _succesfulRecipesAmount;
    }
}
