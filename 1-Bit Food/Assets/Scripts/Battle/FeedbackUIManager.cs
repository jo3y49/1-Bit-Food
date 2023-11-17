using System.Collections.Generic;
using UnityEngine;

public class FeedbackUIManager : MonoBehaviour {
    [SerializeField] private GameObject feedbackContainer;
    private GameObject activeMenu;
    private List<GameObject> menus = new();

    private Food activeFood;
    private Flavor activeFlavor;

    private void Awake() {
        for (int i = 0; i < feedbackContainer.transform.childCount; i++)
        {
            menus.Add(feedbackContainer.transform.GetChild(i).gameObject);
        }

        activeFood = FoodList.GetInstance().GetFoods()[0];
        // activeFlavor = 

        // feedbackContainer.SetActive(false);
    }

    public void UpdateItemMenu(Food food)
    {
        activeFood = food;
    }

    public void UpdateFlavorMenu(Flavor flavor)
    {
        activeFlavor = flavor;
    }

    public void SetResultMenu()
    {
        
    }

    private void SetActiveMenu(GameObject menu)
    {
        activeMenu.SetActive(false);
        menu.SetActive(true);
        activeMenu = menu;
    }

    public void SwitchMenu(int index)
    {
        SetActiveMenu(menus[index]);
    }
}