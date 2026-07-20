using UnityEngine;
using UnityEngine.Audio;

public class audioManager : MonoBehaviour
{
    public static audioManager Instance;

    public AudioSource audioSource;

    [Header("Audio Clips")]
    public AudioClip hitSound;
    public AudioClip goalSound;

    public AudioMixer mixer;

    private void Awake()
    {
        // If another AudioManager already exists, destroy this one.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
    }

    public void playHitSound()
    {
        audioSource.PlayOneShot(hitSound, 1f);
    }

    public void playGoalSound()
    {
        audioSource.PlayOneShot(goalSound);
    }
}