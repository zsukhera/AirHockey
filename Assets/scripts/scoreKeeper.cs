using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class scoreKeeper : MonoBehaviour
{

    public TMP_Text playerScore;
    public TMP_Text enemyScore;
    private int playerScoreValue = 0;
    private int enemyScoreValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("scorekeeper aoa");
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
