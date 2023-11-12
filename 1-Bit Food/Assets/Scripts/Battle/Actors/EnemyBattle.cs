using System.Linq;
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

        attackActions = DessertList.GetInstance().GetAllActions();

        attackActionUses = Enumerable.Repeat(10, attackActions.Count).ToList();
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

    public virtual DessertAction PickEnemyAttack() { return GetAction(0); }
}