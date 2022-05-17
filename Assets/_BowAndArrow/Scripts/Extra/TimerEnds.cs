using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TimerEnds : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject Credits;
    public GameObject timerEnded;

    private XRDirectInteractor rightHandInteractor;
    private XRDirectInteractor leftHandInteractor;
    private void Start()
    {
        GameObject rightHand = GameObject.Find("RightHand Controller");
        GameObject leftHand = GameObject.Find("LeftHand Controller");
        rightHandInteractor = rightHand.GetComponent<XRDirectInteractor>();
        leftHandInteractor = rightHand.GetComponent<XRDirectInteractor>();

        //if (rightHandInteractor.isSelectActive) 
        //{
        //    Main();
        //}
        
    }
    public void Main() 
    {
        timerEnded.SetActive(false);
       // MainMenu.SetActive(true);

    }
}
