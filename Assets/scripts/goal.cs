using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goal : MonoBehaviour
{
    public scoreKeeper scoreKeeper;
    public GameObject audioManager;
    public GameObject player;
    public GameObject opponent; 
    public sfxManager sfxManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindWithTag("audioManager");
        scoreKeeper = FindObjectOfType<scoreKeeper>();
        if (!scoreKeeper) Debug.Log("no scorekeeper at the goal script");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("puck"))
        {
            //audioManager.GetComponent<audioManager>().playGoalSound();
            sfxManager.playGoalSound();
            scoreKeeper.pauseTimer();
            //disable player and opponent movement
            player.GetComponent<player>().disableInput();
            opponent.GetComponent<enemyAI>().disableInput(); ;
            //collision.gameObject.SetActive(false); //uncomment for disaster
        }
    }
}
