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

    public List<int> GetDessertUses()
    {
        return gameData.playerData.dessertUses;
    }

    public void AddDessertUse(int index, int uses = 1)
    {
        List<int> desserts = gameData.playerData.dessertUses;

        if (index >= desserts.Count) return;

        desserts[index] += uses;
    }

    public void SetDessertUses(List<int> uses)
    {
        gameData.playerData.dessertUses = uses;
    }

    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeGameData(new GameData(Resources.LoadAll<Dessert>("Desserts").Length));
        }
        else
        {
            Destroy(gameObject);
        }
    }
}