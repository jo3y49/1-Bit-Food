using UnityEngine;

public class AttackAction
{
    public delegate bool ActionDelegate(CharacterBattle self, CharacterBattle target);

    public string Name { get; private set; }
    public ActionDelegate Action { get; private set; }

    public AttackAction(string name, ActionDelegate action)
    {
        Name = name;
        Action = action;
    }

    public static bool DoAttack(CharacterBattle self, CharacterBattle target, string attackName, float accuracy, float damageMultiplier)
    {
        self.AttackTrigger(attackName);

        bool hitAttack = AttackCheck(self, target, accuracy);

        if (hitAttack) HitTarget(self, target, damageMultiplier);

        return hitAttack;
    }

    private static bool AttackCheck(CharacterBattle self, CharacterBattle target, float moveAccuracy)
    {
        float r = Random.Range(0,1f);
        float a  = (moveAccuracy * self.accuracy) - target.evasion;
        self.hitTarget = r < a;

        return self.hitTarget;
    }

    private static void HitTarget(CharacterBattle self, CharacterBattle target, float d)
    {
        target.SetAnimationTrigger("Attacked");

        int damage = (int)(self.attack * d - target.defense);
        if (damage < 0) damage = 0;

        target.Attacked(damage);
    }

    public static bool DoHeal(CharacterBattle self, string attackName, float healMultipler)
    {
        self.AttackTrigger(attackName);

        self.Heal((int)(self.maxHealth/2 * healMultipler));

        return true;
    }
}
