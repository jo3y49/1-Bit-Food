using TMPro;
using UnityEngine;

public class DebugRoomManager : WorldManager {
    public Vector2[] spawnLocations;
    private GameObject[] activeEnemies;
    public GameObject enemyPrefab;

    public TextMeshProUGUI tutText;

    private void Awake() {
        activeEnemies = new GameObject[spawnLocations.Length];
    }
    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            for (int i = 0; i < spawnLocations.Length; i++)
            {
                if (activeEnemies[i] == null)
                {
                    GameObject enemy = Instantiate(enemyPrefab);
                    enemy.transform.position = spawnLocations[i];
                    activeEnemies[i] = enemy;
                    break;
                }
            }
        }
    }

    public override void EncounterEnemy(GameObject enemy)
    {
        base.EncounterEnemy(enemy);

        tutText.enabled = false;
    }

    public override void WinBattle()
    {
        base.WinBattle();

        tutText.enabled = true;
    }
}