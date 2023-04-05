using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recipeNameText;
    [SerializeField] private Transform _iconContainer;
    [SerializeField] private Transform _iconTemplate;

    private void Awake()
    {
        _iconTemplate.gameObject.SetActive(false);
    }
    public void SetRecipeScriptableObj(RecipeScriptableObj recipeScriptableObj)
    {
        _recipeNameText.text = recipeScriptableObj.recipeName;
        foreach (Transform child in _iconContainer)
        {
            if (child == _iconTemplate) 
            {
                continue;
            }
          
        }
        foreach (KitchenObjectScriptableObj kitchenObjectScriptableObj in recipeScriptableObj.kitchenObjectScriptableObjList)
        {
            
            Transform iconTransform = Instantiate(_iconTemplate, _iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectScriptableObj.sprite;
        }
    }

}
