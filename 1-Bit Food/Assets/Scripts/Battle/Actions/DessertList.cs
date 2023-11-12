using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DessertList
{
    private static DessertList instance;
    private IDictionary<string, DessertAction> dessertList;

    private DessertList()
    {
        FillDictionary();
    }

    public static DessertList GetInstance()
    {
        instance ??= new DessertList();

        return instance;
    }

    public DessertAction GetAction(string key)
    {
        if (dessertList.ContainsKey(key)) return dessertList[key];

        else return new DessertAction("Null", EmptyAction, (c) => EmptyAction());
    }

    public DessertAction GetAction(int index)
    {
        if (dessertList.Count > index) return dessertList.ElementAt(index).Value;

        else return new DessertAction("Null", EmptyAction, (c) => EmptyAction());
    }

    public List<DessertAction> GetAllActions()
    {
        return dessertList.Values.ToList();
    }


    private void FillDictionary()
    {
        Dessert[] desserts = Resources.LoadAll<Dessert>("Desserts");

        dessertList = new Dictionary<string, DessertAction>();

        foreach (Dessert dessert in desserts)
        {
            dessertList.Add(dessert.name, new DessertAction(dessert.name, 
                (self, target) => DessertAction.DoAttack(self, target, dessert.name, dessert.damage),
                (self) => DessertAction.DoHeal(self, dessert.name, dessert.heal)));
        }



        // dessertList = new Dictionary<string, DessertAction>();
        // // {
        // //     {"cake", new DessertAction("Cake", Cake, Cake)},
        // //     // {"pie", new DessertAction("Pie", Pie)},
            
        // // };

        // foreach (string dessert in dessertNames)
        // {
        //     dessertList.Add(dessert, new DessertAction(dessert, 
        //     (self, target) => DessertAction.DoAttack(self, target, dessert), (self) => DessertAction.DoHeal(self, dessert)));
        // }
    }

    public void EmptyAction(CharacterBattle self = null, CharacterBattle target = null)
    {
        Debug.Log("This action is null");
        
        string attackName = "null";
        float damage = 0;

        // DessertAction.DoAttack(self, target, attackName, damage);
    }
}