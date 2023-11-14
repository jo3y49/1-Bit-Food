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
    [SerializeField] private GameObject initialContainer;
    [SerializeField] private GameObject attackContainer, healContainer, flavorContainer, targetContainer, eHealthContainer;

    [Header("Buttons")]
    [SerializeField] private Button initialButton;
    [SerializeField] private Button flavorButton;

    [Header("Button Prefabs")]
    [SerializeField] private Button actionButtonPrefab;
    [SerializeField] private Button backButtonPrefab;

    // Lists to fill with instantiated button prefabs
    private List<Button> targetButtons = new();
    private List<Button> attackButtons = new();
    private List<Button> healButtons = new();

    // store character information
    private PlayerBattle player;
    private List<EnemyBattle> enemies = new();
    private FoodAction selectedAction;
    private CharacterBattle characterToAttack;
    private Flavor flavor;

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
        SetActions();
        
        // Deactivate menus until they are needed
        initialContainer.SetActive(false);
        targetContainer.SetActive(false);
        attackContainer.SetActive(false);
        flavorContainer.SetActive(false);
        healContainer.SetActive(false);
    }

    public void ActivateForPlayerTurn()
    {
        // Update UI
        SetText("");

        // reset used variables
        characterToAttack = null;
        flavor = null;
        UpdateActions();
        
        initialContainer.SetActive(true);
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
            Button selectEnemy = Instantiate(actionButtonPrefab, targetContainer.transform);
            selectEnemy.onClick.AddListener(() => PickTarget(enemy));
            targetButtons.Add(selectEnemy);

            // set button text
            selectEnemy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = enemy.CharacterName;
        }

        Button back = Instantiate(backButtonPrefab, targetContainer.transform);
        back.onClick.AddListener(BackFromTarget);
    }

    private void SetActions()
    {
        // set a button for each combo attack
        for (int i = 0; i < player.CountActions(); i++)
        {
            // get the player's next combo attack
            FoodAction currentAction = player.GetAction(i);

            // make the button to select attack
            Button selectAttack = Instantiate(actionButtonPrefab, attackContainer.transform);
            selectAttack.onClick.AddListener(() => PickAttack(currentAction));
            attackButtons.Add(selectAttack);

            // make the button to select heal
            Button selectHeal = Instantiate(actionButtonPrefab, healContainer.transform);
            selectHeal.onClick.AddListener(() => PickHeal(currentAction));
            healButtons.Add(selectHeal);

            // set button text
            string actionText = $"{currentAction.Name} ({player.GetActionUses(i)}) remaining";

            selectAttack.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = actionText;
            selectHeal.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = actionText;
        }

        Button backAttack = Instantiate(backButtonPrefab, attackContainer.transform);
        backAttack.onClick.AddListener(BackFromAttack);

        Button backHeal = Instantiate(backButtonPrefab, healContainer.transform);
        backHeal.onClick.AddListener(BackFromHeal);
    }

    public void UpdateHealth()
    {
        // update player's health
        UpdatePlayerHealth();

        // update health for each enemy
        UpdateEnemyHealth();
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

            if (enemy.health <= 0) continue;

            // set health display for enemy
            eHealthContainer.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = enemy.CharacterName + "'s Health: " + enemy.health;
        }
    }

    private void UpdateActions()
    {
        for (int i = 0; i < attackButtons.Count; i++)
        {
            FoodAction currentAction = player.GetAction(i);
            int uses = player.GetActionUses(i);

            // set button text
            string actionText = $"{currentAction.Name} ({uses}) remaining";

            attackButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = actionText;
            healButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = actionText;

            if (uses <= 0)
            {
                attackButtons[i].interactable = false;
                healButtons[i].interactable = false;

                if (player.OutOfDessert()) battleManager.LoseBattleFood();
            }
        }
    }

    public void SelectAttack()
    {
        PlayAudio(0);
        initialContainer.SetActive(false);
        attackContainer.SetActive(true);
        Utility.SetActiveButton(attackButtons[0]);
        SetText("What would you like to throw?");
    }

    public void SelectHeal()
    {
        PlayAudio(1);
        initialContainer.SetActive(false);
        healContainer.SetActive(true);
        Utility.SetActiveButton(healButtons[0]);
        SetText("What would you like to eat?");
    }

    public void SelectEscape()
    {
        battleManager.Escape();
    }

    private void BackFromAttack()
    {
        PlayAudio(2);
        attackContainer.SetActive(false);
        initialContainer.SetActive(true);
        Utility.SetActiveButton(initialButton);
        SetText("");
    }

    public void BackFromFlavor()
    {
        PlayAudio(3);
        flavorContainer.SetActive(false);
        attackContainer.SetActive(true);
        Utility.SetActiveButton(attackButtons[0]);
        SetText("What would you like to throw?");
    }

    private void BackFromTarget()
    {   
        PlayAudio(4);
        targetContainer.SetActive(false);
        flavorContainer.SetActive(true);
        ColorSwitcher.instance.ResetFlavor();
        Utility.SetActiveButton(flavorButton);
        SetText("Which flavor would you like to use?");
    }

    private void BackFromHeal()
    {
        PlayAudio(5);
        healContainer.SetActive(false);
        initialContainer.SetActive(true);
        Utility.SetActiveButton(initialButton);
        SetText("");
    }

    private void PickAttack(FoodAction action)
    {
        PlayAudio(6);
        selectedAction = action;
        attackContainer.SetActive(false);
        flavorContainer.SetActive(true);
        Utility.SetActiveButton(flavorButton);
        SetText("Which flavor would you like to use?");
    }

    public void PickFlavor(Flavor flavor)
    {
        PlayAudio(7);
        this.flavor = flavor;

        ColorSwitcher.instance.SetFlavor(flavor);
        flavorContainer.SetActive(false);

        if (enemies.Count > 1)
        {
            targetContainer.SetActive(true);
            Utility.SetActiveButton(targetButtons[0]);
            SetText($"Who would you like to throw your {selectedAction.Name} at?");
        } else 
        {
            PickTarget(enemies[0]);
        }

    }

    private void PickTarget(CharacterBattle characterToAttack)
    {
        PlayAudio(8);
        this.characterToAttack = characterToAttack;
        targetContainer.SetActive(false);
        SendAttackAction();
    }

    private void PickHeal(FoodAction action)
    {
        PlayAudio(9);
        selectedAction = action;
        healContainer.SetActive(false);
        SendHealAction();
    }

    private void SendAttackAction()
    {
        battleManager.SetAttackAction(characterToAttack, selectedAction, flavor);
    }

    private void SendHealAction()
    {
        battleManager.SetHealAction(selectedAction);
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
        // enemies.Remove(enemy);
    }

    public void ClearUI()
    {
        // clear all lists and ui elements from any previous battles
        targetButtons.Clear();
        attackButtons.Clear();
        healButtons.Clear();

        foreach (TextMeshProUGUI t in eHealthContainer.GetComponentsInChildren<TextMeshProUGUI>())
        {
            Destroy(t.gameObject);
        }

        for (int i = 0; i < targetContainer.transform.childCount; i++)
        {
            Destroy(targetContainer.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < attackContainer.transform.childCount; i++)
        {
            Destroy(attackContainer.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < healContainer.transform.childCount; i++)
        {
            Destroy(healContainer.transform.GetChild(i).gameObject);
        }

        // reset used variables
        characterToAttack = null;
    }

    private void PlayAudio(int i)
    {
        AudioManager.instance.PlayUIClip(i);
    }
}