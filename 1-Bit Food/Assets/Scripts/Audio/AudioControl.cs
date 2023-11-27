using System.Collections;
using UnityEngine;

public class AudioControl : MonoBehaviour {
    public static AudioControl instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip initial, loop;

    private float audioTime = 0;

    private bool looped = false;

    private bool pause = false;

    private bool initialPause = false;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource.loop = false;
            audioSource.clip = initial;
            audioSource.Play();

            PauseManager.PauseEvent += PauseAudio;

            StartCoroutine(WaitForLoop());
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    // private void Update() {
    //     if (audioSource.isPlaying && Time.timeScale == 0) PauseAudio(true);
        

    //     else if (!audioSource.isPlaying && Time.timeScale == 1 && !pause) PauseAudio(false);
    // }

    public void PauseAudio(bool b)
    {
        pause = b;

        ToggleAudio(b);
    }

    private void ToggleAudio(bool b)
    {
        if (b)
        {
            audioTime = audioSource.time;
            audioSource.Pause();
        } 
        
        else 
        {
            audioSource.time = audioTime;
            audioSource.Play();
        }
    }

    public void PauseAudioButton()
    {
        audioSource.mute = !audioSource.mute;
    }

    private IEnumerator WaitForLoop()
    {
        while(audioSource.isPlaying && !pause)
        {
            yield return null;
        }

        if (pause)
        {
            audioSource.Pause();
            StartCoroutine(WaitToResume());
            
        } else
        {
            audioSource.loop = true;
            audioSource.clip = loop;
            audioTime = 0;
            audioSource.time = 0;
            audioSource.Play();
        }
    }

    private IEnumerator WaitToResume()
    {
        while (pause)
        {
            yield return null;
        }

        StartCoroutine(WaitForLoop());
    }

    private void OnApplicationFocus(bool focus) {
        if (!focus)
        {
            if (pause) initialPause = true;

            else PauseAudio(true);
        } else
        {
            if (initialPause) initialPause = false;

            else PauseAudio(false);
        }
    }
}