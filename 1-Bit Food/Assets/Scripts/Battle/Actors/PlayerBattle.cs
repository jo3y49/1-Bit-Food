using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerBattle : CharacterBattle {
    public int level {get; private set;}

    protected override void Start() {
        base.Start();

        CharacterName = "Player";

        actions = DessertList.GetInstance().GetAllActions();

        actionUses = GameManager.instance.GetDessertUses();
        
    }

    public override void PrepareCombat()
    {
        GetComponent<PlayerMovement>().enabled = false;
    }

    public void EndCombat(int money)
    {
        GetComponent<PlayerMovement>().enabled = true;

        GameManager.instance.AddPlayerMoney(money);
        GameManager.instance.SetDessertUses(actionUses);
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