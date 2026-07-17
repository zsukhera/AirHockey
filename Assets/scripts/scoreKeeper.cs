using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class scoreKeeper : MonoBehaviour
{
    public GameObject player;
    public GameObject opponent;
    public GameObject puck;
    public TMP_Text playerScore;
    public TMP_Text enemyScore;
    public TMP_Text timer;
    public int gameTime=90;
    private float currentTime;
    private int playerScoreValue = 0;
    private int enemyScoreValue = 0;
    private bool timerPaused = false;
    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text resultText;

    private bool gameEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = gameTime;
        timer.text = currentTime.ToString();
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        updateGameTimer();
    }
    public void IncreasePlayerScore()
    {
        playerScoreValue++;
        playerScore.text = playerScoreValue.ToString();
    }

    public void IncreaseEnemyScore()
    {
        enemyScoreValue++;
        enemyScore.text = enemyScoreValue.ToString();
    }
    public void updateGameTimer()
    {
        if (gameEnded)
            return;

        // Only count down when not paused
        if (!timerPaused)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                gameEnded = true;

                // Disable player input
                player.GetComponent<player>().disableInput();
                opponent.GetComponent<enemyAI>().disableInput();

                // Freeze the puck
                puck.GetComponent<puck>().freezePuck();

                // Show Game Over screen
                gameOverPanel.SetActive(true);

                // Decide winner
                if (playerScoreValue > enemyScoreValue)
                    resultText.text = "You Win!";
                else if (enemyScoreValue > playerScoreValue)
                    resultText.text = "The Computer Won!";
                else
                    resultText.text = "It's a Draw!";
            }
        }

        timer.text = Mathf.CeilToInt(currentTime).ToString();
    }

    //called by goal script to pause the timer
    public void pauseTimer()
    {
        timerPaused = true;
    }

    //called by the puck when it is respawned to resume the timer
    public void resumeTimer()
    {
        timerPaused= false;
    }
}
