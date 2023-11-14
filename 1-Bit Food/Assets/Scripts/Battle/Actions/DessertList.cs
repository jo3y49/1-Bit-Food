using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DessertList
{
    private static DessertList instance;
    private Dessert[] desserts;
    private IDictionary<string, DessertAction> dessertList;
    private DessertAction enemyAction;

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

        else return new DessertAction("Null", EmptyAction, EmptyAction);
    }

    public DessertAction GetAction(int index)
    {
        if (dessertList.Count > index) return dessertList.ElementAt(index).Value;

        else return new DessertAction("Null", EmptyAction, EmptyAction);
    }

    public DessertAction GetEnemyAction()
    {
        return enemyAction;
    }

    public List<DessertAction> GetAllActions()
    {
        return dessertList.Values.ToList();
    }

    public Dessert[] GetDesserts()
    {
        return desserts;
    }

    public int GetDessertIndex(Dessert dessert)
    {
        return desserts.ToList().FindIndex(item => item == dessert);
    }


    private void FillDictionary()
    {
        desserts = Resources.LoadAll<Dessert>("Desserts");

        dessertList = new Dictionary<string, DessertAction>();

        foreach (Dessert dessert in desserts)
        {
            dessertList.Add(dessert.name, new DessertAction(dessert.name, 
                (self, target, flavor) => DessertAction.DoAttack(self, target, dessert.name, dessert.damage, flavor),
                (self) => DessertAction.DoHeal(self, dessert.name, dessert.heal)));
        }

        enemyAction = new DessertAction("Steal", 
            (self, target, flavor) => DessertAction.DoAttack(self, target, "Steal", 3), EmptyAction);

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

    public void EmptyAction(CharacterBattle self, CharacterBattle target, Flavor flavor = null)
    {
        Debug.Log("This action is null");
    }

    public void EmptyAction(CharacterBattle self = null)
    {
        Debug.Log("This action is null");
    }
}