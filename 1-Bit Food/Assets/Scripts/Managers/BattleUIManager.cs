using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour {
    public static BattleUIManager instance;

    [Header("Managers")]
    [SerializeField] private BattleManager battleManager;
    public WorldManager worldManager;
    [SerializeField] private FeedbackUIManager feedbackManager;

    [Header("Middle Menu")]
    [Header("Containers")]
    [SerializeField] private GameObject initialContainer;
    [SerializeField] private GameObject initialContainerSmall, initialContainerLarge, itemContainer, flavorContainer, stealBackContainer, targetContainer;

    [Header("Buttons")]
    [SerializeField] private Button attackButtonSmall;
    [SerializeField] private Button attackButtonLarge, healButtonSmall, stealButton, flavorButton;
    [SerializeField] private GameObject backButton, leftArrowItem, rightArrowItem, leftArrowSteal, rightArrowSteal;

    private Button initialButton;

    [Header("Button Prefabs")]
    [SerializeField] private Button itemButtonPrefab;
    [SerializeField] private Button stealButtonPrefab, enemyButtonPrefab;

    // Lists to fill with instantiated button prefabs
    private List<Button> targetButtons = new();
    private List<Button> itemButtons = new();
    private List<Button> stealButtons = new();
    private List<Button> flavorButtons = new();

    // store character information
    private PlayerBattle player;
    private List<EnemyBattle> enemies = new();
    private CharacterAction selectedAction;
    private CharacterBattle characterToAttack;
    private Flavor flavor;

    private bool attack = true;
    private int currentMenu = 0;
    private int itemIndex, stealIndex = 0;
    private bool stole = false;

    private List<(EnemyBattle, Food)> stolen = new();
    private List<Food> playerFood = new();

    [Header("Right Menu")]

    [Header("Text Display")]
    [SerializeField] private TextMeshProUGUI actionText;

    // private void Awake() {
    //     if (instance == null)
    //         instance = this;
    // }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBattle>();

        backButton.SetActive(false);

        SetText("");
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
        SetFlavors();
        
        // Deactivate menus until they are needed
        initialContainer.SetActive(false);
        initialContainerSmall.SetActive(false);
        initialContainerLarge.SetActive(false);
        targetContainer.SetActive(false);
        itemContainer.SetActive(false);
        flavorContainer.SetActive(false);
        stealBackContainer.SetActive(false);
        backButton.SetActive(false);
    }

    public void ActivateForPlayerTurn()
    {
        initialContainer.SetActive(true);
        actionText.gameObject.SetActive(false);

        // Update UI
        SetText("");

        // reset used variables
        characterToAttack = null;
        flavor = null;
        UpdateActions();
        currentMenu = 0;
        attack = true;

        if (playerFood.Count == 0)
        {
            initialButton = stealButton;
            attackButtonSmall.gameObject.SetActive(false);
            healButtonSmall.gameObject.SetActive(false);
        } 

        Utility.SetActiveButton(initialButton);
    }

    private void SetEnemies()
    {
        // Make ui components for each enemy
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyBattle enemy = enemies[i];

            // set health display
            // TextMeshProUGUI enemyHealthText = Instantiate(enemyPrefabText, enemyHealthContainer.transform);
            // enemyHealthText.rectTransform.anchoredPosition = new Vector3(0, -i * 50);
            // enemyHealthText.text = $"enemy {i+1}: {enemies[i].health}";
            // enemyHealthTexts.Add(enemyHealthText);

            // set button to select enemy
            Button selectEnemy = Instantiate(enemyButtonPrefab, targetContainer.transform);
            selectEnemy.onClick.AddListener(() => PickTarget(enemy));
            targetButtons.Add(selectEnemy);

            // set button text
            selectEnemy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = enemy.CharacterName;
            selectEnemy.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (i+1).ToString();
        }
    }

    private void SetActions()
    {
        // set a button for each combo attack
        for (int i = 0; i < player.CountActions(); i++)
        {
            SetUpAction(i);
        }

        itemButtons[itemIndex].gameObject.SetActive(true);
    }

    private void SetUpAction(int foodIndex)
    {
        if (player.GetActionUses(foodIndex) <= 0) return;

        // get the player's next combo attack
        PlayerAction currentAction = player.GetAction(foodIndex);

        // make the button to select attack
        Button selectAttack = Instantiate(itemButtonPrefab, itemContainer.transform.GetChild(0));
        selectAttack.onClick.AddListener(() => PickItem(currentAction));
        itemButtons.Add(selectAttack);

        selectAttack.transform.GetChild(1).GetComponent<Image>().sprite = player.GetAction(foodIndex).Food.sprite;
        selectAttack.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = player.GetActionUses(foodIndex).ToString("D2");

        playerFood.Add(player.GetAction(foodIndex).Food);

        selectAttack.gameObject.SetActive(false);
    }

    private void SetFlavors()
    {
        Transform flavorItems = flavorContainer.transform.GetChild(1);

        for (int i = 0; i < flavorItems.childCount; i++)
        {
            Button button = flavorItems.GetChild(i).GetComponent<Button>();

            button.interactable = true;
            flavorButtons.Add(button);
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

        //     enemyHealthTexts[i].text = $"enemy {i+1}: {enemy.health}"; 

        //     if (enemy.health < 0 && targetButtons[i].interactable)
        //     {
        //         targetButtons[i].interactable = false;
        //     }
        // }
    }

    private void UpdateActions()
    {
        int items = itemButtons.Count;

        for (int i = 0; i < items; i++)
        {
            // PlayerAction currentAction = player.GetAction(i);
            int uses = player.GetActionUses(playerFood[i]);
            Transform child = itemButtons[i].transform.GetChild(0);

            if (uses > 0)
            {
                child.GetComponent<TextMeshProUGUI>().text = uses.ToString("D2");
            } 
            else
            {
                if (playerFood.Count == 0)
                {
                    if (stolen.Count == 0)
                    {
                        battleManager.LoseBattleFood();
                        return;
                    }
                } 

                GameObject oldButton = itemButtons[i].gameObject;

                playerFood.RemoveAt(i);
                itemButtons.RemoveAt(i);
                Destroy(oldButton);

                if (itemIndex >= i)
                {
                    if (itemIndex != 0)
                    {
                        // itemButtons[itemIndex].gameObject.SetActive(false);
                        itemIndex--;
                    } 

                    if (playerFood.Count != 0) itemButtons[itemIndex].gameObject.SetActive(true);
                }

                i--;
                items--;
            }
        }

        for (int i = 0; i < flavorButtons.Count; i++)
        {
            int uses = player.GetFlavorUses(i);

            if (uses <= 0) flavorButtons[i].interactable = false;

            flavorButtons[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = uses.ToString("D2");
        }

        UpdateSteal();
    }

    private void UpdateSteal()
    {
        if (stolen.Count <= 0) 
        {
            initialContainerLarge.SetActive(true);
            initialContainerSmall.SetActive(false);
            initialButton = attackButtonLarge;
            return;
        }

        initialContainerSmall.SetActive(true);
        initialContainerLarge.SetActive(false);
        initialButton = attackButtonSmall;

        stealButtons.Clear();

        Transform container = stealBackContainer.transform.GetChild(0);

        for (int i = 0; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }

        for (int i = 0; i < stolen.Count; i++)
        {
            (EnemyBattle, Food) element = stolen[i];

            Button button = Instantiate(stealButtonPrefab, container.transform);
            button.onClick.AddListener(() => PickSteal(element));
            button.transform.GetChild(0).GetComponent<Image>().sprite = element.Item2.sprite;
            stealButtons.Add(button);
            button.gameObject.SetActive(false);
        }

        stealButtons.Reverse();

        stealIndex = 0;
        stealButtons[0].gameObject.SetActive(true);
    }

    // private void RefreshArrows()
    // {
    //     leftArrowItem.SetActive(true);
    //     leftArrowSteal.SetActive(true);
    //     rightArrowItem.SetActive(true);
    //     rightArrowSteal.SetActive(true);

    //     if (stealIndex == 0) leftArrowSteal.SetActive(false);
    // }

    public void SelectAttack()
    {
        SelectAttackOrHeal(true);
    }

    public void SelectHeal()
    {
        SelectAttackOrHeal(false);
    }

    private void SelectAttackOrHeal(bool attack)
    {
        this.attack = attack;
        currentMenu++;
        backButton.SetActive(true);

        if (playerFood.Count != 0)
        {
            ToggleMenu(initialContainer, itemContainer, itemButtons[itemIndex], 0);
            feedbackManager.GoToItemMenu(attack, playerFood[itemIndex]);
            return;
        } 
        ToggleMenu(initialContainer, itemContainer, backButton.GetComponent<Button>(),0);
    }

    public void SelectSteal()
    {
        currentMenu = 4;
        backButton.SetActive(true);
        ToggleMenu(initialContainer, stealBackContainer, stealButtons[stealIndex], 1);
        feedbackManager.GoToItemMenu(true, stolen[stealButtons.Count - (stealIndex + 1)].Item2);
    }

    public void SelectLeftItem()
    {
        if (itemIndex <= 0) return;

        itemButtons[itemIndex].gameObject.SetActive(false);

        itemIndex--;

        itemButtons[itemIndex].gameObject.SetActive(true);

        feedbackManager.UpdateItemMenu(playerFood[itemIndex]);      
    }

    public void SelectLeftSteal()
    {
        if (stealIndex <= 0) return;

        stealButtons[stealIndex].gameObject.SetActive(false);

        stealIndex--;

        stealButtons[stealIndex].gameObject.SetActive(true);

        feedbackManager.UpdateItemMenu(stolen[stealButtons.Count - (stealIndex + 1)].Item2);
    }

    public void SelectRightItem()
    {
        if (itemIndex >= itemButtons.Count - 1) return;

        itemButtons[itemIndex].gameObject.SetActive(false);

        itemIndex++;

        itemButtons[itemIndex].gameObject.SetActive(true);

        feedbackManager.UpdateItemMenu(playerFood[itemIndex]);
    }

    public void SelectRightSteal()
    {
        if (stealIndex >= stealButtons.Count - 1) return;

        stealButtons[stealIndex].gameObject.SetActive(false);

        stealIndex++;

        stealButtons[stealIndex].gameObject.SetActive(true);

        feedbackManager.UpdateItemMenu(stolen[stealButtons.Count - (stealIndex + 1)].Item2);
    }

    private void BackFromItem()
    {
        currentMenu--;
        backButton.SetActive(false);
        ToggleMenu(itemContainer, initialContainer, initialButton, 2);
        feedbackManager.CloseMenus();

    }

    public void BackFromFlavor()
    {
        currentMenu--;
        ToggleMenu(flavorContainer, itemContainer, itemButtons[itemIndex], 3);
        feedbackManager.BackFromTotalMenu();
    }

    private void BackFromTarget()
    {   
        ColorSwitcher.instance.ResetFlavor();

        currentMenu--;
        ToggleMenu(targetContainer, flavorContainer, flavorButton, 4);
    }

    private void BackFromSteal()
    {
        currentMenu = 0;
        backButton.SetActive(false);
        ToggleMenu(stealBackContainer, initialContainer, initialButton, 5);
        feedbackManager.CloseMenus();
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

        ToggleMenu(itemContainer, flavorContainer, flavorButton, 6);

        feedbackManager.GoToTotalMenu();
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

            for (int i = 0; i < targetButtons.Count; i++)
            {
                if (targetButtons[i].gameObject.activeSelf)
                {
                    Utility.SetActiveButton(targetButtons[i]);
                    break;
                }

            }

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
        StealFood(element);
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
        EndTurn();
    }

    private void SendHealAction()
    {
        battleManager.SetHealAction(selectedAction, flavor);
        EndTurn();
    }

    private void SendStealAction(Food food)
    {
        battleManager.SetStealAction(characterToAttack, food);
        EndTurn();
    }

    private void EndTurn()
    {
        backButton.SetActive(false);
        feedbackManager.CloseMenus();
        actionText.gameObject.SetActive(true);
    }

    public void SetText(string s)
    {
        actionText.text = s;
    }

    public void DefeatedEnemy(EnemyBattle enemy)
    {
        // get the enemy's index in enemy list
        int enemyIndex = enemies.IndexOf(enemy);

        RemoveFoods(enemy);

        // remove button associated with enemy
        Button targetButton = targetContainer.transform.GetChild(enemyIndex).GetComponent<Button>();
        // targetButtons.Remove(targetButton);
        // Destroy(targetButton.gameObject);
        targetButton.gameObject.SetActive(false);

        // set enemy's health ui to defeated text
        // eHealthContainer.transform.GetChild(enemyIndex).GetComponent<TextMeshProUGUI>().text = enemy.CharacterName + " is defeated";
    }

    public void ClearUI()
    {
        // clear all lists and ui elements from any previous battles
        targetButtons.Clear();
        itemButtons.Clear();
        playerFood.Clear();
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
        itemIndex = stealIndex = 0;

        // feedbackManager.gameObject.SetActive(false);
    }

    public void EnemyStolen(EnemyBattle enemy)
    {
        foreach (Food food in enemy.GetRecentlyStolenItems())
            stolen.Add((enemy, food));

        enemy.ClearRecentItems();
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
            RemoveStolenFood(s);
    }

    private void RemoveStolenFood((EnemyBattle, Food) food)
    {
        stolen.Remove(food);

        stealIndex = 0;
    }

    private void StealFood((EnemyBattle, Food) foodElement)
    {
        RemoveStolenFood(foodElement);

        if (playerFood.Contains(foodElement.Item2)) return;
        
        playerFood.Clear();
        itemButtons.Clear();
        Transform container = itemContainer.transform.GetChild(0);

        for (int j = 0; j < container.childCount; j++)
        {
            Destroy(container.GetChild(j).gameObject);
        }

        SetActions();

        if (playerFood.Count == 0) 
        {
            attackButtonSmall.gameObject.SetActive(true);
            healButtonSmall.gameObject.SetActive(true);
        }
    }

    private void ToggleMenu(GameObject close, GameObject open, Button activeButton, int audioIndex = 0, string text = "")
    {
        PlayAudio(audioIndex);
        close.SetActive(false);
        open.SetActive(true);
        Utility.SetActiveButton(activeButton);
        // feedbackManager.SwitchMenu(currentMenu);
        SetText(text);
    }

    private void PlayAudio(int i)
    {
        AudioManager.instance.PlayUIClip(i);
    }
}