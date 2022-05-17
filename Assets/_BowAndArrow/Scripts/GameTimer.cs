using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class GameTimer : MonoBehaviour
{

    ActionBasedController controllerLeft;
    ActionBasedController controllerRight;


    public GameObject timerEnded;
    public GameObject scene;

    public GameObject MainMenu;
    public GameObject Credits;


    public ControllerResponse cResponse;

    public static float elapsedTime = 5;
    // Start is called before the first frame update
   // public HealthBar healthbar;
    void Start()
    {
        timerEnded = GameObject.Find("MenuCredits");
        MainMenu = GameObject.Find("Main Menu");
        controllerLeft = GameObject.Find("LeftHand Contoller").GetComponent<ActionBasedController>();
        controllerRight = GameObject.Find("RightHand Contoller").GetComponent<ActionBasedController>();
        scene = GameObject.Find("Scene");

        timerEnded.SetActive(false);
        MainMenu.SetActive(false);
        scene.SetActive(true);

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
            RestartGame();
        }

        SumScore.UpdateCount(elapsedTime);

        //healthbar.Tick(elapsedTime);
    }
    public void RestartGame()
    {

        scene.SetActive(false);
        //SceneManager.LoadScene("TimerEnds");

        timerEnded.SetActive(true);
        MainMenu.SetActive(false);
        //char response;
        if (controllerLeft.selectInteractionState.active)
        {
            //Left controller grip button was pressed
            timerEnded.SetActive(false);
            Credits.SetActive(true);
   

        }
        else if (controllerRight.selectInteractionState.active)
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

