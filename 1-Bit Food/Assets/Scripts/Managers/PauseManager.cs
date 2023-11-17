using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class PauseManager : MonoBehaviour {
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private Button pauseButton, quitButton;
    [SerializeField] private Sprite pause, resume;

    public static event Action<bool> pauseEvent;

    private InputActions actions;
    
    private int mainMenuSceneIndex = 0;

    private void Awake() {
        actions = new InputActions();

        pauseUI.SetActive(false);
    }

    public void TogglePause() {
        if (pauseUI.activeSelf)
            Resume();
        else
            Pause();
    }

    public void Resume() {
        Time.timeScale = 1; 
        pauseUI.SetActive(false);
        pauseEvent(false);
        pauseButton.image.sprite = pause;
    }

    private void Pause() {
        Time.timeScale = 0; 
        pauseUI.SetActive(true);
        pauseEvent(true);
        Utility.SetActiveButton(pauseButton);
        pauseButton.image.sprite = resume;
    }

    // public void Quit() {
    //     Time.timeScale = 1;
    //     actions.Gameplay.Disable();
    //     SceneManager.LoadScene(mainMenuSceneIndex);
    // }
}