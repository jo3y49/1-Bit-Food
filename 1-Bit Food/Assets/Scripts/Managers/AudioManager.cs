using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {
    public static AudioManager instance;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] uiSounds;
    private void Awake() {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    public void PlayUIClip(int i)
    {
        if (uiSounds.Length > i) PlayClip(uiSounds[i]);

        else PlayClip(uiSounds[0]);
    }

    private void PlayClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}