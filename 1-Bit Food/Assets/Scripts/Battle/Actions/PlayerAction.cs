public class PlayerAction : CharacterAction
{
    public delegate void AttackDelegate(CharacterBattle self, CharacterBattle target, Flavor flavor);
    public delegate void HealDelegate(CharacterBattle self, Flavor flavor);

    public AttackDelegate Attack { get; private set; }
    public HealDelegate Heal { get; private set; }
    public Food Food { get; private set; }

    public PlayerAction(string name, AttackDelegate attack, HealDelegate heal, Food food) : base(name)
    {
        Attack = attack;
        Heal = heal;
        Food = food;
    }
}
