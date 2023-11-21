using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class BattleManager : MonoBehaviour {
    [Header("Managers")]
    [SerializeField] private WorldManager worldManager;
    [SerializeField] private UIManager uiManager;

    [Header("Other Variables")]
    public float dialogueDisplayTime = 1.5f;

    [Header("Extras, no need to change")]
    public List<EnemyBattle> enemies = new();
    private PlayerBattle player;
    private Queue<CharacterBattle> turnOrder = new();
    public CharacterBattle characterToAttack;
    public CharacterAction activeCharacterAttack;
    private Coroutine battleLoop;

    // tracker variables
    public int moneyCount = 0;
    public bool awaitCommand = false;
    public bool heal = false;
    public bool steal = false;
    public Flavor flavor;

    private void Awake() {
        gameObject.SetActive(false);
    }
    
    private void OnEnable() 
    {
        // Setup Characters
        InitializeCharacters();

        if (player.OutOfFood())
        {
            StartCoroutine(LoseBattle());
            return;
        }

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
        uiManager.SetForBattle(enemies);
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
                uiManager.ActivateForPlayerTurn();
                
                // make the loop stay here until the player selects their commands in the ui
                while (awaitCommand)
                {

                    yield return null;
                }

                // standard action
                // do the attack or heal
                if (heal)
                    Heal(activeCharacter, activeCharacterAttack, flavor);
                    
                else if (!steal)
                    PlayerAttack(activeCharacter, activeCharacterAttack, flavor);
                    

                // wait for the animation to play
                while (activeCharacter.GetIsActing())
                {
                    
                    yield return null;
                }

                // attacked character resets
                characterToAttack.Recover();

                // update enemy's health ui
                uiManager.UpdateHealth();

                // handle enemy's death
                if (characterToAttack.health <= 0)
                {
                    DefeatedEnemy();
                }
            } else {

                GetEnemyAttack(activeCharacter);
                    
                // wait for the attack animation to play
                while (activeCharacter.GetIsActing())
                {
                    
                    yield return null;
                }

                // attacked character resets
                characterToAttack.Recover();

                // updates player's health
                uiManager.UpdateHealth();

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
        heal = false;
        steal = false;
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

    private void PlayerAttack(CharacterBattle activeCharacter, CharacterAction action, Flavor flavor)
    {
        string text = $"{activeCharacter.CharacterName} threw {action.Name} at {characterToAttack.CharacterName}";

        Attack(activeCharacter, action, text, flavor);
    }

    private void EnemyAttack(CharacterBattle activeCharacter, CharacterAction action)
    {
        AudioManager.instance.PlayUIClip(5);

        string text = $"{activeCharacter.CharacterName} attacked {characterToAttack.CharacterName}";

        Attack(activeCharacter, action, text);
    }

    private void Attack(CharacterBattle activeCharacter, CharacterAction action, string text, Flavor flavor = null)
    {
        string tempText = activeCharacter.DoAttack(action, characterToAttack, flavor);

        if (tempText != "")
        {
            text = tempText;

            if (activeCharacter.GetType() == typeof(EnemyBattle)) uiManager.EnemyStolen(activeCharacter as EnemyBattle);
        }
            
        uiManager.SetText(text);
    }

    private void Heal(CharacterBattle activeCharacter, CharacterAction comboAction, Flavor flavor = null)
    {
        uiManager.SetText($"{activeCharacter.CharacterName} ate {comboAction.Name}");

        (activeCharacter as PlayerBattle).DoHeal(comboAction, flavor);
    }

    private void DefeatedEnemy()
    {
        // convert target into the EnemyInfo type
        EnemyBattle defeatedEnemy = characterToAttack as EnemyBattle;

        // take enemy out of battle system
        var newTurnOrder = new Queue<CharacterBattle>(turnOrder.Where(x => x != defeatedEnemy));
        turnOrder = newTurnOrder;

        // gain stats from kill
        // moneyCount += defeatedEnemy.GetLoot();

        // remove enemy from ui
        uiManager.DefeatedEnemy(defeatedEnemy);
        defeatedEnemy.Defeated();

        // end battle if it was the last enemy
        if (turnOrder.Count <= 1)
            StartCoroutine(WinBattle());
    }

    private void GetEnemyAttack(CharacterBattle enemy)
    {
        // set enemy combo
        activeCharacterAttack = (enemy as EnemyBattle).PickEnemyAttack();

        // enemy targets the player
        characterToAttack = player;

        // do the attack and save whether it hit or not
        EnemyAttack(enemy, activeCharacterAttack);
    }

    public void SetAttackAction(CharacterBattle characterToAttack, CharacterAction action, Flavor flavor)
    {
        this.characterToAttack = characterToAttack;
        activeCharacterAttack = action;
        this.flavor = flavor;
        awaitCommand = false;
    }

    public void SetHealAction(CharacterAction action, Flavor flavor)
    {
        activeCharacterAttack = action;
        heal = true;
        this.flavor = flavor;
        awaitCommand = false;
    }

    public void SetStealAction(CharacterBattle characterToAttack, Food food)
    {
        player.TakeItem(characterToAttack.StealItem(food));
        steal = true;
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

        uiManager.ClearUI();

        gameObject.SetActive(false);
    }

    private IEnumerator WinBattle()
    {
        StopCoroutine(battleLoop);

        uiManager.SetText($"You won! You got {worldManager.coinReward} coins");

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

        uiManager.SetText("You were defeated!");

        player.EndCombat(0);

        yield return new WaitForSeconds(dialogueDisplayTime);

        worldManager.LoseBattle();
    }

    public void LoseBattleFood()
    {
        StartCoroutine(LoseBattle());
    }
}