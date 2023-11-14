using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour 
{
    [SerializeField] private GameObject dessertContainer;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject popUp;
    [SerializeField] private TextMeshProUGUI moneyText, confirmationMessage;

    private int money;

    private Dessert[] desserts;
    private Dessert selectedDessert;

    private int level = 0;

    private void Start() {
        SetMoney(GameManager.instance.GetPlayerMoney());
        popUp.SetActive(false);
        desserts = DessertList.GetInstance().GetDesserts();

        foreach (Dessert dessert in desserts)
        {
            GameObject button = Instantiate(buttonPrefab, dessertContainer.transform);
            button.GetComponentInChildren<Image>().sprite = dessert.sprite;
            button.GetComponentInChildren<TextMeshProUGUI>().text = $"{dessert.name}: {dessert.price} coins";

            button.GetComponent<Button>().onClick.AddListener(() => Buy(dessert));
        }
    }

    private void Buy(Dessert dessert)
    {
        if (money >= dessert.price && !popUp.activeSelf)
        {
            selectedDessert = dessert;

            confirmationMessage.text = $"Buy {dessert.name} for {dessert.price}?";

            popUp.SetActive(true);
        }
    }

    public void Confirm()
    {
        GameManager.instance.AddPlayerMoney(-selectedDessert.price);
        SetMoney(GameManager.instance.GetPlayerMoney());

        GameManager.instance.AddDessertUse(DessertList.GetInstance().GetDessertIndex(selectedDessert));

        popUp.SetActive(false);
    }

    public void Cancel()
    {
        popUp.SetActive(false);
    }

    private void SetMoney(int money)
    {
        this.money = money;

        moneyText.text = "Coins: " + money;
    }

    public void Return()
    {
        SceneManager.LoadScene(level);
    }
}