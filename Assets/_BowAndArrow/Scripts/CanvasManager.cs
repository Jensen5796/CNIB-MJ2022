using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    RectTransform gameOver;
    RectTransform mainMenu;
    RectTransform credits;

    //Skybox scene;

    //Skybox
    RectTransform skyboxOption;

    //GameObject[] testing;
    Text tester;

    Bow leftBow;
    Animator leftHand;
    Bow rightBow;
    Animator rightHand;
    BowOrHand bowHandScript;
    Quiver quiver;
    Renderer ground;
    TargetManager tm;

    
    // Start is called before the first frame update
    void Awake()
    {
        
        tester = GameObject.Find("TestText").GetComponent<Text>();
       // scene = GameObject.Find("Scene").GetComponent<Skybox>();

        leftBow = GameObject.Find("BowL").GetComponent<Bow>();
        leftHand = GameObject.Find("HandL").GetComponent<Animator>();
        rightBow = GameObject.Find("BowR").GetComponent<Bow>();
        rightHand = GameObject.Find("HandR").GetComponent<Animator>();
        bowHandScript = GameObject.Find("XR Rig").GetComponent<BowOrHand>();
        quiver = GameObject.Find("Quiver").GetComponent<Quiver>();
        ground = GameObject.Find("Ground").GetComponent<Renderer>();
        tm = GameObject.Find("Targets").GetComponent<TargetManager>();

        gameOver = GameObject.Find("EndGame").GetComponent<RectTransform>();
        mainMenu = GameObject.Find("MainMenu").GetComponent<RectTransform>();
        credits = GameObject.Find("Credits").GetComponent<RectTransform>();

        //skybox
        skyboxOption = GameObject.Find("Skybox Option").GetComponent<RectTransform>();

        gameOver.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(false);
        credits.gameObject.SetActive(false);

        //skybox
        skyboxOption.gameObject.SetActive(false);
        //testing = GameObject.FindGameObjectsWithTag("TestingText");
        //setTestingText("testing");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameTimer.getElapsedTime() <= 0)
        {
            handleGameOver();
            //handleSkyboxDN();
        }
        else
        {
            tester.text = "canvas manager online + "+GameTimer.getElapsedTime().ToString();
        }
        
        
    }

    //private void setTestingText(string text)
    //{
    //    foreach (GameObject go in testing){
    //        go.GetComponent<Text>().text = text;
    //    }
    //}

    public void handleGameOver()
    {
        char decision = ControllerResponse.getControllerResponse();
        tester.text = decision.ToString();
        //setTestingText(decision.ToString());
        gameOver.gameObject.SetActive(true);
        DisableGameComponents();
        GetExecuteEndGameDecision(decision);
    }

    private void DisableGameComponents()
    {
        if (bowHandScript.menuOption != "N")
        {
            //goController.DisableAll();

            
            bowHandScript.menuOption = "N"; // should disable bows and hands
            quiver.enabled = false;
            ground.enabled = false;
            tm.enabled = false;

            GameObject[] activeTargets = GameObject.FindGameObjectsWithTag("Target");
            foreach (GameObject target in activeTargets)
            {
                Destroy(target);
            }
        }
        
       

    }

    private void GetExecuteEndGameDecision(char decision)
    {

        if (decision == 'L')
        {
            gameOver.gameObject.SetActive(false);
            credits.gameObject.SetActive(true);
        }
        else if (decision == 'R')
        {
            gameOver.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
        }
        else
        {
            return;
        }
    }

    //Day or Night Skybox
    private void handleSkyboxDN() 
    {
        DisableLeftRightComponents();
        skyboxOption.gameObject.SetActive(true);
        char skyboxDecision = ControllerResponse.getControllerResponse();

        GetExecuteSkyboxDecision(skyboxDecision);
    }

    private void DisableLeftRightComponents() 
    {
        //disable Dat's function for the panel
    }

    private void GetExecuteSkyboxDecision(char decision) 
    {
        if (decision == 'L')
        {
            //disable left/right panel
            //day
            skyboxOption.gameObject.SetActive(false);
            RenderSettings.skybox.HasProperty("FluffballDay");


        }
        else if (decision == 'R')
        {
            //disable left/right panel
            //night
            skyboxOption.gameObject.SetActive(false);
            RenderSettings.skybox.HasProperty("DarkStorm4K");
            
        }
        else
        {
            return;
        }
    }

}
