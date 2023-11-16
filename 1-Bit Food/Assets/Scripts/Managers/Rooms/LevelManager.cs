using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : WorldManager {
    [SerializeField] private GameObject heart;
    [SerializeField] private GameObject heartContainer;

    private GameObject[] hearts;

    public int currentHealthIndex;

    protected override void Start() {
        base.Start();

        hearts = new GameObject[playerBattle.maxHealth];

        currentHealthIndex = playerBattle.health - 1;
        
        SetHealth();
    }

    private void SetHealth()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            GameObject image = Instantiate(heart, heartContainer.transform);
            hearts[i] = image;
        }

        LoseHealth(playerBattle.maxHealth - playerBattle.health);

        StartCoroutine(Test());        
    }

    public void AddHealth(int heal = 1)
    {
        int targetIndex = currentHealthIndex + heal;

        for (int i = currentHealthIndex; i < targetIndex; i++)
        {
            hearts[i + 1].SetActive(true);
        }

        currentHealthIndex = targetIndex;
    }

    public void LoseHealth(int damage = 1)
    {
        Debug.Log(currentHealthIndex);

        int targetIndex = currentHealthIndex - damage;

        for (int i = currentHealthIndex; i > targetIndex; i--)
        {
            Debug.Log(i);
            hearts[i].SetActive(false);
        }

        currentHealthIndex = targetIndex;
    }

    private IEnumerator Test()
    {
        while (currentHealthIndex > 0)
        {
            yield return new WaitForSeconds(2);

            LoseHealth(3);

            yield return new WaitForSeconds(1);

            AddHealth(2);
        }
    }
}