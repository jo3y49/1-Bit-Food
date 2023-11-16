public class EnemyAction : CharacterAction {
    public delegate void AttackDelegate(CharacterBattle self, CharacterBattle target, Food food = null);

    public AttackDelegate Attack { get; private set; }

    public EnemyAction(string name, AttackDelegate attack) : base(name)
    {
        Attack = attack;
    }
}