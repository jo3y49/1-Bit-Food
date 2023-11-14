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

    public List<int> GetFoodUses()
    {
        return gameData.playerData.foodUses;
    }

    public void AddFoodUse(int index, int uses = 1)
    {
        List<int> desserts = gameData.playerData.foodUses;

        if (index >= desserts.Count) return;

        desserts[index] += uses;
    }

    public void SetFoodUses(List<int> uses)
    {
        gameData.playerData.foodUses = uses;
    }

    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeGameData(new GameData(Resources.LoadAll<Food>("CombatFood").Length));
        }
        else
        {
            Destroy(gameObject);
        }
    }
}