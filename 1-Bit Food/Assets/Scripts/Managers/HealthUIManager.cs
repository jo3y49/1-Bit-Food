using UnityEngine;

public class HealthUIManager : MonoBehaviour {
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject heartContainer;

    private GameObject[] hearts;
    private int currentHealthIndex;
    
    public void SetHealth()
    {
        int maxHealth = GameManager.instance.GetMaxHealth();
        int health = GameManager.instance.GetHealth();

        hearts = new GameObject[maxHealth];

        currentHealthIndex = maxHealth - 1;

        int children = heartContainer.transform.childCount;

        
        for (int i = 0; i < children; i++)
        {
            Destroy(heartContainer.transform.GetChild(i).gameObject);
        }
        

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject image = Instantiate(heartPrefab, heartContainer.transform);
            hearts[i] = image;
        }

        LoseHealth(maxHealth - health);      
    }

    // public void UpdateHealth()
    // {
    //     int targetIndex = playerBattle.health - 1;

    //     if (targetIndex > currentHealthIndex) AddHealth(targetIndex - currentHealthIndex);

    //     else if (targetIndex < currentHealthIndex) LoseHealth(currentHealthIndex - targetIndex);
    // }

    public void AddHealth(int heal = 1)
    {
        int targetIndex = currentHealthIndex + heal;

        if (targetIndex >= hearts.Length) targetIndex = hearts.Length - 1;

        for (int i = currentHealthIndex; i < targetIndex; i++)
        {
            hearts[i + 1].SetActive(true);
        }

        currentHealthIndex = targetIndex;
    }

    public void LoseHealth(int damage = 1)
    {
        int targetIndex = currentHealthIndex - damage;

        if (targetIndex < -1) targetIndex = -1;

        for (int i = currentHealthIndex; i > targetIndex; i--)
        {
            hearts[i].SetActive(false);
        }

        currentHealthIndex = targetIndex;
    }
}