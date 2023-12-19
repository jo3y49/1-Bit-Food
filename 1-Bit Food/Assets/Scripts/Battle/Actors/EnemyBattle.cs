using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBattle : CharacterBattle {

    [SerializeField] private EnemyHealthDisplay healthDisplay;
    [SerializeField] private EnemyItemDisplay itemDisplay;

    public int minDamage, maxDamage;
    public int maxSteals;

    [Range(0,1)]
    public float stealChance = .2f;
    
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

        // healthDisplay.SetHealth(health);
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
        itemDisplay.ClearItems();
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
            if (recentSteals.Count > 0)
            {
                string text = $"{CharacterName} stole ";

                foreach (Food food in recentSteals)
                {
                    text += $"{food.name}, ";
                }

                text = text.TrimEnd();
                text = text.TrimEnd(',');

                return text;
            } else 
            {
                return CharacterName + " tried to steal, but you've got nothing!";
            }

        }

        else return "";
    }

    public override void StealItem(Food food)
    {
        if (food == null) return;

        recentSteals.Add(food);
        stolenFood.Add(food);

        itemDisplay.SetItem(food);
    }

    public List<Food> GetRecentlyStolenItems()
    {
        return recentSteals;
    }

    public override Food StolenItem(Food food)
    {
        stolenFood.Remove(food);

        itemDisplay.RemoveItem(food);

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

    public virtual CharacterAction PickEnemyAttack() 
    { 
        if (stolenFood.Count < maxSteals) return StealList.GetInstance().GetRandomAction(stealChance);

        else return StealList.GetInstance().GetAction("Attack");
    }
}