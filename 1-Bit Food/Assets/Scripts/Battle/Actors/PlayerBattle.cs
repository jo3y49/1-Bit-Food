using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerBattle : CharacterBattle {
    public int level {get; private set;}

    protected override void Start() {
        base.Start();

        CharacterName = "Player";

        actions = FoodList.GetInstance().GetAllActions();

        actionUses = GameManager.instance.GetFoodUses();
        
    }

    public override void PrepareCombat()
    {
        GetComponent<PlayerMovement>().enabled = false;
    }

    public void EndCombat(int money)
    {
        GetComponent<PlayerMovement>().enabled = true;

        GameManager.instance.AddPlayerMoney(money);
        GameManager.instance.SetFoodUses(actionUses);
    }

    public bool OutOfDessert()
    {
        return actionUses.Max() <= 0;
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }
}