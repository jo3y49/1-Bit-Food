using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class BattleManager : MonoBehaviour {
    [Header("Managers")]
    [SerializeField] private WorldManager worldManager;
    [SerializeField] private BattleUIManager battleUIManager;

    [Header("Other Variables")]
    public float dialogueDisplayTime = 1.5f;

    [Header("Extras, no need to change")]
    public List<EnemyBattle> enemies = new();
    private PlayerBattle player;
    private Queue<CharacterBattle> turnOrder = new();
    public CharacterBattle characterToAttack;
    public AttackAction activeCharacterAttack;
    private Coroutine battleLoop;

    // tracker variables
    public int killCount = 0;
    public bool awaitCommand = false;

    private void Awake() {
        gameObject.SetActive(false);
    }
    
    private void OnEnable() 
    {
        // Setup Characters
        InitializeCharacters();

        // Setup UI
        SetUpUI();

        // start the battle loop
        battleLoop = StartCoroutine(BattleLoop());
    }

    private void InitializeCharacters()
    {
        // set enemies, end battle if none
        enemies = worldManager.GetBattleEnemies();
        if (enemies.Count <= 0) EndBattle();

        // save player reference
        player = worldManager.GetPlayer();

        // Set up the turn order
        IEnumerable<CharacterBattle> characters = new List<CharacterBattle>{ player }.Concat(enemies);
        turnOrder = new Queue<CharacterBattle>(characters);
    }

    private void SetUpUI()
    {
        battleUIManager.SetForBattle(player, enemies);
    }

    private IEnumerator BattleLoop()
    {
        while (turnOrder.Count > 0) {
            // get next character in order
            CharacterBattle activeCharacter = turnOrder.Peek();

            if (activeCharacter == player)
            {
                // prepare variables for the players turn
                InitializePlayer();

                // activate ui
                battleUIManager.ActivateForPlayerTurn();
                
                // make the loop stay here until the player selects their commands in the ui
                while (awaitCommand)
                {

                    yield return null;
                }

                // standard action
                // do the attack and save whether it hit or not
                bool hit = Attack(activeCharacter, activeCharacterAttack);
                    
                // wait for the attack animation to play
                while (activeCharacter.GetIsAttacking())
                {
                    
                    yield return null;
                }

                // attacked character resets
                characterToAttack.Recover();

                // handles a miss
                if (!hit)
                {
                    battleUIManager.SetText($"{activeCharacter.CharacterName} missed");
                    break;
                }

                // update enemy's health ui
                battleUIManager.UpdateHealth();

                // handle enemy's death
                if (characterToAttack.health <= 0)
                {
                    DefeatedEnemy();

                    // break out of combo when enemy dies
                    break;
                }
            } else {
                // set enemy combo
                activeCharacterAttack = (activeCharacter as EnemyBattle).PickEnemyAttack();

                // enemy targets the player
                characterToAttack = player;

                // do the attack and save whether it hit or not
                bool hit = Attack(activeCharacter, activeCharacterAttack);
                    
                // wait for the attack animation to play
                while (activeCharacter.GetIsAttacking())
                {
                    
                    yield return null;
                }

                // attacked character resets
                characterToAttack.Recover();

                // handles a miss
                if (!hit)
                {
                    battleUIManager.SetText($"{activeCharacter.CharacterName} missed");
                    break;
                }

                // updates player's health
                battleUIManager.UpdateHealth();

                // lose battle if player dies
                if (characterToAttack.health <= 0)
                {
                    StartCoroutine(LoseBattle());

                    break;
                }
            }

            // creates a pause between turns
            yield return new WaitForSeconds(1f);

            NextTurn(activeCharacter);
        }
    }

    private void InitializePlayer()
    {
        // Prepares player for turn
        awaitCommand = true;
    }

    private void NextTurn(CharacterBattle activeCharacter)
    {
        // moves character that just acted to the back
        turnOrder.Dequeue();
        turnOrder.Enqueue(activeCharacter);

        // reset necessary variables
        activeCharacterAttack = null;
    }

    public void Escape()
    {
        StopCoroutine(battleLoop);
        worldManager.EscapeBattle();
        EndBattle();
    }

    private bool Attack(CharacterBattle activeCharacter, AttackAction comboAction)
    {
        battleUIManager.SetText($"{activeCharacter.CharacterName} used {comboAction.Name} at {characterToAttack.CharacterName}");

        return activeCharacter.DoAction(comboAction, characterToAttack);
    }

    private void DefeatedEnemy()
    {
        // convert target into the EnemyInfo type
        EnemyBattle defeatedEnemy = characterToAttack as EnemyBattle;

        // take enemy out of battle system
        var newTurnOrder = new Queue<CharacterBattle>(turnOrder.Where(x => x != defeatedEnemy));
        turnOrder = newTurnOrder;

        // gain stats from kill
        killCount++;

        // remove enemy from ui
        battleUIManager.DefeatedEnemy(defeatedEnemy);
        defeatedEnemy.Defeated();

        // end battle if it was the last enemy
        if (turnOrder.Count <= 1)
            StartCoroutine(WinBattle());
    }

    private void GetEnemyAttack(EnemyBattle enemy)
    {
        activeCharacterAttack = enemy.PickEnemyAttack();
    }

    public void SetComboAction(CharacterBattle characterToAttack, AttackAction activeCharacterCombo)
    {
        this.characterToAttack = characterToAttack;
        this.activeCharacterAttack = activeCharacterCombo;
        awaitCommand = false;
    }

    private void EndBattle()
    {
        player.EndCombat();

        StopAllCoroutines();

        enemies.Clear();

        killCount = 0;
        activeCharacterAttack = null;

        battleUIManager.ClearUI();

        gameObject.SetActive(false);
    }

    private IEnumerator WinBattle()
    {
        StopCoroutine(battleLoop);

        battleUIManager.SetText("You won!");

        int currentLevel = player.level;

        yield return new WaitForSeconds(dialogueDisplayTime);

        if (player.level > currentLevel)
        {
            battleUIManager.SetText("Level up! You are now level " + player.level);
            yield return new WaitForSeconds(dialogueDisplayTime);
        }

        foreach (EnemyBattle e in enemies)
        {
            e.Kill();
        }

        worldManager.WinBattle();

        EndBattle();
    }

    private IEnumerator LoseBattle()
    {
        StopCoroutine(battleLoop);

        battleUIManager.SetText("You were defeated!");

        yield return new WaitForSeconds(dialogueDisplayTime);

        worldManager.LoseBattle();
    }
}