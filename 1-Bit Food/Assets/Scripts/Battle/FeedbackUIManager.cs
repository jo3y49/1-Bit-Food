using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackUIManager : MonoBehaviour {
    [SerializeField] private GameObject feedbackContainer, itemMenu, totalMenu, plus, flavorOutline;
    [SerializeField] private Image actionImage, itemImage1, itemImage2, flavorImage;
    [SerializeField] private TextMeshProUGUI itemNameText, itemDamageText, itemHealText, totalText;

    [Header("Sprites")]
    [SerializeField] private Sprite attackSprite;
    [SerializeField] private Sprite healSprite;

    private bool attack;
    private Food activeFood;
    private Flavor activeFlavor;

    private int itemNumber, totalNumber;

    private void Awake() {
        activeFood = FoodList.GetInstance().GetFoods()[0];
        activeFlavor = null;

        CloseMenus();
    }

    public void GoToItemMenu(bool attack, Food food)
    {
        this.attack = attack;

        if (attack) actionImage.sprite = attackSprite;

        else actionImage.sprite = healSprite;

        UpdateItemMenu(food);

        itemMenu.SetActive(true);
    }

    public void UpdateItemMenu(Food food)
    {
        activeFood = food;

        itemNameText.text = food.name;
        itemImage1.sprite = food.sprite;
        itemDamageText.text = food.damage.ToString();
        itemHealText.text = food.heal.ToString();
    }

    public void GoToTotalMenu()
    {
        if (attack) itemNumber = activeFood.damage;

        else itemNumber = activeFood.heal;

        totalNumber = itemNumber;
        totalText.text = totalNumber.ToString();

        itemImage2.sprite = activeFood.sprite;

        itemMenu.SetActive(false);
        totalMenu.SetActive(true);
    }

    public void UpdateTotalMenu(Flavor flavor)
    {
        activeFlavor = flavor;

        if (activeFlavor == null)
        {
            flavorImage.gameObject.SetActive(false);
            plus.SetActive(false);
            flavorOutline.SetActive(false);

            totalNumber = itemNumber;
        }
        else
        {
            flavorImage.gameObject.SetActive(true);
            plus.SetActive(true);
            flavorOutline.SetActive(true);

            totalNumber = itemNumber + flavor.bonus;
            flavorImage.sprite = flavor.sprite;
        }

        totalText.text = totalNumber.ToString();
    }

    public void BackFromTotalMenu()
    {
        totalMenu.SetActive(false);
        itemMenu.SetActive(true);
    }

    public void CloseAllMenus()
    {   
        feedbackContainer.SetActive(false);
        CloseMenus();
    }

    public void CloseMenus()
    {
        itemMenu.SetActive(false);
        totalMenu.SetActive(false);
        plus.SetActive(false);
        flavorOutline.SetActive(false);
        flavorImage.gameObject.SetActive(false);
    }
}