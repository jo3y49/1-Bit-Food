using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class StoreManager : MonoBehaviour {
    [SerializeField] protected GameObject foodContainer;
    [SerializeField] protected GameObject buttonPrefab;
    [SerializeField] protected GameObject popUp;
    [SerializeField] protected TextMeshProUGUI confirmationMessage;

    protected Food[] foods;
    protected Food selectedFood;

    protected readonly int level = 0;

    protected virtual void Start() {
        popUp.SetActive(false);
        foods = FoodList.GetInstance().GetFoods();
    }

    public virtual void Confirm()
    {
        AudioManager.instance.PlayUIClip(1);

        Transaction();

        GameManager.instance.AddFoodUse(FoodList.GetInstance().GetFoodIndex(selectedFood));

        popUp.SetActive(false);
    }

    protected abstract void Transaction();

    public virtual void Cancel()
    {
        AudioManager.instance.PlayUIClip(2);

        popUp.SetActive(false);
    }

    public virtual void Return()
    {
        AudioManager.instance.PlayUIClip(3);
        SceneManager.LoadScene(level);
    }
}