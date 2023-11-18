using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBattle : CharacterBattle {

    [SerializeField] private EnemyHealthDisplay healthDisplay;

    public int lowDamage, highDamage;

    public int steals;
    protected GameObject player;
    protected bool isAttacking;

    protected WorldManager worldManager;

    public List<Food> stolenFood = new();
    public List<Food> recentSteals = new();

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player");
        worldManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WorldManager>();

        CharacterName = "Thief";

        healthDisplay.SetHealth(health);
    }

    public override void PrepareCombat()
    {
        // GetComponent<Collider2D>().enabled = false;
    }

    public override void Attacked(int damage, Flavor flavor = null)
    {
        base.Attacked(damage, flavor);

        healthDisplay.SetHealth(health);
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

    // public int GetLoot()
    // {
    //     int loot = Random.Range(20,40);

    //     return loot;
    // }

    public override string DoAttack(CharacterAction action, CharacterBattle target, Flavor flavor = null) 
    {
        (action as EnemyAction).Attack(this, target as PlayerBattle);

        if (action.Name == "Steal") 
        {
            string text = $"{CharacterName} stole ";

            foreach (Food food in recentSteals)
            {
                text += $"{food.name}, ";
            }

            return text;
        }

        else return "";
    }

    public override void TakeItem(Food food)
    {
        recentSteals.Add(food);
        stolenFood.Add(food);
    }

    public List<Food> GetRecentlyStolenItems()
    {
        return recentSteals;
    }

    public override Food StealItem(Food food)
    {
        stolenFood.Remove(food);

        return food;
    }

    public void ClearRecentItems()
    {
        recentSteals.Clear();
    }

    // protected virtual void OnTriggerEnter2D(Collider2D other) 
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         worldManager.EncounterEnemy(gameObject);
    //     }
    // }

    public virtual CharacterAction PickEnemyAttack() { return StealList.GetInstance().GetRandomAction(); }
}