using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    SinglePlayer,
    LocalMultiplayer,
    Online
}

public class goal : MonoBehaviour
{
    [SerializeField] private GameMode gameMode = GameMode.SinglePlayer;

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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("puck"))
            return;

        sfxManager.playGoalSound();
        scoreKeeper.pauseTimer();

        switch (gameMode)
        {
            case GameMode.SinglePlayer:

                player.GetComponent<player>().disableInput();
                opponent.GetComponent<enemyAI>().disableInput();
                break;

            case GameMode.LocalMultiplayer:

                player.GetComponent<player>().disableInput();
                opponent.GetComponent<player>().disableInput();
                break;

            case GameMode.Online:

                // Let the networking system handle this.
                // Example:
                // NetworkManager.Instance.OnGoalScored();
                break;
        }
    }
}
