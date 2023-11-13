using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerBattle : CharacterBattle {
    private int experience = 0;
    public int level {get; private set;}

    protected override void Start() {
        base.Start();

        CharacterName = "Player";

        actions = DessertList.GetInstance().GetAllActions();

        actionUses = Enumerable.Repeat(10, actions.Count).ToList();
        
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

    public bool OutOfDessert()
    {
        return actionUses.Max() <= 0;
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }
}