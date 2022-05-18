using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class ControllerTester : MonoBehaviour
{
    ActionBasedController controllerLeft;
    ActionBasedController controllerRight;
    Text test;
    ColorChange testCube;
    GameTimer gameTime;

    // Start is called before the first frame update
    void Awake()
    {
        controllerLeft = GameObject.Find("LeftHand Controller").GetComponent<ActionBasedController>();
        controllerRight = GameObject.Find("RightHand Controller").GetComponent<ActionBasedController>();
        test = GameObject.Find("TestText").GetComponent<Text>();
        //test.text = "controller testing";
        testCube = GameObject.Find("TestCube").GetComponent<ColorChange>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        char response = getControllerResponse();
        testCube.changeColor(response);
        
        
    }

    public char getControllerResponse()
    {
        //should be called from an update function so user response can be recorded when it is given
        //otherwise, set up a loop to continuously call this function if the response returned is 'N'

        char response = 'N'; // null response before assignment
        //test.text = response.ToString(); 
        if (controllerLeft.selectInteractionState.active)
        {
            //Left controller grip button was pressed
            
            response = 'L';
            //test.text += response.ToString();
        }
        if (controllerRight.selectInteractionState.active)
        {
            //Right controller grip button was pressed
            response = 'R';
            //test.text += response.ToString();
        }
        return response;
    }
}
