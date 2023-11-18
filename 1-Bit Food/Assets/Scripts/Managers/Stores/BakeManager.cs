using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BakeManager : StoreManager {
    private Recipe[] recipes;

    private Recipe selectedRecipe;

    protected override void OnEnable() {
        base.OnEnable();

        recipes = Resources.LoadAll<Recipe>("Recipes");
    }

    public void Craft(Recipe recipe)
    {
        if (!GameManager.instance.OpenInventory()) return;

        FoodList foodList = FoodList.GetInstance();

        for (int i = 0; i < recipe.ingredientQuantities.Count; i++)
        {
            Food food = recipe.ingredients[i];
            int have = GameManager.instance.GetFoodUses(foodList.GetFoodIndex(food));
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
        FoodList foodList = FoodList.GetInstance();

        for (int i = 0; i < selectedRecipe.ingredientQuantities.Count; i++)
        {
            GameManager.instance.AddFoodUse(foodList.GetFoodIndex(selectedRecipe.ingredients[i]), -selectedRecipe.ingredientQuantities[i]);
        }
    }
}