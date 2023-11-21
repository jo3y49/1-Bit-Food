using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PlayerData
{
    public readonly int maxHealth = 27;
    public int health;
    public int money = 120;
    public int score = 0;
    public readonly int startFoodAmount = 1;
    public readonly int startFlavorAmount = 3;
    public List<int> foodUses;
    public List<int> flavorUses;
    public readonly int maxFoodUses = 99;

    public PlayerData(int dessertLength)
    {
        health = maxHealth;
        foodUses = Enumerable.Repeat(startFoodAmount, dessertLength).ToList();
        flavorUses = Enumerable.Repeat(startFlavorAmount, 5).ToList();
    }

    public PlayerData(List<int> dessertUses)
    {
        health = maxHealth;
        this.foodUses = dessertUses;
        flavorUses = Enumerable.Repeat(startFlavorAmount, 5).ToList();
    }
}

[System.Serializable]
public class WorldData
{
    public int currentScene = 1;
}

[System.Serializable]
public class SettingsData
{
    
}

[System.Serializable]
public class GameData
{
    public PlayerData playerData;
    public WorldData worldData = new();
    public SettingsData settingsData = new();

    public GameData(int dessertLength)
    {
        playerData = new(dessertLength);
    }

    public GameData NewGame()
    {
        playerData = new(playerData.foodUses);
        worldData = new();

        return this;
    }
}