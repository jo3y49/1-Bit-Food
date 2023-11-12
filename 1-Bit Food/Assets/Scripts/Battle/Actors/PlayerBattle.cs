using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerBattle : CharacterBattle {
    private int experience = 0;
    public int level {get; private set;}

    protected override void Start() {
        base.Start();

        CharacterName = "Food Fighter Guy";

        attackActions = DessertList.GetInstance().GetAllActions();

        attackActionUses = Enumerable.Repeat(10, attackActions.Count).ToList();
        
    }

    public override void PrepareCombat()
    {
        GetComponent<PlayerMovement>().enabled = false;
    }

    public void EndCombat()
    {
        GetComponent<PlayerMovement>().enabled = true;
    }
    public void SetData(int level, int experience)
    {
        this.level = level;
        this.experience = experience;
        
        // SetStats(level);
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }
}