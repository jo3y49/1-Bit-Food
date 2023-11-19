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
        

        else if (!audioSource.isPlaying && Time.timeScale == 1) PauseAudio(false);
    }

    private IEnumerator WaitForLoop()
    {
        while(audioSource.isPlaying && !pause)
        {
            yield return null;
        }

        audioSource.loop = true;
        audioSource.clip = loop;
        audioSource.Play();
    }

    public void PauseAudio(bool b)
    {
        pause = b;

        if (b)  audioSource.Pause();
        

        else audioSource.Play();
    }
}