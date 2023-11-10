using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour {
    [SerializeField] private BattleManager battleManager;

    [Header("Text Display")]
    [SerializeField] private TextMeshProUGUI pHealth;
    [SerializeField] private TextMeshProUGUI actionText, eHealthPrefab;

    [Header("Containers")]
    [SerializeField] private GameObject attackContainer;
    [SerializeField] private GameObject targetContainer, eHealthContainer;

    [Header("Button Prefabs")]
    [SerializeField] private Button attackButtonPrefab;
    [SerializeField] private Button targetButtonPrefab, backButtonPrefab;

    // Lists to fill with instantiated button prefabs
    private List<Button> targetButtons = new();
    private List<Button> attackButtons = new();

    // store character information
    private PlayerBattle player;
    private List<EnemyBattle> enemies = new();
    private AttackAction selectedAtack;
    private CharacterBattle characterToAttack;

    public void SetForBattle(PlayerBattle player, List<EnemyBattle> enemies)
    {
        // ensure everything is reset
        ClearUI();

        // Initialize characters in the fight
        this.player = player;
        this.enemies = enemies;

        // Setup all the menus
        SetEnemies();
        UpdateHealth();
        SetAttacks();
        
        // Deactivate menus until they are needed
        targetContainer.SetActive(false);
        attackContainer.SetActive(false);
    }

    public void ActivateForPlayerTurn()
    {
        // Update UI
        SetText("");

        // reset used variables
        characterToAttack = null;
        
        attackContainer.SetActive(true);
        Utility.SetActiveButton(attackButtons[0]);
    }

    private void SetEnemies()
    {
        // Make ui components for each enemy
        for (int i = 0; i < enemies.Count; i++)
        {
            // set health display
            EnemyBattle enemy = enemies[i];
            TextMeshProUGUI enemyHealthText = Instantiate(eHealthPrefab, eHealthContainer.transform);
            enemyHealthText.rectTransform.anchoredPosition = new Vector3(0, -i * 100);

            // set button to select enemy
            Button selectEnemy = Instantiate(targetButtonPrefab, targetContainer.transform);
            selectEnemy.onClick.AddListener(() => PickTarget(enemy));
            targetButtons.Add(selectEnemy);

            // set button text
            selectEnemy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = enemy.CharacterName;
        }

        Button back = Instantiate(backButtonPrefab, targetContainer.transform);
        back.onClick.AddListener(BackFromTarget);
    }

    public void UpdateHealth()
    {
        // update player's health
        UpdatePlayerHealth();

        // update health for each enemy
        UpdateEnemyHealth();
    }

    private void SetAttacks()
    {
        // set a button for each combo attack
        for (int i = 0; i < player.CountActions(); i++)
        {
            // get the player's next combo attack
            AttackAction currentAttack = player.GetAction(i);

            // make the button to select attack
            Button selectAttack = Instantiate(attackButtonPrefab, attackContainer.transform);
            selectAttack.onClick.AddListener(() => PickAttack(currentAttack));
            attackButtons.Add(selectAttack);

            // set button text
            selectAttack.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentAttack.Name;
        }
    }

    private void UpdatePlayerHealth()
    {
        // set player's health ui element
        pHealth.text = player.CharacterName + "'s Health: " + player.health;
    }

    private void UpdateEnemyHealth()
    {
        // set all enemys' health ui elements
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyBattle enemy = enemies[i];

            // set health display for enemy
            eHealthContainer.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = enemy.CharacterName + "'s Health: " + enemy.health;
        }
    }

    public void SelectEscape()
    {
        battleManager.Escape();
    }

    private void BackFromTarget()
    {   
        targetContainer.SetActive(false);
        attackContainer.SetActive(true);
        Utility.SetActiveButton(attackButtons[0]);
    }

    private void PickAttack(AttackAction attackAction)
    {
        selectedAtack = attackAction;
        attackContainer.SetActive(false);
        targetContainer.SetActive(true);
        Utility.SetActiveButton(targetButtons[0]);
    }

    private void PickTarget(CharacterBattle characterToAttack)
    {
        this.characterToAttack = characterToAttack;
        SendAttackAction();
        targetContainer.SetActive(false);
    }

    private void SendAttackAction()
    {
        battleManager.SetComboAction(characterToAttack, selectedAtack);
    }

    public void SetText(string s)
    {
        actionText.text = s;
    }

    public void DefeatedEnemy(EnemyBattle enemy)
    {
        // get the enemy's index in enemy list
        int enemyIndex = enemies.IndexOf(enemy);

        // remove button associated with enemy
        Button targetButton = targetContainer.transform.GetChild(enemyIndex).GetComponent<Button>();
        targetButtons.Remove(targetButton);
        targetButton.interactable = false;

        // set enemy's health ui to defeated text
        eHealthContainer.transform.GetChild(enemyIndex).GetComponent<TextMeshProUGUI>().text = enemy.CharacterName + " is defeated";

        // remove from list
        enemies.Remove(enemy);
    }

    public void ClearUI()
    {
        // clear all lists and ui elements from any previous battles
        targetButtons.Clear();
        attackButtons.Clear();

        foreach (TextMeshProUGUI t in eHealthContainer.GetComponentsInChildren<TextMeshProUGUI>())
        {
            Destroy(t.gameObject);
        }

        for (int i = 0; i < targetContainer.transform.childCount; i++)
        {
            Destroy(targetContainer.transform.GetChild(i).gameObject);
        }

        // reset used variables
        characterToAttack = null;
    }
    
}