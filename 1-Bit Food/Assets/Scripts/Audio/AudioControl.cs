using System.Collections;
using UnityEngine;

public class AudioControl : MonoBehaviour {
    public static AudioControl instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip initial, loop;

    private bool pause = false;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource.loop = false;
            audioSource.clip = initial;
            audioSource.Play();

            StartCoroutine(WaitForLoop());
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (audioSource.isPlaying && Time.timeScale == 0) PauseAudio(true);
        

        else if (!audioSource.isPlaying && Time.timeScale == 1 && !pause) PauseAudio(false);
    }

    public void PauseAudio(bool b)
    {
        if (b) audioSource.Pause();
        
        else audioSource.Play();
    }

    public void PauseAudio()
    {
        pause = !pause;
        PauseAudio(pause);
    }

    private IEnumerator WaitForLoop()
    {
        while(audioSource.isPlaying && !pause && Time.timeScale == 1)
        {
            yield return null;
        }

        if (pause || Time.timeScale == 0)
        {
            audioSource.Pause();
            StartCoroutine(WaitToResume());
            
        } else
        {
            audioSource.loop = true;
            audioSource.clip = loop;
            audioSource.Play();
        }
    }

    private IEnumerator WaitToResume()
    {
        while (!audioSource.isPlaying)
        {
            yield return null;
        }

        StartCoroutine(WaitForLoop());
    }
}