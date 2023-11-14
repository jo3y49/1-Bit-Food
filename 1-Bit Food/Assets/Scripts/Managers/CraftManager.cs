using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour {
    private Recipe[] recipes;

    private void Awake() {
        recipes = Resources.LoadAll<Recipe>("Recipes");
    }
}