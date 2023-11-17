using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    [Header("Managers")]
    [SerializeField] private BattleManager battleManager;
    public WorldManager worldManager;

    [Header("Left Menu")]
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject heartContainer;

    private GameObject[] hearts;
    private int currentHealthIndex;

    [Header("Middle Menu")]
    [Header("Containers")]
    [SerializeField] private GameObject initialContainer;
    [SerializeField] private GameObject itemContainer, flavorContainer, stealBackContainer, targetContainer;

    [Header("Buttons")]
    [SerializeField] private Button initialButton;
    [SerializeField] private Button flavorButton;
    [SerializeField] private GameObject backButton, stealBackButton, leftArrowItem, rightArrowItem, leftArrowSteal, rightArrowSteal;

    [Header("Button Prefabs")]
    [SerializeField] private Button itemButtonPrefab;
    [SerializeField] private Button enemyButtonPrefab;

    // Lists to fill with instantiated button prefabs
    private List<Button> targetButtons = new();
    private List<Button> itemButtons = new();
    private List<Button> stealButtons = new();
    private TextMeshProUGUI[] flavorValues;

    // store character information
    private PlayerBattle player;
    private List<EnemyBattle> enemies = new();
    private CharacterAction selectedAction;
    private CharacterBattle characterToAttack;
    private Flavor flavor;

    private bool attack = true;
    private int currentMenu = 0;
    private int itemIndex, stealIndex = 0;

    private List<(EnemyBattle, Food)> stolen = new();

    [Header("Right Menu")]

    [Header("Text Display")]
    [SerializeField] private TextMeshProUGUI actionText;

    private void Awake() {
        if (instance == null)
            instance = this;
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBattle>();

        hearts = new GameObject[player.maxHealth];

        currentHealthIndex = player.health - 1;

        flavorValues = new TextMeshProUGUI[flavorContainer.transform.childCount];

        backButton.SetActive(false);

        SetText("");
        
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

        itemIndex = stealIndex = 0;
        leftArrowItem.SetActive(false);
        leftArrowSteal.SetActive(false);
        
        // Deactivate menus until they are needed
        initialContainer.SetActive(false);
        targetContainer.SetActive(false);
        itemContainer.SetActive(false);
        flavorContainer.SetActive(false);
        stealBackContainer.SetActive(false);

        stealBackButton.SetActive(false);
        backButton.SetActive(false);
    }

    public void ActivateForPlayerTurn()
    {
        // Update UI
        SetText("");

        // reset used variables
        characterToAttack = null;
        flavor = null;
        UpdateActions();
        currentMenu = 0;
        attack = true;
        
        initialContainer.SetActive(true);
        Utility.SetActiveButton(initialButton);
    }

    private void SetEnemies()
    {
        // Make ui components for each enemy
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyBattle enemy = enemies[i];

            // set health display
            // TextMeshProUGUI enemyHealthText = Instantiate(eHealthPrefab, eHealthContainer.transform);
            // enemyHealthText.rectTransform.anchoredPosition = new Vector3(0, -i * 100);

            // set button to select enemy
            Button selectEnemy = Instantiate(enemyButtonPrefab, targetContainer.transform);
            selectEnemy.onClick.AddListener(() => PickTarget(enemy));
            targetButtons.Add(selectEnemy);

            // set button text
            selectEnemy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = enemy.CharacterName;
            selectEnemy.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = i.ToString();
        }
    }

    private void SetActions()
    {
        Food[] foodList = FoodList.GetInstance().GetFoods();

        // set a button for each combo attack
        for (int i = 0; i < player.CountActions(); i++)
        {
            // get the player's next combo attack
            PlayerAction currentAction = player.GetAction(i);

            // make the button to select attack
            Button selectAttack = Instantiate(itemButtonPrefab, itemContainer.transform.GetChild(0));
            selectAttack.onClick.AddListener(() => PickItem(currentAction));
            itemButtons.Add(selectAttack);

            selectAttack.transform.GetChild(1).GetComponent<Image>().sprite = player.GetAction(i).Food.sprite;
            selectAttack.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = player.GetActionUses(i).ToString("D2");

            selectAttack.gameObject.SetActive(false);
        }

        itemButtons[0].gameObject.SetActive(true);

        Transform flavorButtons = flavorContainer.transform.GetChild(1);

        for (int i = 0; i < flavorValues.Length; i++)
        {
            flavorValues[i] = flavorButtons.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
        }
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
        // for (int i = 0; i < enemies.Count; i++)
        // {
        //     EnemyBattle enemy = enemies[i];

        //     if (enemy.health < 0 && targetButtons[i].interactable)
        //     {
        //         targetButtons[i].interactable = false;
        //     }
        // }
    }

    private void UpdateActions()
    {
        List<Button> delete = new();

        for (int i = 0; i < itemButtons.Count; i++)
        {
            // PlayerAction currentAction = player.GetAction(i);
            int uses = player.GetActionUses(i);
            Transform child = itemButtons[i].transform.GetChild(0);

            if (uses > 0)
            {
                child.GetComponent<TextMeshProUGUI>().text = uses.ToString("D2");
            } 
            else
            {
                if (player.OutOfDessert() && stolen.Count == 0) battleManager.LoseBattleFood();

                Destroy(child.gameObject);
                delete.Add(itemButtons[i]);
                itemIndex = 0;
                leftArrowItem.SetActive(false);

                if (itemButtons.Count <= 1) rightArrowItem.SetActive(false);
            }
        }

        foreach (Button b in delete)
        {
            itemButtons.Remove(b);
        }

        for (int i = 0; i < flavorValues.Length; i++)
        {
            flavorValues[i].text = player.GetFlavorUses(i).ToString("D2");
        }

        stealButtons.Clear();

        Transform container = stealBackContainer.transform.GetChild(0);

        for (int i = 0; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }

        if (stolen.Count <= 0) return;

        if (stolen.Count == 1) rightArrowItem.SetActive(false);

        for (int i = 0; i < stolen.Count; i++)
        {
            (EnemyBattle, Food) element = stolen[i];

            Button button = Instantiate(itemButtonPrefab, stealBackContainer.transform);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = element.Item2.name;
            button.onClick.AddListener(() => PickSteal(element));
            stealButtons.Add(button);
        }
    }

    public void SelectAttack()
    {
        attack = true;
        currentMenu++;
        backButton.SetActive(true);
        ToggleMenu(initialContainer, itemContainer, itemButtons[itemIndex], 0, "What would you like to throw?");
    }

    public void SelectHeal()
    {
        attack = false;
        currentMenu++;
        backButton.SetActive(true);
        ToggleMenu(initialContainer, itemContainer, itemButtons[itemIndex], 1, "What would you like to eat?");
    }

    public void SelectSteal()
    {
        currentMenu = 4;
        ToggleMenu(initialContainer, stealBackContainer, stealButtons[stealIndex], 1, "What do you want to take back?");
    }

    public void SelectLeftItem()
    {
        if (itemIndex <= 0) return;

        itemButtons[itemIndex].gameObject.SetActive(false);

        itemIndex--;

        itemButtons[itemIndex].gameObject.SetActive(true);

        if (itemIndex == 0) 
        {
            leftArrowItem.SetActive(false);
            Utility.SetActiveButton(itemButtons[itemIndex]);
        }

        else if (itemIndex == itemButtons.Count - 2) rightArrowItem.SetActive(true);

        
    }

    public void SelectLeftSteal()
    {
        if (stealIndex <= 0) return;

        stealButtons[stealIndex].gameObject.SetActive(false);

        stealIndex--;

        stealButtons[stealIndex].gameObject.SetActive(true);

        if (stealIndex == 0) 
        {
            leftArrowSteal.SetActive(false);
            Utility.SetActiveButton(stealButtons[stealIndex]);
        }

        else if (stealIndex == stealButtons.Count - 2) rightArrowSteal.SetActive(true);
    }

    public void SelectRightItem()
    {
        if (itemIndex >= itemButtons.Count - 1) return;

        itemButtons[itemIndex].gameObject.SetActive(false);

        itemIndex++;

        itemButtons[itemIndex].gameObject.SetActive(true);

        if (itemIndex == itemButtons.Count - 1) 
        {
            rightArrowItem.SetActive(false);
            Utility.SetActiveButton(itemButtons[itemIndex]);
        }

        else if (itemIndex == 1) leftArrowItem.SetActive(true);
    }

    public void SelectRightSteal()
    {
        if (stealIndex >= stealButtons.Count - 1) return;

        stealButtons[stealIndex].gameObject.SetActive(false);

        stealIndex++;

        stealButtons[stealIndex].gameObject.SetActive(true);

        if (stealIndex == stealButtons.Count - 1) 
        {
            rightArrowSteal.SetActive(false);
            Utility.SetActiveButton(stealButtons[stealIndex]);
        }

        else if (stealIndex == 1) leftArrowSteal.SetActive(true);
    }

    private void BackFromItem()
    {
        currentMenu--;
        backButton.SetActive(false);
        ToggleMenu(itemContainer, initialContainer, initialButton, 2);
    }

    public void BackFromFlavor()
    {
        currentMenu--;
        ToggleMenu(flavorContainer, itemContainer, itemButtons[itemIndex], 3);
    }

    private void BackFromTarget()
    {   
        ColorSwitcher.instance.ResetFlavor();

        currentMenu--;
        ToggleMenu(targetContainer, flavorContainer, flavorButton, 4, "Which flavor would you like to use?");
    }

    private void BackFromSteal()
    {
        currentMenu = 0;
        ToggleMenu(stealBackContainer, initialContainer, initialButton, 5);
    }

    public void Back()
    {
        switch (currentMenu)
        {
            case 1:
                BackFromItem();
            break;
            case 2:
                BackFromFlavor();
            break;
            case 3:
                BackFromTarget();
            break;
            case 4:
                BackFromSteal();
            break;
        }
    }

    public void Skip()
    {
        PickFlavor(null);
    }

    private void PickItem(PlayerAction action)
    {
        currentMenu++;
        selectedAction = action;

        ToggleMenu(itemContainer, flavorContainer, flavorButton, 6, "Which flavor would you like to use?");
    }

    public void PickFlavor(Flavor flavor)
    {
        PlayAudio(7);
        currentMenu++;
        this.flavor = flavor;

        ColorSwitcher.instance.SetFlavor(flavor);
        flavorContainer.SetActive(false);

        if (attack)
        {
            targetContainer.SetActive(true);
            Utility.SetActiveButton(targetButtons[0]);
            SetText($"Who would you like to throw your {selectedAction.Name} at?");
        } else 
        {
            SendHealAction();
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
        Destroy(targetButton.gameObject);

        // set enemy's health ui to defeated text
        // eHealthContainer.transform.GetChild(enemyIndex).GetComponent<TextMeshProUGUI>().text = enemy.CharacterName + " is defeated";

        RemoveFoods(enemy);
    }

    public void ClearUI()
    {
        // clear all lists and ui elements from any previous battles
        targetButtons.Clear();
        itemButtons.Clear();
        stolen.Clear();

        // foreach (TextMeshProUGUI t in eHealthContainer.GetComponentsInChildren<TextMeshProUGUI>())
        // {
        //     Destroy(t.gameObject);
        // }

        for (int i = 0; i < targetContainer.transform.childCount; i++)
        {
            Destroy(targetContainer.transform.GetChild(i).gameObject);
        }

        Transform container = itemContainer.transform.GetChild(0);

        for (int i = 0; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }

        container = stealBackContainer.transform.GetChild(0);

        for (int i = 0; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }

        // reset used variables
        characterToAttack = null;
    }

    public void EnemyStolen(EnemyBattle enemy)
    {
        stolen.Add((enemy, enemy.GetRecentlyStolenItem()));

        stealBackButton.SetActive(true);
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

        stealIndex = 0;

        if (stolen.Count <= 0) stealBackButton.SetActive(false);
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