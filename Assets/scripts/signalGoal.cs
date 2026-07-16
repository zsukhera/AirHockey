using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class signalGoal : MonoBehaviour
{
    /// <summary>
    ///this script shall be applied to the goals to signal
    ///the scorekeeper to increase the values
    /// </summary>

    public GameObject scoreKeeper;
    // Start is called before the first frame update
    void Start()
    {
        if (!scoreKeeper)
            scoreKeeper = GameObject.FindWithTag("scoreKeeper");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("puck"))
        {
            increasePlayerScore();
        }
    }

    //the player score will be increased, since this is the player goal
    private void increasePlayerScore()
    {
        scoreKeeper.GetComponent<scoreKeeper>().IncreasePlayerScore();
    }
}
