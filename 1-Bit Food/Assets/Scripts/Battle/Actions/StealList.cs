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

    public EnemyAction GetRandomAction()
    {
        int r = Random.Range(0,10);

        if (r < 2) return actionList["Steal"];

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
            {"Steal", new EnemyAction("Steal", Steal)},
            {"Attack",new EnemyAction("Attack", 
            (self, target, food) => CharacterAction.DoAttackRandom(self, target, "Attack", self.minDamage, self.maxDamage))},
        };
    }

    private void Steal(EnemyBattle self, PlayerBattle target, Food food = null)
    {
        int s = Random.Range(0, self.steals);

        for (int i = 0; i <= s; i++)
            self.TakeItem(target.StealItem(food));
    }

    public void EmptyAction(CharacterBattle self, CharacterBattle target)
    {
        Debug.Log("This action is null");
    }
}