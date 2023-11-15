public class StealAction : Actions {
    public delegate void AttackDelegate(CharacterBattle self, CharacterBattle target);

    public AttackDelegate Attack { get; private set; }

    public StealAction(string name, AttackDelegate attack) : base(name)
    {
        Attack = attack;
    }
}