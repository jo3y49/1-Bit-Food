using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerBattle : CharacterBattle {    

    protected List<int> actionUses = new();

    protected List<FoodAction> actions = new();

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

    public Food StealRandomItem()
    {
        int r = Random.Range(0, actions.Count);

        for (int i = 0; i < actions.Count; i++)
        {
            if (actionUses[r] > 0)
            {
                actionUses[r]--;
                return FoodList.GetInstance().GetFoods()[r];
                
            } else {
                if (r < actions.Count)
                    r++;
                else
                    r = 0;
            }
        }

        return null;
    }

    public bool OutOfDessert()
    {
        return actionUses.Max() <= 0;
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }

    public virtual FoodAction GetAction(int i)
    {
        if (i < actions.Count) return actions[i];

        else return actions[0];
    }

    public virtual int GetActionUses(int i)
    {
        if (i < actionUses.Count && i < actions.Count) return actionUses[i];

        else return 0;
    }

    public virtual int GetActionUses(FoodAction action)
    {
        int i = actions.FindIndex(item => item == action);

        return GetActionUses(i);
    }

    public virtual bool CanUseAction(FoodAction attackAction)
    {
        return GetActionUses(attackAction) > 0;
    }

    public override string DoAttack(Actions action, CharacterBattle target, Flavor flavor = null)
    {
        UseAction(action);

        (action as FoodAction).Attack(this, target, flavor);

        return "";
    }

    public virtual void DoHeal(Actions action)
    {
        UseAction(action);

        (action as FoodAction).Heal(this);
    }

    protected void UseAction(Actions action)
    {
        int i = actions.FindIndex(item => item == action);

        if (i != -1 && actionUses.Count > i) actionUses[i]--;
    }
    

    public virtual int CountActions()
    {
        return actions.Count;
    }
}