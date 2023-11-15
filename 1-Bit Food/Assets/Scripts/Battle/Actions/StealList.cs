using System.Collections.Generic;
using UnityEngine;

public class StealList {
    private static StealList instance;

    private Dictionary<string, StealAction> actionList;

    private StealList()
    {
        FillDictionary();
    }

    public static StealList GetInstance()
    {
        instance ??= new StealList();

        return instance;
    }

    public StealAction GetAction()
    {
        int r = Random.Range(0,10);

        if (r < 2) 
            return actionList["Steal"];

        else return actionList["Attack"];
    }

    private void FillDictionary()
    {
        actionList = new Dictionary<string, StealAction>()
        {
            {"Attack" ,new StealAction("Attack", 
            (self, target) => Actions.DoAttack(self, target, "Attack", 3))},
            {"Steal", new StealAction("Steal", Steal)}

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