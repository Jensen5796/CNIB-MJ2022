using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowOrHand : MonoBehaviour
{
    public GameObject bowR;
    public GameObject bowL;
    public GameObject handR;
    public GameObject handL;
    public string menuOption;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(menuOption == "R")
        {
            bowR.SetActive(true);
            handR.SetActive(false);
            bowL.SetActive(false);
            handL.SetActive(true);
        }
        else
        {
            bowR.SetActive(false);
            handR.SetActive(true);
            bowL.SetActive(true);
            handL.SetActive(false);
        }
    }
}
