using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugRoomManager : WorldManager {
    public Vector2[] spawnLocations;
    private GameObject[] activeEnemies;
    public GameObject enemyPrefab;

    public TextMeshProUGUI tutText;

    private int shopScene = 2;

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

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(shopScene);
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