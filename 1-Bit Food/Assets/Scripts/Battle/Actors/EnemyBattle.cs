using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBattle : CharacterBattle {
    protected WorldManager worldManager;
    protected GameObject player;
    protected bool isAttacking;

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player");
        worldManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WorldManager>();

        CharacterName = "Thief Guy";

        attackKeys.Add("steal");

        attackActions = FillAttackList(attackKeys);

        attackActionUses.Add(10);
    }

    // public override void PrepareCombat()
    // {
    //     SetStats();
    // }

    public void ResetFromFight()
    {
        gameObject.SetActive(true);
    }

    protected virtual void SetStats(){}

    public override void Kill()
    {
        Destroy(gameObject);
    }

    public override void Defeated()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            worldManager.EncounterEnemy(gameObject);
        }
    }

    public virtual AttackAction PickEnemyAttack() { return GetAction(0); }
}