using UnityEngine;

public class ShopTrigger : MonoBehaviour {
    [SerializeField] private GameObject storeManager;
    [SerializeField] private PlayerMovement player;
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        player.TogglePause(true);
        storeManager.SetActive(true);
    }
}