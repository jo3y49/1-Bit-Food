using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class PauseManager : MonoBehaviour {
    private static PauseManager instance;

    public static event Action<bool> PauseEvent;

    private void Awake() {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }

       
    }

    public static void TogglePause() {
        bool resume = Time.timeScale == 0;

        if (resume)
        {
            Time.timeScale = 1;
            PauseEvent?.Invoke(false);
        }
        else
        {
            Time.timeScale = 0;
            PauseEvent?.Invoke(true);
        }
    }

    // public void Quit() {
    //     Time.timeScale = 1;
    //     actions.Gameplay.Disable();
    //     SceneManager.LoadScene(mainMenuSceneIndex);
    // }
}