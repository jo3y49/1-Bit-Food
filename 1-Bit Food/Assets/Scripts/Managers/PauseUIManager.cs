using UnityEngine;

public class PauseUIManager : MonoBehaviour {
    [SerializeField] private GameObject pauseUI;

    private void Awake() {
        
    }

    private void OnEnable() {
        PauseManager.PauseEvent += ToggleUI;

        pauseUI.SetActive(false);
    }

    private void OnDisable() {
        PauseManager.PauseEvent -= ToggleUI;
    }

    private void ToggleUI(bool b)
    {
        pauseUI.SetActive(b);
    }
}