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

    private void Craft(Recipe recipe)
    {
        if (popUp.activeSelf || !GameManager.instance.OpenInventory()) return;

        string ingredientList = "";
        string playerIngredientList = "";
        FoodList foodList = FoodList.GetInstance();

        for (int i = 0; i < recipe.ingredientQuantities.Count; i++)
        {
            Food food = recipe.ingredients[i];
            int have = GameManager.instance.GetFoodUses(foodList.GetFoodIndex(food));
            int need = recipe.ingredientQuantities[i];

            if (have < need) return;

            ingredientList += ", " + need + " " + food.name;
            playerIngredientList += ", " + have + " " + food.name;
        }

        ingredientList = ingredientList.TrimStart(',');
        playerIngredientList = playerIngredientList.TrimStart(',');

        AudioManager.instance.PlayUIClip(0);

        selectedFood = recipe.result;
        selectedRecipe = recipe;

        confirmationMessage.text = $"Bake a {selectedFood.name} with{ingredientList}? You have{playerIngredientList}";

        popUp.SetActive(true);
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