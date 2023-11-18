using UnityEngine;

public abstract class CharacterAction {

    public string Name { get; protected set; }

    public CharacterAction(string name)
    {
        Name = name;
    }

    public static void DoAttack(CharacterBattle self, CharacterBattle target, string attackName, int damage = 1, Flavor flavor = null)
    {
        DoAction(self, attackName);

        target.SetAnimationTrigger("Attacked");

        if (flavor != null) damage += flavor.bonus;

        target.Attacked(damage, flavor);
    }

    public static void DoAttackRandom(CharacterBattle self, CharacterBattle target, string attackName, int low, int high, Flavor flavor = null)
    {
        int damage = Random.Range(low, high + 1);

        DoAttack(self, target, attackName, damage, flavor);
    }

    public static void DoHeal(CharacterBattle self, string attackName, int heal = 1, Flavor flavor = null)
    {
        DoAction(self, attackName);

        if (flavor != null) heal += flavor.bonus;

        self.Heal(heal);
    }

    private static void DoAction(CharacterBattle self, string attackName)
    {
        self.AttackTrigger(attackName);
    }
    
}