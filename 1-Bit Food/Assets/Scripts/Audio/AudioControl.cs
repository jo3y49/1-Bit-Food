using System.Collections;
using UnityEngine;

public class AudioControl : MonoBehaviour {
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip initial, loop;

    private void Awake() {
        audioSource.loop = false;
        audioSource.clip = initial;
        audioSource.Play();

        StartCoroutine(WaitForLoop());
    }

    private void Update() {
        if (audioSource.isPlaying && Time.timeScale == 0) audioSource.Pause();

        else if (!audioSource.isPlaying && Time.timeScale == 1) audioSource.Play();
    }

    private IEnumerator WaitForLoop()
    {
        while(audioSource.isPlaying)
        {
            yield return null;
        }

        audioSource.loop = true;
        audioSource.clip = loop;
        audioSource.Play();
    }
}