public class ShopManager : StoreManager 
{
    public void Buy(Ingredient food)
    {
        AudioManager.instance.PlayUIClip(0);

        if (money < food.price || !GameManager.instance.OpenInventory()) return;

        selectedFood = food;

        Confirm();

    }

    protected override void Transaction()
    {
        GameManager.instance.AddPlayerMoney(-selectedFood.price);
        SetMoney(GameManager.instance.GetPlayerMoney());
    }
}