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

    protected virtual void Start() {
        health = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void Kill(){}

    public virtual void Defeated(){}

    public virtual void PrepareCombat(){}
    public virtual string DoAttack(CharacterAction action, CharacterBattle target, Flavor flavor = null) {return "";}
    public virtual void PlayAttackSound()
    {
        if (hitTarget)
        {
            audioSource.clip = attackClip;
            audioSource.time = 2f;
            audioSource.Play();
        }
    }

    public virtual void Attacked(int damage, Flavor flavor = null)
    {
        health -= damage;
        if (health < 0) health = 0;
    }

    public virtual void TakeItem(Food food) {}

    public virtual Food StealItem(Food food) { return null;}

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