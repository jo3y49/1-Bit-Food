using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class StoreManager : MonoBehaviour {
    [SerializeField] protected TextMeshProUGUI inventoryAmountText, inventoryCapText;

    protected Food[] foods;
    protected Food selectedFood;

    protected int inventoryAmount;

    protected int inventoryCap;

    private void Awake() {
        gameObject.SetActive(false);
    }

    protected virtual void OnEnable() {
        foods = FoodList.GetInstance().GetFoods();

        InitializeInventory();
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
        GameManager.instance.AddFoodUse(FoodList.GetInstance().GetFoodIndex(food));
        inventoryAmount++;

        inventoryAmountText.text = inventoryAmount.ToString("D2");
    }
}