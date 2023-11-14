using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PlayerData
{
    public int money = 120;
    public int score = 0;
    public List<int> foodUses;

    public PlayerData(int dessertLength)
    {
        foodUses = Enumerable.Repeat(10, dessertLength).ToList();
    }

    public PlayerData(List<int> dessertUses)
    {
        this.foodUses = dessertUses;
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