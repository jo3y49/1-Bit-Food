using System.Collections.Generic;
using UnityEngine;

public class AttackList
{
    private static AttackList instance;
    private IDictionary<string, AttackAction> actionList;

    private AttackList()
    {
        FillDictionary();
    }

    public static AttackList GetInstance()
    {
        instance ??= new AttackList();

        return instance;
    }

    public AttackAction GetAction(string key)
    {
        if (actionList.ContainsKey(key)) return actionList[key];

        else return new AttackAction("Null", EmptyAction);
    }


    private void FillDictionary()
    {
        actionList = new Dictionary<string, AttackAction>()
        {
            {"steal", new AttackAction("Steal", Steal)},
            {"cake", new AttackAction("Cake", Cake)},
            // {"pie", new AttackAction("Pie", Pie)},
            
        };
    }

    public bool EmptyAction(CharacterBattle self, CharacterBattle target)
    {
        Debug.Log("This action is null");
        
        string attackName = "null";
        float damageMultiplier = 0;
        float accuracy = 0;

        return AttackAction.DoAttack(self, target, attackName, damageMultiplier, accuracy);
    }

    // player attacks

    public bool Cake(CharacterBattle self, CharacterBattle target)
    {
        string attackName = "Cake";

        return AttackAction.DoAttack(self, target, attackName);
    }

    // enemy attacks

    public bool Steal(CharacterBattle self, CharacterBattle target)
    {
        string attackName = "Steal";

        return AttackAction.DoAttack(self, target, attackName);
    }
}