using UnityEngine;
using System.Collections.Generic;

public class PlayerBattle : CharacterBattle {
    private int experience = 0;
    public int level {get; private set;}

    protected override void Start() {
        base.Start();

        CharacterName = "Leoh";

        attackKeys.Add("physical");
        attackKeys.Add("ranged");
        attackKeys.Add("heal");

        attackActions.Add(AttackList.GetInstance().GetAction(attackKeys[0]));
        attackActions.Add(AttackList.GetInstance().GetAction(attackKeys[1]));
        attackActions.Add(AttackList.GetInstance().GetAction(attackKeys[2]));
    }

    public override void PrepareCombat()
    {
        GetComponent<PlayerMovement>().enabled = false;
    }

    private void LevelUp(int xpForLevel)
    {
        experience -= xpForLevel;
        level++;

        SetStats(level);

        GameManager.instance.SetPlayerLevel(level);
    }

    public void SetStats(int level)
    {
        maxHealth = 20 + level * 5;
        attack = 9 + (int)(level * 1.5f);
        defense = 5 + (int)(level * 1.1f);
        accuracy = .85f + level / 2000f;
        evasion = .05f + level / 1000f;

        ResetHealth();
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