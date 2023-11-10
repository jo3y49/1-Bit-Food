using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PlayerData
{
    
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
    public PlayerData playerData = new();
    public WorldData worldData = new();
    public SettingsData settingsData = new();

    public GameData NewGame()
    {
        playerData = new();
        worldData = new();

        return this;
    }
}