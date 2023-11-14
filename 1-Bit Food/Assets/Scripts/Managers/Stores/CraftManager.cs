using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftManager : StoreManager {
    private Recipe[] recipes;

    private Recipe selectedRecipe;

    protected override void Start() {
        base.Start();

        recipes = Resources.LoadAll<Recipe>("Recipes");

        foreach (Recipe recipe in recipes)
        {
            GameObject button = Instantiate(buttonPrefab, foodContainer.transform);
            button.GetComponentInChildren<Image>().sprite = recipe.result.sprite;
            button.GetComponentInChildren<TextMeshProUGUI>().text = recipe.result.name;

            button.GetComponent<Button>().onClick.AddListener(() => Craft(recipe));
        }

        
    }

    private void Craft(Recipe recipe)
    {
        if (popUp.activeSelf) return;

        string ingredientList = "";
        FoodList foodList = FoodList.GetInstance();

        for (int i = 0; i < recipe.ingredientQuantities.Count; i++)
        {
            Ingredient ingredient = recipe.ingredients[i];
            int have = GameManager.instance.GetFoodUses(foodList.GetFoodIndex(ingredient));
            int need = recipe.ingredientQuantities[i];

            if (have < need) return;

            ingredientList += ", " + need + " " + ingredient.name;
        }

        ingredientList = ingredientList.TrimStart(',');

        AudioManager.instance.PlayUIClip(0);

        selectedFood = recipe.result;
        selectedRecipe = recipe;

        confirmationMessage.text = $"Craft {selectedFood.name} with{ingredientList}?";

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