using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [Header("Managers")]
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private WorldManager worldManager;

    [Header("Left Menu")]
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject heartContainer;

    private GameObject[] hearts;
    private int currentHealthIndex;

    [Header("Middle Menu")]
    [Header("Containers")]
    [SerializeField] private GameObject initialContainer;
    [SerializeField] private GameObject arrowContainer, attackContainer, healContainer, flavorContainer, stealBackContainer, targetContainer, eHealthContainer;

    [Header("Buttons")]
    [SerializeField] private Button initialButton;
    [SerializeField] private Button stealBackButton, flavorButton;

    [Header("Button Prefabs")]
    [SerializeField] private Button actionButtonPrefab;
    [SerializeField] private Button backButtonPrefab;

    // Lists to fill with instantiated button prefabs
    private List<Button> targetButtons = new();
    private List<Button> attackButtons = new();
    private List<Button> healButtons = new();
    private List<Button> stealButtons = new();

    // store character information
    private PlayerBattle player;
    private List<EnemyBattle> enemies = new();
    private CharacterAction selectedAction;
    private CharacterBattle characterToAttack;
    private Flavor flavor;

    private bool[] initialChoice = new bool[3]{false,false,false};

    private List<(EnemyBattle, Food)> stolen = new();

    [Header("Right Menu")]

    [Header("Text Display")]
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private TextMeshProUGUI eHealthPrefab;

    private void Start() {
        player = worldManager.GetPlayer();

        hearts = new GameObject[player.maxHealth];

        currentHealthIndex = player.health - 1;

        arrowContainer.SetActive(false);
        
        SetHealth();
    }

    public void SetForBattle(List<EnemyBattle> enemies)
    {
        // ensure everything is reset
        ClearUI();

        // Initialize characters in the fight
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
        stealBackContainer.SetActive(false);
        stealBackButton.interactable = false;

        initialChoice = new bool[]{false,false,false};
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
        arrowContainer.SetActive(false);
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
            PlayerAction currentAction = player.GetAction(i);

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
        // UpdatePlayerHealth();

        // update health for each enemy
        UpdateEnemyHealth();
    }

    // private void UpdatePlayerHealth()
    // {
    //     // set player's health ui element
    //     worldUIManager.UpdateHealth();
    // }

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
            PlayerAction currentAction = player.GetAction(i);
            int uses = player.GetActionUses(i);

            // set button text
            string actionText = $"{currentAction.Name} ({uses}) remaining";

            attackButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = actionText;
            healButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = actionText;

            if (uses <= 0)
            {
                attackButtons[i].interactable = false;
                healButtons[i].interactable = false;

                if (player.OutOfDessert() && stolen.Count == 0) battleManager.LoseBattleFood();
            }
        }

        stealButtons.Clear();

        for (int i = 0; i < stealBackContainer.transform.childCount; i++)
        {
            Destroy(stealBackContainer.transform.GetChild(i).gameObject);
        }

        if (stolen.Count <= 0) return;

        for (int i = 0; i < stolen.Count; i++)
        {
            (EnemyBattle, Food) element = stolen[i];

            Button button = Instantiate(actionButtonPrefab, stealBackContainer.transform);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = element.Item2.name;
            button.onClick.AddListener(() => PickSteal(element));
            stealButtons.Add(button);
        }

        Button back = Instantiate(backButtonPrefab, stealBackContainer.transform);
        back.onClick.AddListener(BackFromSteal);
        stealButtons.Add(back);
    }

    public void SelectAttack()
    {
        ToggleMenu(initialContainer, attackContainer, attackButtons[0], 0, "What would you like to throw?");
    }

    public void SelectHeal()
    {
        ToggleMenu(initialContainer, healContainer, healButtons[0], 1, "What would you like to eat?");
    }

    public void SelectSteal()
    {
        ToggleMenu(initialContainer, stealBackContainer, stealButtons[0], 1, "What do you want to take back?");
    }

    public void SelectEscape()
    {
        battleManager.Escape();
    }

    private void BackFromAttack()
    {
        ToggleMenu(attackContainer, initialContainer, initialButton, 2);
    }

    public void BackFromFlavor()
    {
        ToggleMenu(flavorContainer, attackContainer, attackButtons[0], 3);
    }

    private void BackFromTarget()
    {   
        ColorSwitcher.instance.ResetFlavor();

        ToggleMenu(targetContainer, flavorContainer, flavorButton, 4, "Which flavor would you like to use?");
    }

    private void BackFromHeal()
    {
        ToggleMenu(healContainer, initialContainer, initialButton, 5);
    }

    private void BackFromSteal()
    {
        ToggleMenu(stealBackContainer, initialContainer, initialButton, 5);
    }

    private void PickAttack(PlayerAction action)
    {
        selectedAction = action;

        ToggleMenu(attackContainer, flavorContainer, flavorButton, 6, "Which flavor would you like to use?");
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

    private void PickSteal((EnemyBattle, Food) element)
    {
        PlayAudio(8);
        characterToAttack = element.Item1;
        stealBackContainer.SetActive(false);
        SendStealAction(element.Item2);
        SetText($"Stole back your {element.Item2.name} from {characterToAttack.CharacterName}!");
        RemoveFood(element);
    }

    private void PickHeal(PlayerAction action)
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

    private void SendStealAction(Food food)
    {
        battleManager.SetStealAction(characterToAttack, food);
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

        RemoveFoods(enemy);
    }

    public void ClearUI()
    {
        // clear all lists and ui elements from any previous battles
        targetButtons.Clear();
        attackButtons.Clear();
        healButtons.Clear();
        stolen.Clear();

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

        for (int i = 0; i < stealBackContainer.transform.childCount; i++)
        {
            Destroy(stealBackContainer.transform.GetChild(i).gameObject);
        }

        // reset used variables
        characterToAttack = null;
    }

    public void EnemyStolen(EnemyBattle enemy)
    {
        stolen.Add((enemy, enemy.GetRecentlyStolenItem()));

        stealBackButton.interactable = true;
    }

    private void RemoveFoods(EnemyBattle enemy)
    {
        if (stolen.Count <= 0) return;

        List<(EnemyBattle, Food)> temp = new();

        foreach ((EnemyBattle, Food) s in stolen)
        {
            if (s.Item1 == enemy)
                temp.Add(s);
        }

        foreach ((EnemyBattle, Food) s in temp)
            RemoveFood(s);
    }

    private void RemoveFood((EnemyBattle, Food) food)
    {
        stolen.Remove(food);

        if (stolen.Count <= 0) stealBackButton.interactable = false;
    }

    private void ToggleMenu(GameObject close, GameObject open, Button activeButton, int audioIndex = 0, string text = "")
    {
        PlayAudio(audioIndex);
        close.SetActive(false);
        open.SetActive(true);
        Utility.SetActiveButton(activeButton);
        SetText(text);
    }

    private void PlayAudio(int i)
    {
        AudioManager.instance.PlayUIClip(i);
    }

    private void SetHealth()
    {
        int children = heartContainer.transform.childCount;

        if (children > 0)
        {
            for (int i = 0; i < children; i++)
            {
                Destroy(heartContainer.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            GameObject image = Instantiate(heartPrefab, heartContainer.transform);
            hearts[i] = image;
        }

        LoseHealth(player.maxHealth - player.health);      
    }

    // public void UpdateHealth()
    // {
    //     int targetIndex = playerBattle.health - 1;

    //     if (targetIndex > currentHealthIndex) AddHealth(targetIndex - currentHealthIndex);

    //     else if (targetIndex < currentHealthIndex) LoseHealth(currentHealthIndex - targetIndex);
    // }

    public void AddHealth(int heal = 1)
    {
        int targetIndex = currentHealthIndex + heal;

        if (targetIndex >= hearts.Length) targetIndex = hearts.Length - 1;

        for (int i = currentHealthIndex; i < targetIndex; i++)
        {
            hearts[i + 1].SetActive(true);
        }

        currentHealthIndex = targetIndex;
    }

    public void LoseHealth(int damage = 1)
    {
        int targetIndex = currentHealthIndex - damage;

        if (targetIndex < -1) targetIndex = -1;

        for (int i = currentHealthIndex; i > targetIndex; i--)
        {
            hearts[i].SetActive(false);
        }

        currentHealthIndex = targetIndex;
    }
}