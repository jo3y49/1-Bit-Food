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
    public DessertAction activeCharacterAttack;
    private Coroutine battleLoop;

    // tracker variables
    public int moneyCount = 0;
    public bool awaitCommand = false;
    public bool attack = true;
    public Flavor flavor;

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
                // do the attack or heal
                if (attack)
                    PlayerAttack(activeCharacter, activeCharacterAttack, flavor);
                else 
                    Heal(activeCharacter, activeCharacterAttack);

                // wait for the animation to play
                while (activeCharacter.GetIsActing())
                {
                    
                    yield return null;
                }

                // attacked character resets
                characterToAttack.Recover();

                // update enemy's health ui
                battleUIManager.UpdateHealth();

                // handle enemy's death
                if (characterToAttack.health <= 0)
                {
                    DefeatedEnemy();
                }
            } else {
                // set enemy combo
                activeCharacterAttack = (activeCharacter as EnemyBattle).PickEnemyAttack();

                // enemy targets the player
                characterToAttack = player;

                // do the attack and save whether it hit or not
                EnemyAttack(activeCharacter, activeCharacterAttack);
                    
                // wait for the attack animation to play
                while (activeCharacter.GetIsActing())
                {
                    
                    yield return null;
                }

                // attacked character resets
                characterToAttack.Recover();

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
        characterToAttack = player;
    }

    private void NextTurn(CharacterBattle activeCharacter)
    {
        // moves character that just acted to the back
        turnOrder.Dequeue();
        turnOrder.Enqueue(activeCharacter);

        // reset necessary variables
        activeCharacterAttack = null;
        ColorSwitcher.instance.ResetFlavor();
    }

    public void Escape()
    {
        StartCoroutine(WinBattle());
    }

    private void PlayerAttack(CharacterBattle activeCharacter, DessertAction action, Flavor flavor)
    {
        string text = $"{activeCharacter.CharacterName} threw a {action.Name} at {characterToAttack.CharacterName}";

        if ((characterToAttack as EnemyBattle).favoriteFlavor == flavor) text += " He loved it!";

        Attack(activeCharacter, action, text, flavor);
    }

    private void EnemyAttack(CharacterBattle activeCharacter, DessertAction action)
    {
        string text = $"{activeCharacter.CharacterName} tried to steal from {characterToAttack.CharacterName}";

        Attack(activeCharacter, action, text);
    }

    private void Attack(CharacterBattle activeCharacter, DessertAction action, string text, Flavor flavor = null)
    {
        battleUIManager.SetText(text);

        activeCharacter.DoAttack(action, characterToAttack, flavor);
    }

    private void Heal(CharacterBattle activeCharacter, DessertAction comboAction)
    {
        battleUIManager.SetText($"{activeCharacter.CharacterName} ate {comboAction.Name}");

        activeCharacter.DoHeal(comboAction);
    }

    private void DefeatedEnemy()
    {
        // convert target into the EnemyInfo type
        EnemyBattle defeatedEnemy = characterToAttack as EnemyBattle;

        // take enemy out of battle system
        var newTurnOrder = new Queue<CharacterBattle>(turnOrder.Where(x => x != defeatedEnemy));
        turnOrder = newTurnOrder;

        // gain stats from kill
        moneyCount += defeatedEnemy.GetLoot();

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

    public void SetAttackAction(CharacterBattle characterToAttack, DessertAction action, Flavor flavor)
    {
        this.characterToAttack = characterToAttack;
        activeCharacterAttack = action;
        this.flavor = flavor;
        attack = true;
        awaitCommand = false;
    }

    public void SetHealAction(DessertAction action)
    {
        activeCharacterAttack = action;
        attack = false;
        awaitCommand = false;
    }

    private void EndBattle()
    {
        player.EndCombat(moneyCount);

        StopAllCoroutines();

        enemies.Clear();

        moneyCount = 0;
        activeCharacterAttack = null;
        ColorSwitcher.instance.ResetFlavor();

        battleUIManager.ClearUI();

        gameObject.SetActive(false);
    }

    private IEnumerator WinBattle()
    {
        StopCoroutine(battleLoop);

        battleUIManager.SetText($"You won! You got {moneyCount} coins");

        yield return new WaitForSeconds(dialogueDisplayTime);

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

    public void LoseBattleFood()
    {
        StartCoroutine(LoseBattle());
    }
}