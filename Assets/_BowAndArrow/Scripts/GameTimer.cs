using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class GameTimer : MonoBehaviour
{
    //GameObject controllerLeft;
    //GameObject controllerRight;
    //XRDirectInteractor controllerL;
    //XRDirectInteractor controllerR;
    ActionBasedController controllerLeft;
    XRDirectInteractor controllerRight;

    public GameObject timerEnded;
    public GameObject scoreBoard;
    public GameObject timerBar;

    public GameObject MainMenu;
    public GameObject Credits;
    public GameObject scene;

    public static float elapsedTime = 6;
    // Start is called before the first frame update
   // public HealthBar healthbar;
    void Start()
    {

        scene = GameObject.Find("Scene");
        MainMenu = GameObject.Find("Main Menu");
        timerEnded = GameObject.Find("MenuCredits");

        timerEnded.SetActive(false);
        MainMenu.SetActive(false); 

    }

    private void Awake()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        elapsedTime -= Time.deltaTime;


        if (elapsedTime <= 0) 
        {
            scene.SetActive(false);
            //SceneManager.LoadScene("TimerEnds");
            MainMenu.SetActive(false);
            //controllerL = controllerLeft.GetComponent<ActionBasedController>();
            //controllerR = controllerRight.GetComponent<ActionBasedController>();
            RestartGame();
        }

        SumScore.UpdateCount(elapsedTime);

        //healthbar.Tick(elapsedTime);
    }
    public void RestartGame()
    {

        timerEnded.SetActive(true);
        //controllerLeft = GameObject.Find("LeftHand Contoller");
        //controllerRight = GameObject.Find("RightHand Contoller");
        //GameObject rightHand = GameObject.Find("RightHand Controller");
        //GameObject LeftHand = GameObject.Find("LeftHand Contoller");
        //controllerL = rightHand.GetComponent<XRDirectInteractor>();
        //controllerR = LeftHand.GetComponent<XRDirectInteractor>();

        controllerLeft = GameObject.Find("LeftHand Controller").GetComponent<ActionBasedController>();
        controllerRight = GameObject.Find("RightHand Controller").GetComponent<XRDirectInteractor>();
        char response;

        response = ControllerResponse.getControllerResponse();
        if (response == 'L')
        {

            //Left controller grip button was pressed
            timerEnded.SetActive(false);
            Credits.SetActive(true);


        }
        else if (response == 'R')
        {
            //Right controller grip button was pressed
            timerEnded.SetActive(false);
            MainMenu.SetActive(true);

        }

        //if (response == 'L')
        //{
        //    Credits.SetActive(true);
        //    MainMenu.SetActive(false);
        //}
        //else if (response == 'R')
        //{
        //    MainMenu.SetActive(true);
        //    Credits.SetActive(false);
        //}

    }
}

