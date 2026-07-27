using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setPanel : MonoBehaviour
{
    //this script will be given to buttons and they will then be able to 
    //set the panels and screens as active or as inactive

    public GameObject panel;//this is the panel to be set or reset 
    // Start is called before the first frame update
    void Start()
    {
        if (!panel)
            Debug.Log("The panel is not attached.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setActive()
    {
        panel.SetActive(true);  
    }

    public void reset()
    {
        panel.SetActive(false);
    }
}
