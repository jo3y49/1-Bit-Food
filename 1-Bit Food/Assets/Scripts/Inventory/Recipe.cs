using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe")]
public class Recipe : ScriptableObject {
    public Food result;
    public List<Ingredient> ingredients;

    [Header("Add amount of ingredients needed for recipe")]
    public List<int> ingredientQuantities;
}