using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goal : MonoBehaviour
{
    public GameObject respawner;
    public GameObject audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindWithTag("audioManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("puck"))
        {
            audioManager.GetComponent<audioManager>().playGoalSound();
            StartCoroutine(DeactivatePuck(collision.gameObject));
        }
    }

    IEnumerator DeactivatePuck(GameObject puck)
    {
        yield return new WaitForSeconds(2f);
        puck.SetActive(false);
        respawner.GetComponent<respawnPuck>().instantiatePuckAgain();
    }

}
