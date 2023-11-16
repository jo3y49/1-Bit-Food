using System.Collections.Generic;
using UnityEngine;

public class StealList {
    private static StealList instance;

    private Dictionary<string, EnemyAction> actionList;

    private StealList()
    {
        FillDictionary();
    }

    public static StealList GetInstance()
    {
        instance ??= new StealList();

        return instance;
    }

    public EnemyAction GetAction()
    {
        int r = Random.Range(0,10);

        if (r < 2) 
            return actionList["Steal"];

        else return actionList["Attack"];
    }

    public EnemyAction GetAction(string key)
    {
        return actionList[key];
    }

    private void FillDictionary()
    {
        actionList = new Dictionary<string, EnemyAction>()
        {
            {"Attack" ,new EnemyAction("Attack", 
            (self, target, food) => CharacterAction.DoAttack(self, target, "Attack", 3))},
            {"Steal", new EnemyAction("Steal", Steal)}

        };
    }

    private void Steal(CharacterBattle self, CharacterBattle target, Food food = null)
    {
        self.TakeItem(target.StealItem(food));
    }

    public void EmptyAction(CharacterBattle self, CharacterBattle target)
    {
        Debug.Log("This action is null");
    }
}