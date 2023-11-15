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

    private void FillDictionary()
    {
        actionList = new Dictionary<string, EnemyAction>()
        {
            {"Attack" ,new EnemyAction("Attack", 
            (self, target) => CharacterAction.DoAttack(self, target, "Attack", 3))},
            {"Steal", new EnemyAction("Steal", Steal)}

        };
    }

    private void Steal(CharacterBattle self, CharacterBattle target)
    {
        (self as EnemyBattle).TakeItem((target as PlayerBattle).StealRandomItem());
    }

    public void EmptyAction(CharacterBattle self, CharacterBattle target)
    {
        Debug.Log("This action is null");
    }
}