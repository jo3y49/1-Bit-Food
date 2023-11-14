using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PlayerData
{
    public int money = 120;
    public int score = 0;
    public List<int> dessertUses;

    public PlayerData(int dessertLength)
    {
        dessertUses = Enumerable.Repeat(10, dessertLength).ToList();
    }

    public PlayerData(List<int> dessertUses)
    {
        this.dessertUses = dessertUses;
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
        playerData = new(playerData.dessertUses);
        worldData = new();

        return this;
    }
}