using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class scoreKeeper : MonoBehaviour
{

    public TMP_Text playerScore;
    public TMP_Text enemyScore;
    public TMP_Text timer;
    public int gameTime=90;
    private float currentTime;
    private int playerScoreValue = 0;
    private int enemyScoreValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = gameTime;
        timer.text = currentTime.ToString();
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
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            currentTime = 0;
            // Game over logic here
        }

        timer.text = Mathf.CeilToInt(currentTime).ToString();
    }
}
