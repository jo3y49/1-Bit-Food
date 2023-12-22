public class BakeManager : StoreManager {
    private Recipe selectedRecipe;

    public void Craft(Recipe recipe)
    {
        for (int i = 0; i < recipe.ingredientQuantities.Count; i++)
        {
            Food food = recipe.ingredients[i];
            int have = GameManager.instance.GetFoodUses(food.index);
            int need = recipe.ingredientQuantities[i];

            if (have < need) return;
        }

        AudioManager.instance.PlayUIClip(0);

        selectedFood = recipe.result;
        selectedRecipe = recipe;

        Confirm();

    }

    protected override void Transaction()
    {
        for (int i = 0; i < selectedRecipe.ingredientQuantities.Count; i++)
        {
            GameManager.instance.AddFoodUse(selectedRecipe.ingredients[i].index, -selectedRecipe.ingredientQuantities[i]);
            inventoryAmount -= selectedRecipe.ingredientQuantities[i];
        }
    }
}