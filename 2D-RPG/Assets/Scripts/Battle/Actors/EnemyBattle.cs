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
        // worldManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WorldManager>();
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
}