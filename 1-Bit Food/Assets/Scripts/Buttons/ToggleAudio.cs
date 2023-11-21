using UnityEngine;

public class ToggleAudio : MonoBehaviour {

    public GameObject audioPlayer;
    private AudioControl audioControl;
    private AudioSource audioSource;

    private void Start() {
        audioPlayer = GameObject.FindGameObjectWithTag("MainCamera");
        audioControl = audioPlayer.GetComponent<AudioControl>();
        audioSource = audioPlayer.GetComponent<AudioSource>();
    }

    public void TogglePause()
    {
        if (Time.timeScale == 0) return;

        audioControl.PauseAudio();
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