using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBattle : CharacterBattle {
    protected WorldManager worldManager;
    protected GameObject player;
    protected bool isAttacking;

    public Flavor favoriteFlavor;

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player");
        worldManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WorldManager>();

        CharacterName = "Food Thief";

        actions = new List<FoodAction>{FoodList.GetInstance().GetEnemyAction()};
    }

    public override void Attacked(int damage, Flavor flavor = null)
    {
        if (favoriteFlavor == flavor) damage *= 2;

        base.Attacked(damage);
    }

    public override void PrepareCombat()
    {
        GetComponent<Collider2D>().enabled = false;
    }

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

    public int GetLoot()
    {
        int loot = Random.Range(20,40);

        return loot;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            worldManager.EncounterEnemy(gameObject);
        }
    }

    public virtual FoodAction PickEnemyAttack() { return GetAction(0); }
}