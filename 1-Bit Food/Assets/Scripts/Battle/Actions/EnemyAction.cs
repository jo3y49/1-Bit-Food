public class EnemyAction : CharacterAction {
    public delegate void AttackDelegate(CharacterBattle self, CharacterBattle target);

    public AttackDelegate Attack { get; private set; }

    public EnemyAction(string name, AttackDelegate attack) : base(name)
    {
        Attack = attack;
    }
}