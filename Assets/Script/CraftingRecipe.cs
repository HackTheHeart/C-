using System.Collections.Generic;

[System.Serializable]
public class CraftingRecipe
{
    public ItemData resultItem;
    public int resultAmount = 1;
    [System.Serializable]
    public class Ingredient
    {
        public ItemData item;
        public int amount;
    }
    public List<Ingredient> ingredients = new List<Ingredient>();
}
