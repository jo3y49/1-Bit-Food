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

    public List<Food> stolenFood = new();

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player");
        worldManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WorldManager>();

        CharacterName = "Food Thief";
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

    public override string DoAttack(CharacterAction action, CharacterBattle target, Flavor flavor = null) 
    {
        (action as EnemyAction).Attack(this, target);

        if (action.Name == "Steal") return $"{CharacterName} Stole {GetRecentlyStolenItem().name}!";

        else return "";
    }

    public override void TakeItem(Food food)
    {
        stolenFood.Add(food);
    }

    public Food GetRecentlyStolenItem()
    {
        return stolenFood.Last();
    }

    public override Food StealItem(Food food)
    {
        stolenFood.Remove(food);

        return food;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            worldManager.EncounterEnemy(gameObject);
        }
    }

    public virtual CharacterAction PickEnemyAttack() { return StealList.GetInstance().GetAction(); }
}