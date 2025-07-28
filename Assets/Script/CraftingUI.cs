using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CraftingUI : MonoBehaviour
{
    [Header("Recipes")]
    public List<CraftingRecipe> allRecipes;
    public Transform recipeListContainer;
    public GameObject recipeButtonPrefab;
    [Header("Right Panel")]
    public TMP_Text resultNameText;
    public Image resultIcon;
    public Transform ingredientListContainer;
    public GameObject ingredientUIPrefab;
    public Button craftButton;

    private CraftingRecipe currentRecipe;
    void Start()
    {
        GenerateRecipeButtons();
        craftButton.onClick.AddListener(OnCraftClicked);
    }
    void GenerateRecipeButtons()
    {
        var buttons = recipeListContainer.GetComponentsInChildren<CraftingRecipeButton>();

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < allRecipes.Count)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].Setup(allRecipes[i], this);
            }
            else
            {
                buttons[i].gameObject.SetActive(false); 
            }
        }
    }
    public void DisplayRecipe(CraftingRecipe recipe)
    {
        Debug.Log("DisplayRecipe: " + recipe.resultItem.itemName);
        currentRecipe = recipe;
        resultNameText.text = recipe.resultItem.itemName;
        resultIcon.sprite = recipe.resultItem.itemSprite;

        foreach (Transform child in ingredientListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var ing in recipe.ingredients)
        {
            Debug.Log($"Ingredient: {ing.item.itemName} x{ing.amount}");
            GameObject go = Instantiate(ingredientUIPrefab, ingredientListContainer);
            var ingUI = go.GetComponent<CraftingIngredientUI>();
            ingUI.Setup(ing.item, ing.amount);
        }

        craftButton.interactable = CanCraft(recipe);
    }

    bool CanCraft(CraftingRecipe recipe)
    {
        foreach (var ing in recipe.ingredients)
        {
            if (!Player.Instance.HasItem(ing.item.itemName, ing.amount))
                return false;
        }
        return true;
    }
    void OnCraftClicked()
    {
        if (currentRecipe == null) return;
        if (!CanCraft(currentRecipe)) return;
        foreach (var ing in currentRecipe.ingredients)
        {
            Player.Instance.ConsumeItem(ing.item.itemName, ing.amount);
        }
        Player.Instance.AddToInventory(currentRecipe.resultItem.itemName, currentRecipe.resultAmount);
        DisplayRecipe(currentRecipe);
    }
}
