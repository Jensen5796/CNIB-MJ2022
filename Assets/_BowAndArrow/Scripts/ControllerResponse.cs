using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public static class ControllerResponse 
{

    //ActionBasedController controllerLeft;
    //ActionBasedController controllerRight;

    // Start is called before the first frame update
    //void Start()
    //{
    //    controllerLeft = GameObject.Find("LeftHand Controller").GetComponent<ActionBasedController>();
    //    controllerRight = GameObject.Find("RightHand Controller").GetComponent<ActionBasedController>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public static char getControllerResponse()
    {
        //should be called from an update function so user response can be recorded when it is given
        //otherwise, set up a loop to continuously call this function if the response returned is 'N'

        ActionBasedController controllerLeft;
        ActionBasedController controllerRight;

        controllerLeft = GameObject.Find("LeftHand Controller").GetComponent<ActionBasedController>();
        controllerRight = GameObject.Find("RightHand Controller").GetComponent<ActionBasedController>();

        char response = 'N'; // null response before assignment
        if (controllerLeft.selectInteractionState.active)
        {
            //Left controller grip button was pressed
            response = 'L';
            
        }
        else if (controllerRight.selectInteractionState.active)
        {
            //Right controller grip button was pressed
            response = 'R';
            
        }
        return response;
    }
}
