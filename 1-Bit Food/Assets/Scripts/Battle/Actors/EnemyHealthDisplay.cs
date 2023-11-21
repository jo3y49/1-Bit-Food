using UnityEngine;
using TMPro;

public class EnemyHealthDisplay : MonoBehaviour {
    [SerializeField] private EnemyBattle enemy;
    [SerializeField] private TextMeshProUGUI textPrefab;

    private TextMeshProUGUI healthText;

    private void Start() {
        Transform canvas = GameObject.FindGameObjectWithTag("Canvas").transform;

        healthText = Instantiate(textPrefab, canvas);

        healthText.transform.position = transform.position;
        healthText.fontSize = enemy.transform.localScale.x * 8;
        healthText.text = enemy.health.ToString();

    }

    public void SetHealth(int health)
    {
        healthText.text = health.ToString();
    }
}