using UnityEngine;

public class ToggleAudioButtons : MonoBehaviour {

    public GameObject audioPlayer;
    private AudioSource audioSource;
    private AudioControl audioControl;

    private void Start() {
        audioPlayer = GameObject.FindGameObjectWithTag("MainCamera");
        audioSource = audioPlayer.GetComponent<AudioSource>();
        audioControl = audioPlayer.GetComponent<AudioControl>();
        
    }

    public void TogglePause()
    {
        PauseManager.TogglePause();
    }

    public void ToggleAudio()
    {
        audioControl.PauseAudioButton();
    }

    public void ToggleVolume()
    {
        float lowest = .15f;
        float low = .35f;
        float medium = .65f;
        float high = 1f;

        float volume = audioSource.volume;

        if (volume == high)
            volume = medium;
        else if (volume >= medium)
            volume = low;
        else if (volume >= low)
            volume = lowest;
        else 
            volume = high;

        audioSource.volume = volume;
    }
    
}