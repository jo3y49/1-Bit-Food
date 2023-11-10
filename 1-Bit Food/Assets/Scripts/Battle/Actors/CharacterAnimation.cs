using System.Collections;
using UnityEngine;

public abstract class CharacterAnimation : MonoBehaviour {
    [SerializeField] protected Animator anim;

    public string currentTrigger = "";
    protected bool isFighting = false;
    public bool isAttacking = false;

    public virtual void AttackTrigger(string triggerName)
    {
        if (Utility.CheckIfAnimationParamExists("Attack", anim))
        {
            StartCoroutine(AttackDuration());
            anim.SetTrigger("Attack");

            if (Utility.CheckIfAnimationParamExists(triggerName, anim))
            {
                currentTrigger = triggerName;
                anim.SetTrigger(currentTrigger);
            }
        }
    }

    public virtual void AnimationTrigger(string triggerName)
    {
        if (Utility.CheckIfAnimationParamExists(triggerName, anim))
        {
            currentTrigger = triggerName;
            anim.SetTrigger(currentTrigger);
        }
    }

    public virtual void AnimationSetBool(string triggerName, bool b)
    {
        if (Utility.CheckIfAnimationParamExists(triggerName, anim))
            anim.SetBool(triggerName, b);
    }

    private IEnumerator AttackDuration()
    {
        isAttacking = true;

        yield return new WaitForSeconds(1);

        isAttacking = false;
    }

    public virtual Animator GetAnimator() {return anim;}
}