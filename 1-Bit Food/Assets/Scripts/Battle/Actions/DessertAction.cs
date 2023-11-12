using UnityEngine;

public class DessertAction
{
    public delegate void AttackDelegate(CharacterBattle self, CharacterBattle target);
    public delegate void HealDelegate(CharacterBattle self);

    public string Name { get; private set; }
    public AttackDelegate Attack { get; private set; }
    public HealDelegate Heal { get; private set; }

    public DessertAction(string name, AttackDelegate attack, HealDelegate heal)
    {
        Name = name;
        Attack = attack;
        Heal = heal;
    }

    public static void DoAttack(CharacterBattle self, CharacterBattle target, string attackName, int damage = 1)
    {
        DoAction(self, attackName);

        target.SetAnimationTrigger("Attacked");

        target.Attacked(damage);
    }

    public static void DoHeal(CharacterBattle self, string attackName, int heal = 1)
    {
        DoAction(self, attackName);

        self.Heal(heal);
    }

    private static void DoAction(CharacterBattle self, string attackName)
    {
        self.AttackTrigger(attackName);
    }
}
