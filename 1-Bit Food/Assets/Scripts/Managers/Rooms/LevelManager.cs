using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : WorldManager {
    [SerializeField] private Image heart;
    [SerializeField] private GameObject heartContainer;

    protected override void Start() {
        base.Start();
        
        SetHealth();
    }

    private void SetHealth()
    {
        for (int i = 0; i < playerBattle.health; i++)
        {
            Instantiate(heart, heartContainer.transform);
        }
    }
}