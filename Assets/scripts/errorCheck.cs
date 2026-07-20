using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class errorCheck : MonoBehaviour
{
    public puck puck;   //assign via inspector
    //save some computation for once
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("puck"))
        {
            puck.respawn();
        }
    }
}
