using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxManager : MonoBehaviour
{

    public AudioSource audioSource;

    [Header("Audio Clips")]
    public AudioClip hitSound;
    public AudioClip goalSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
