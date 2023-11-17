using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FoodList
{
    private static FoodList instance;
    private Food[] foods;
    private IDictionary<string, PlayerAction> foodList;

    private FoodList()
    {
        FillDictionary();
    }

    public static FoodList GetInstance()
    {
        instance ??= new FoodList();

        return instance;
    }

    private void FillDictionary()
    {
        Ingredient[] ingredients = Resources.LoadAll<Ingredient>("Ingredients");
        Dessert[] desserts = Resources.LoadAll<Dessert>("Desserts");
        
        Array.Sort(ingredients, (x, y) => {
            return x.index.CompareTo(y.index);
        });

        Array.Sort(desserts, (x, y) => {
            return x.index.CompareTo(y.index);
        });

        foods = new Food[ingredients.Length + desserts.Length];
        Array.Copy(ingredients, foods, ingredients.Length);
        Array.Copy(desserts, 0, foods, ingredients.Length, desserts.Length);

        foodList = new Dictionary<string, PlayerAction>();

        foreach (Food food in foods)
        {
            foodList.Add(food.name, new PlayerAction(food.name, 
                (self, target, flavor) => CharacterAction.DoAttack(self, target, food.name, food.damage, flavor),
                (self, flavor) => CharacterAction.DoHeal(self, food.name, food.heal, flavor), food));
        }
    }

    public PlayerAction GetAction(string key)
    {
        if (foodList.ContainsKey(key)) return foodList[key];

        else return new PlayerAction("Null", EmptyAction, EmptyAction, null);
    }

    public PlayerAction GetAction(int index)
    {
        if (foodList.Count > index) return foodList.ElementAt(index).Value;

        else return new PlayerAction("Null", EmptyAction, EmptyAction, null);
    }

    public List<PlayerAction> GetAllActions()
    {
        return foodList.Values.ToList();
    }

    public Dessert[] GetDesserts()
    {
        return (Dessert[])foods.OfType<Dessert>();
    }

    public Ingredient[] GetIngredients()
    {
        return (Ingredient[])foods.OfType<Ingredient>();
    }

    public Food[] GetFoods()
    {
        return foods;
    }

    public int GetFoodIndex(Food food)
    {
        return foods.ToList().FindIndex(item => item == food);
    }

    public void EmptyAction(CharacterBattle self, CharacterBattle target, Flavor flavor = null)
    {
        Debug.Log("This action is null");
    }

    public void EmptyAction(CharacterBattle self, Flavor flavor = null)
    {
        Debug.Log("This action is null");
    }
}