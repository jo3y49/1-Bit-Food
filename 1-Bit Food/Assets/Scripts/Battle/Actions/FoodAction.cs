public class FoodAction : Actions
{
    public delegate void AttackDelegate(CharacterBattle self, CharacterBattle target, Flavor flavor);
    public delegate void HealDelegate(CharacterBattle self);

    public AttackDelegate Attack { get; private set; }
    public HealDelegate Heal { get; private set; }

    public FoodAction(string name, AttackDelegate attack, HealDelegate heal) : base(name)
    {
        Attack = attack;
        Heal = heal;
    }
}
