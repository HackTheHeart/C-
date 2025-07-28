using UnityEngine;
using UnityEngine.UI;

public class CraftingRecipeButton : MonoBehaviour
{
    private CraftingRecipe recipe;
    private CraftingUI craftingUI;

    public Image iconImage;
    public Button button;

    public void Setup(CraftingRecipe recipe, CraftingUI ui)
    {
        this.recipe = recipe;
        this.craftingUI = ui;

        if (iconImage != null)
        {
            iconImage.sprite = recipe.resultItem.itemSprite;
        }

        // Đảm bảo nút có component Button
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);

        Debug.Log("Setup xong button cho: " + recipe.resultItem.itemName);
    }

    void OnClick()
    {
        Debug.Log("Click vào recipe: " + recipe.resultItem.itemName);
        craftingUI.DisplayRecipe(recipe);
    }
}
