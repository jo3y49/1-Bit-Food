using System.Collections;
using UnityEngine;

public class AudioControl : MonoBehaviour {
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip initial, loop;

    private float initalLength;

    private void Awake() {
        audioSource.loop = false;
        audioSource.clip = initial;
        audioSource.Play();

        StartCoroutine(WaitForLoop());
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