using TMPro;
using UnityEngine;

public abstract class StoreManager : MonoBehaviour {
    [SerializeField] protected TextMeshProUGUI inventoryAmountText, inventoryCapText;
    [SerializeField] protected TextMeshProUGUI moneyText;
    [SerializeField] protected FeedbackUIManager feedbackUIManager;

    protected int money;

    protected Food[] foods;
    protected Food selectedFood;

    protected int inventoryAmount;

    protected int inventoryCap;

    private InputActions actions;

    private void Awake() {
        actions = new InputActions();

        gameObject.SetActive(false);
    }

    protected virtual void OnEnable() {

        if (GameManager.instance == null) return;

        SetMoney(GameManager.instance.GetPlayerMoney());

        foods = FoodList.GetInstance().GetFoods();

        InitializeInventory();

        actions.Gameplay.Enable();

        actions.Gameplay.Pause.performed += context => gameObject.SetActive(false);
    }

    protected virtual void OnDisable() {
        actions.Gameplay.Pause.performed -= context => gameObject.SetActive(false);

        actions.Gameplay.Disable();

        feedbackUIManager.CloseMenus();

        PlayerMovement playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        playerMovement.TogglePause(false);
    }

    public virtual void Confirm()
    {
        AudioManager.instance.PlayUIClip(1);

        Transaction();

        SetInventory(selectedFood);
    }

    protected abstract void Transaction();

    public virtual void Return()
    {
        AudioManager.instance.PlayUIClip(3);
    }

    private void InitializeInventory()
    {
        inventoryCap = GameManager.instance.GetMaxFoodUses();
        inventoryAmount = GameManager.instance.GetFoodUses();

        inventoryCapText.text = inventoryCap.ToString();
        inventoryAmountText.text = inventoryAmount.ToString();
    }

    public virtual void SetInventory(Food food)
    {
        GameManager.instance.AddFoodUse(food.index);
        inventoryAmount++;

        inventoryAmountText.text = inventoryAmount.ToString("D2");
    }

    protected virtual void SetMoney(int money)
    {
        this.money = money;

        moneyText.text = " : " + money;
    }

    public virtual void FoodHighlight(Food food)
    {
        selectedFood = food;

        feedbackUIManager.UpdateItemMenu(food);
    }

    public virtual void FoodDeHighlight()
    {
        feedbackUIManager.CloseMenus();
    }
}