using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class TimerEnds : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject Credits;
    public GameObject timerEnded;
    ControllerResponse controllerHandler;
    Text test;

    //private XRDirectInteractor rightHandInteractor;
    //private XRDirectInteractor leftHandInteractor;
    private void Start()
    {
        //GameObject rightHand = GameObject.Find("RightHand Controller");
        //GameObject leftHand = GameObject.Find("LeftHand Controller");
        //rightHandInteractor = rightHand.GetComponent<XRDirectInteractor>();
        //leftHandInteractor = rightHand.GetComponent<XRDirectInteractor>();

        //if (rightHandInteractor.isSelectActive) 
        //{
        //    Main();
        //}
        controllerHandler = new ControllerResponse();
        test = GameObject.Find("Text").GetComponent<Text>();
        
    }

    private void Update()
    {
        char response = controllerHandler.getControllerResponse();
        if (response == 'L')
        {
            //do left side thing
            test.text = "L side pressed";
        }
        else if (response == 'R')
        {
            //do right side thing
            test.text = "R side pressed";
            Main();
        }
    }
    public void Main() 
    {
        timerEnded.SetActive(false);
        MainMenu.SetActive(true);

    }
}
