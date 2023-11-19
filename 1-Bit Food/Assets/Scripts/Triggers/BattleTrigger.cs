using UnityEngine;

public class BattleTrigger : MonoBehaviour {
    private WorldManager worldManager;

    private void Start() {
        worldManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WorldManager>();
    }
    protected virtual void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            worldManager.EncounterEnemy();
        }

        Destroy(gameObject);
    }
}