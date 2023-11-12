using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class WorldManager : MonoBehaviour {
    [SerializeField] protected GameObject battleUI;
    // [SerializeField] protected DialogManager dialogManager;
    protected GameObject player;
    protected PlayerBattle playerBattle;
    protected PlayerMovement playerMovement;
    protected List<EnemyBattle> enemies = new();

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

    public virtual void EncounterEnemy(GameObject enemy)
    {
        enemies.Add(enemy.GetComponent<EnemyBattle>());
        StartBattle();
    }

    public virtual void WinBattle(){}

    public virtual void LoseBattle()
    {
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