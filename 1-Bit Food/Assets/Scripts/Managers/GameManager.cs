using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    private GameData gameData;
    private GameObject player;

    public void InitializeGameData(GameData data)
    {
        gameData = data;
    }

    public void SaveGame()
    {
        SaveSystem.SaveGameData(gameData);
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public void AddPlayerMoney(int money)
    {
        gameData.playerData.money += money;
    }

    public int GetPlayerMoney()
    {
        return gameData.playerData.money;
    }

    public void AddPlayerScore(int score)
    {
        gameData.playerData.score += score;
    }

    public void SetCurrentScene(int sceneIndex)
    {
        gameData.worldData.currentScene = sceneIndex;
    }

    public List<int> GetFoodIntUsesList()
    {
        return gameData.playerData.foodUses;
    }

    public int GetFoodUses()
    {
        return Utility.CountListOfInts(gameData.playerData.foodUses);
    }

    public int GetFoodUses(int i)
    {
        return gameData.playerData.foodUses[i];
    }

    public bool AddFoodUse(int index, int uses = 1)
    {
        List<int> desserts = gameData.playerData.foodUses;

        if (index >= desserts.Count || GetFoodUses() + uses > gameData.playerData.maxFoodUses) return false;

        desserts[index] += uses;

        return true;
    }

    public bool OpenInventory()
    {
        return GetFoodUses() < gameData.playerData.maxFoodUses;
    }

    public void SetFoodUses(List<int> uses)
    {
        if (Utility.CountListOfInts(uses) <= gameData.playerData.maxFoodUses)
            gameData.playerData.foodUses = uses;

        Debug.Log(Utility.CountListOfInts(uses) + " " + gameData.playerData.maxFoodUses);
    }

    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeGameData(new GameData(Resources.LoadAll<Food>("").Length));
        }
        else
        {
            Destroy(gameObject);
        }
    }
}