using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField]
    [Header("AudioClips")]
    public AudioClip hitSound;
    public AudioClip goalSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playHitSound()
    {
        audioSource.PlayOneShot(hitSound,25);
    }

    
    public void playGoalSound()
    {
        audioSource.PlayOneShot(goalSound);
    }
    
}
