using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : StoreManager 
{
    [SerializeField] private TextMeshProUGUI moneyText;

    private int money;

    protected override void OnEnable() {
        base.OnEnable();

        SetMoney(GameManager.instance.GetPlayerMoney());


    }

    public void Buy(Ingredient food)
    {
        if (money < food.price || popUp.activeSelf || !GameManager.instance.OpenInventory()) return;
        
        AudioManager.instance.PlayUIClip(0);

        selectedFood = food;

        confirmationMessage.text = $"Buy {food.name} for {food.price} coins?";

        popUp.SetActive(true);
        

    }

    protected override void Transaction()
    {
        GameManager.instance.AddPlayerMoney(-selectedFood.price);
        SetMoney(GameManager.instance.GetPlayerMoney());
    }

    private void SetMoney(int money)
    {
        this.money = money;

        moneyText.text = " : " + money;
    }
}