using UnityEngine;

public class ToggleAudio : MonoBehaviour {

    public AudioControl audioControl;

    private bool pause = false;

    public void TogglePause()
    {
        pause = !pause;

        audioControl.PauseAudio(pause);
    }
    
}