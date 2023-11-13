using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class CharacterBattle : MonoBehaviour {
    [SerializeField] protected CharacterAnimation characterAnimation;
    [SerializeField] protected SpriteRenderer spriteRenderer; 
    public string CharacterName { get; protected set; }
    public int maxHealth;
    public int health;
    public bool hitTarget = false;
    protected AudioSource audioSource;
    [SerializeField] protected AudioClip attackClip;

    protected List<string> attackKeys = new();

    protected List<DessertAction> actions = new();

    protected List<int> actionUses = new();

    protected virtual void Start() {
        health = maxHealth;
        audioSource = GetComponent<AudioSource>();

        spriteRenderer.color = ColorSwitcher.instance.bright;
    }

    protected static List<DessertAction> FillAttackList(List<string> keys)
    {
        List<DessertAction> attackActions = new();
        DessertList attackList = DessertList.GetInstance();

        foreach (string k in keys)
        {
            attackActions.Add(attackList.GetAction(k));
        }

        return attackActions;
    }

    public virtual void Kill(){}

    public virtual void Defeated(){}

    public virtual void PrepareCombat(){}

    public virtual DessertAction GetAction(int i)
    {
        if (i < actions.Count) return actions[i];

        else return actions[0];
    }

    public virtual int GetActionUses(int i)
    {
        if (i < actionUses.Count && i < actions.Count) return actionUses[i];

        else return 0;
    }

    public virtual int GetActionUses(DessertAction action)
    {
        int i = actions.FindIndex(item => item == action);

        return GetActionUses(i);
    }

    public virtual bool CanUseAction(DessertAction attackAction)
    {
        return GetActionUses(attackAction) > 0;
    }

    protected virtual void UseAction(DessertAction action)
    {
        int i = actions.FindIndex(item => item == action);

        if (i != -1 && actionUses.Count > i) actionUses[i]--;
    }

    public virtual int CountActions()
    {
        return actions.Count;
    }

    public virtual void DoAttack(DessertAction action, CharacterBattle target)
    {
        UseAction(action);

        action.Attack(this, target);
    }

    public virtual void DoHeal(DessertAction action)
    {
        UseAction(action);

        action.Heal(this);
    }

    public virtual void PlayAttackSound()
    {
        if (hitTarget)
        {
            audioSource.clip = attackClip;
            audioSource.time = 2f;
            audioSource.Play();
        }
    }

    public virtual void Attacked(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
    }

    public virtual void Heal(int heal)
    {
        health += heal;
        if (health > maxHealth) health = maxHealth;
    }

    public virtual void SetAnimationTrigger(string triggerName) { characterAnimation.AnimationTrigger(triggerName); }

    public virtual void SetAnimationBool(string triggerName, bool b) { characterAnimation.AnimationSetBool(triggerName, b); }

    public virtual void AttackTrigger(string triggerName) { characterAnimation.AttackTrigger(triggerName); }

    public virtual Animator GetAnimator() {return characterAnimation.GetAnimator();}

    public virtual bool GetIsActing() {return characterAnimation.isAttacking;}

    public virtual void Recover() {}
}