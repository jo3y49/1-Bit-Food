using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour 
{
    [SerializeField] private GameObject foodContainer;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject popUp;
    [SerializeField] private TextMeshProUGUI moneyText, confirmationMessage;

    private int money;

    private Food[] foods;
    private Food selectedFood;

    private int level = 0;

    private void Start() {
        SetMoney(GameManager.instance.GetPlayerMoney());
        popUp.SetActive(false);
        foods = FoodList.GetInstance().GetFoods();

        foreach (Food food in foods)
        {
            GameObject button = Instantiate(buttonPrefab, foodContainer.transform);
            button.GetComponentInChildren<Image>().sprite = food.sprite;
            button.GetComponentInChildren<TextMeshProUGUI>().text = $"{food.name}: {food.price} coins";

            button.GetComponent<Button>().onClick.AddListener(() => Buy(food));
        }
    }

    private void Buy(Food food)
    {
        if (money >= food.price && !popUp.activeSelf)
        {
            AudioManager.instance.PlayUIClip(0);

            selectedFood = food;

            confirmationMessage.text = $"Buy {food.name} for {food.price}?";

            popUp.SetActive(true);
        }

    }

    public void Confirm()
    {
        AudioManager.instance.PlayUIClip(1);

        GameManager.instance.AddPlayerMoney(-selectedFood.price);
        SetMoney(GameManager.instance.GetPlayerMoney());

        GameManager.instance.AddFoodUse(FoodList.GetInstance().GetFoodIndex(selectedFood));

        popUp.SetActive(false);
    }

    public void Cancel()
    {
        AudioManager.instance.PlayUIClip(2);

        popUp.SetActive(false);
    }

    private void SetMoney(int money)
    {
        this.money = money;

        moneyText.text = "Coins: " + money;
    }

    public void Return()
    {
        AudioManager.instance.PlayUIClip(3);
        SceneManager.LoadScene(level);
    }
}