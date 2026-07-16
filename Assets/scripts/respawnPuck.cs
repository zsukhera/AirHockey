using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnPuck : MonoBehaviour
{
    public GameObject puckPrefab;
    public Transform respawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void instantiatePuckAgain()
    {

        if (puckPrefab == null)
        {
            Debug.LogError("Puck prefab is missing!");
            return;
        }
        Debug.Log("here");//this is not printed
    }
}
