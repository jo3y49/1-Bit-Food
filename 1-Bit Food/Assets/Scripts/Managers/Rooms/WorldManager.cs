using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour {
    [SerializeField] protected GameObject battleUI, shopUI, bakeUI;
    // [SerializeField] protected DialogManager dialogManager;
    protected GameObject player;
    protected PlayerBattle playerBattle;
    protected PlayerMovement playerMovement;
    public List<EnemyBattle> enemies = new();

    public float nearbyEncounterRange = 3f;

    public int coinReward;

    protected virtual void Start() {

        player = GameObject.FindGameObjectWithTag("Player");
        GameManager.instance.SetPlayer(player);
        playerBattle = player.GetComponent<PlayerBattle>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    public virtual List<EnemyBattle> GetBattleEnemies() 
    {
        return enemies;
    }

    public virtual void EncounterEnemy()
    {
        if (battleUI.activeSelf) return;

        foreach (EnemyBattle e in enemies)
        {
            
            // enemies.Add(e);
            e.PrepareCombat();
            
                
        }

        // enemies.Reverse();

        StartBattle();
    }

    public virtual void WinBattle()
    {
        if (coinReward < 0) SceneManager.LoadScene(5);


        GameManager.instance.AddPlayerMoney(coinReward);
    }

    public virtual void LoseBattle()
    {
        playerBattle.ResetHealth();
        SceneManager.LoadScene(0);
    }

    public virtual void EscapeBattle(){}

    // protected virtual IEnumerator DoDialog(DialogObject dialogObject)
    // {
    //     playerMovement.enabled = false;

    //     dialogManager.enabled = true;
        
    //     // Start the next dialog.
    //     dialogManager.DisplayDialog(dialogObject);

    //     // Wait for this dialog to finish before proceeding to the next one.
    //     yield return new WaitUntil(() => !dialogManager.ShowingDialog());
        
    //     dialogManager.enabled = false;

    //     playerMovement.enabled = true;
    // }

    protected virtual void StartBattle() 
    {
        playerBattle.PrepareCombat();
        battleUI.SetActive(true);
    }

    public PlayerBattle GetPlayer() { return playerBattle; }
}