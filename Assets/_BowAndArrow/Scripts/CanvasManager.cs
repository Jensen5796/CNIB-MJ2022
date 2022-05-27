using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    private RectTransform gameOver;
    private RectTransform mainMenu;
    private RectTransform credits;
    private RectTransform targetColSelectionDay;
    private RectTransform targetColSelectionNight;
    private RectTransform handSelection;

    //Skybox scene;

    //Skybox
    private RectTransform skyboxOption;

    //GameObject[] testing;
    private Text tester;

    private Bow leftBow;
    private Animator leftHand;
    private Bow rightBow;
    private Animator rightHand;
    private BowOrHand bowHandScript;
    private Quiver quiver;
    private Renderer ground;
    private TargetManager tm;
    private Canvas scoreboard;
    private Canvas healthbar;
    private GameTimer gameTimer;
    private Transform demoTarget;

    //Int to hold state of the game (which elements should be visible at particular time)
    //1 = Main menu (game start) - Demo or Play
    //2 = Hand selection (setting option) - Right or Left
    //3 = Environment selection (setting option) - Day or Night
    //4 = Target colors (setting option) - Set 1 or Set 2
    //5 = Demo mode (shoot arrow up to leave demo)
    //6 = Game play mode (shoot arrow up to access settings, or timer ends
    //7 = Round end - Main Menu or Credits
    //8 = Credits (any button returns to Main Menu)
    public static int gameState = 1; //start at main menu

    private bool isDemoModeSelected = false; //whether user has chosen demo or not
    private bool isDayModeSelected = true;
    private char LRHandSelection = 'N';
    private char prevButtonState = 'N';

    // Start is called before the first frame update
    private void Awake()
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
        scoreboard = GameObject.Find("ScoreBoard").GetComponentInChildren<Canvas>();
        healthbar = GameObject.Find("TimerCanvas").GetComponent<Canvas>();
        gameTimer = GameObject.Find("Scene").GetComponent<GameTimer>();

        gameOver = GameObject.Find("EndGame").GetComponent<RectTransform>();
        mainMenu = GameObject.Find("MainMenu").GetComponent<RectTransform>();
        credits = GameObject.Find("Credits").GetComponent<RectTransform>();
        targetColSelectionDay = GameObject.Find("TargetColorsDay").GetComponent<RectTransform>();
        targetColSelectionNight = GameObject.Find("TargetColorsNight").GetComponent<RectTransform>();
        handSelection = GameObject.Find("LeftRight Option").GetComponent<RectTransform>();
        //skybox
        skyboxOption = GameObject.Find("Skybox Option").GetComponent<RectTransform>();

        //Target for Demo Mode
        demoTarget = GameObject.Find("DemoMode_SphericalTarget").GetComponent<Transform>();

        gameOver.gameObject.SetActive(false);

        credits.gameObject.SetActive(false);
        targetColSelectionDay.gameObject.SetActive(false);
        targetColSelectionNight.gameObject.SetActive(false);
        handSelection.gameObject.SetActive(false);

        //Target for Demo Mode
        demoTarget.gameObject.SetActive(false);
        //skybox
        skyboxOption.gameObject.SetActive(false);
        //testing = GameObject.FindGameObjectsWithTag("TestingText");
        //setTestingText("testing");
        DisableGameComponents();
        mainMenu.gameObject.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
        //check gameState in switch statement, execute appropriate function

        char checkState = ControllerResponse.getControllerResponse();
        if (prevButtonState == 'N' && checkState != 'N')
        {
            prevButtonState = checkState;
            switch (gameState)
            {
                case 1:
                    handleMainMenu();
                    return;

                case 2:
                    handleHandSelection();
                    return;

                case 3:
                    handleEnvSelection();
                    return;

                case 4:
                    handleTargetColSelection();
                    return;

                case 5:
                    InitiateDemoMode();
                    return;

                case 6:
                    InitiateGameMode();
                    return;

                case 7:
                    handleGameOver();
                    return;

                case 8:
                    ShowCredits();
                    return;

                default:
                    //if gameState is not set currently
                    return;
            }
        }
        else
        {
            prevButtonState = checkState;
        }

        //if (GameTimer.getElapsedTime() <= 0)  // find where this happens and trigger the game state from that code
        //{
        //    handleGameOver();
        //    //handleSkyboxDN();
        //}
    }

    public void handleMainMenu()
    {
        DisableGameComponents();
        mainMenu.gameObject.SetActive(true);
        char decision = ControllerResponse.getControllerResponse();
        ExecuteMainMenuDecision(decision);
    }

    private void ExecuteMainMenuDecision(char decision)
    {
        tester.text = "In Main Menu Selection";

        if (decision == 'L') //Demo
        {
            isDemoModeSelected = true;

            //hide menu screen
            mainMenu.gameObject.SetActive(false);

            //change game state
            gameState = 2;
            handSelection.gameObject.SetActive(true);
        }
        else if (decision == 'R') //Play --> proceed thru settings
        {
            //hide menu screen
            mainMenu.gameObject.SetActive(false);

            //change game state
            gameState = 2;
            handSelection.gameObject.SetActive(true);
        }
        else
        {
            return;
        }
    }

    public void handleHandSelection()
    {
        DisableGameComponents(); //done in Main Menu, but also needs done here in case settings is called within game
                                 //handSelection.gameObject.SetActive(true);
        char decision = ControllerResponse.getControllerResponse();

        ExecuteHandSelectionDecision(decision);
    }

    private void ExecuteHandSelectionDecision(char decision)
    {
        tester.text += " In Hand Selection";

        if (decision == 'L') //set left hand
        {
            //make setting change for L:
            LRHandSelection = 'L';

            //change game state
            gameState = 3;

            //hide menu
            handSelection.gameObject.SetActive(false);
            skyboxOption.gameObject.SetActive(true);
        }
        else if (decision == 'R') //set right hand
        {
            //make setting change for R:
            LRHandSelection = 'R';

            //change game state
            gameState = 3;

            //hide menu
            handSelection.gameObject.SetActive(false);
            skyboxOption.gameObject.SetActive(true);
        }
        else
        {
            return;
        }
    }

    public void handleEnvSelection()
    {
        //skyboxOption.gameObject.SetActive(true);
        char decision = ControllerResponse.getControllerResponse();
        ExecuteEnvSelectionDecision(decision);
    }

    private void ExecuteEnvSelectionDecision(char decision)
    {
        tester.text += " In Skybox Selection";

        if (decision == 'L') //set night mode
        {
            isDayModeSelected = false;
            //make setting change for L:

            //change game state
            gameState = 4;

            //hide menu
            skyboxOption.gameObject.SetActive(false);
            if (isDayModeSelected) //day mode
            {
                //show panel for daytime target options
                targetColSelectionDay.gameObject.SetActive(true);
            }
            else
            {
                //show panel for nighttime target options
                targetColSelectionNight.gameObject.SetActive(true);
            }
        }
        else if (decision == 'R') //set day mode
        {
            isDayModeSelected = true;
            //make setting change for R:

            //change game state
            gameState = 4;

            //hide menu
            skyboxOption.gameObject.SetActive(false);
            if (isDayModeSelected) //day mode
            {
                //show panel for daytime target options
                targetColSelectionDay.gameObject.SetActive(true);
            }
            else
            {
                //show panel for nighttime target options
                targetColSelectionNight.gameObject.SetActive(true);
            }
        }
        else
        {
            return;
        }
    }

    public void handleTargetColSelection()
    {
        //existing targets should already be destroyed from Disabling game components at beginning of hand selection step

        //if (isDayModeSelected) //day mode
        //{
        //    //show panel for daytime target options
        //    targetColSelectionDay.gameObject.SetActive(true);
        //}
        //else
        //{
        //    //show panel for nighttime target options
        //    targetColSelectionNight.gameObject.SetActive(true);
        //}

        char decision = ControllerResponse.getControllerResponse();
        ExecuteTargetColSelectionDecision(decision);
    }

    private void ExecuteTargetColSelectionDecision(char decision)
    {
        tester.text += " In Target color Selection";

        if (decision == 'L') //target col choice 1
        {
            //make setting change for L:  **make check for day or night mode** and changes need to be made to prefab
            if (isDayModeSelected)
            {
            }
            else
            {
            }

            //change game state
            if (isDemoModeSelected)
            {
                gameState = 5;
                InitiateDemoMode();
            }
            else
            {
                gameState = 6;
                InitiateGameMode();
            }

            //hide menu
            if (isDayModeSelected) //day mode
            {
                //hide panel for daytime target options
                targetColSelectionDay.gameObject.SetActive(false);
            }
            else
            {
                //hide panel for nighttime target options
                targetColSelectionNight.gameObject.SetActive(false);
            }
        }
        else if (decision == 'R') //target col choice 2
        {
            //make setting change for R: **make check for day or night mode** changes need to be made to prefab
            if (isDayModeSelected)
            {
            }
            else
            {
            }

            //hide menu
            if (isDayModeSelected) //day mode
            {
                //hide panel for daytime target options
                targetColSelectionDay.gameObject.SetActive(false);
            }
            else
            {
                //hide panel for nighttime target options
                targetColSelectionNight.gameObject.SetActive(false);
            }
            //change game state
            if (isDemoModeSelected)
            {
                gameState = 5;
                InitiateDemoMode();
            }
            else
            {
                gameState = 6;
                InitiateGameMode();
            }
        }
        else
        {
            return;
        }
    }

    //private void InitiateDemoMode()
    //{
    //    tester.text += " In Demo Mode";

    //    //show singleton target, ground, skybox, enable quiver, set BowOrQuiver script to LRHandSelection
    //    //hide healthbar and scoreboard
    //    //disable target manager, game timer, ?others

    //    //***** handling sound cues to guide user, based on single target?
    //    // if target not hit (no child 'Arrow')
    //    //play directions to load and shoot arrow
    //    //play directions to turn body to find target
    //    //when player hits target (coin sound)
    //    //tell player the score value for each target ring
    //    //in Play Mode the target will disappear and need to find new target
    //    //tell player how long the round is
    //    //tell player to shoot straight up above them to access settings in game, or to leave demo now
    //    //need to add collider for above player w/ script that checks this one for demo mode
    //    //if demo mode
    //    //change game state to main menu,
    //    //set isDemoModeSelected to false, and set gameStateInstantiated[5] = false
    //    //remove singleton target

    //    //if play mode then then pause game timer script change game state to l/r hand setting
    //    //(suggestion - get health bar script to look at elapsed time in game timer script rather than have internal timing)
    //}
    private void InitiateDemoMode()
    {
        tester.text += " In Demo Mode";

        //enable ground
        ground.enabled = true;

        //enable quiver
        quiver.enabled = true;


        //enable skybox (day/night) - doesn't have to deal with this since the skybox will stay on 


        //enable target for demo
        demoTarget.gameObject.SetActive(true);

        //enable bow and arrow
        bowHandScript.menuOption = "L";

        /**enable sound cues:**/
        /*You are in Demo Mode. To exit shoot upward - this will return the player to the main menu*/

        /*
         * **Left Handed**
         * As a left handed the bow should be on your right
         * 
         * You can stretch the string by pressing left grip while pulling back
         * 
         * To load or reload the arrow onto the bow, press the right grip
         */
        /**-----**/
        /*
         * Righ Handed
         * As a left handed the bow should be on your right
         * 
         * To load or reload the arrow onto the bow, press the left grip
         * 
         * Try to aim the target: inner target is 100pts, middle target is 50%, and the outer target is 20%
         * 
         * 
         */
    }
    private void InitiateGameMode()
    {
        tester.text += " In Game Mode";

        //start TargetManagerScript
        //show scoreboard, healthbar
        //start gameTimer script
        //enable ground, skybox, enable quiver,
        //set BowOrHand script to LRHandSelection
    }

    public void handleGameOver()
    {
        //will need to call this function from the GameTimer script somehow

        gameOver.gameObject.SetActive(true);
        char decision = ControllerResponse.getControllerResponse();
        //tester.text = decision.ToString();
        //setTestingText(decision.ToString());

        DisableGameComponents();
        GetExecuteEndGameDecision(decision);
    }

    private void DisableGameComponents()    {
        if (bowHandScript.menuOption != "N")
        {
            bowHandScript.menuOption = "N"; // should disable bows and hands
            gameTimer.enabled = false;
            quiver.enabled = false;
            ground.enabled = false;
            tm.enabled = false;
            scoreboard.enabled = false;
            healthbar.enabled = false;

            GameObject[] activeTargets = GameObject.FindGameObjectsWithTag("Target");
            foreach (GameObject target in activeTargets)
            {
                Destroy(target);
            }
        }
    }

    private void GetExecuteEndGameDecision(char decision)
    {
        tester.text += " In End Game Selection";
        if (decision == 'L')
        {
            gameState = 8; //credits
            gameOver.gameObject.SetActive(false);
            credits.gameObject.SetActive(true);
        }
        else if (decision == 'R')
        {
            gameState = 1; //main menu
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
        //DisableLeftRightComponents(); - won't need this, it will be handled before this function is called
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

    private void ShowCredits()
    {
        tester.text += " In Credits";

        //credits.gameObject.SetActive(true);
        char creditsDecision = ControllerResponse.getControllerResponse();
        if (creditsDecision == 'L' || creditsDecision == 'R') //if any char has been assigned meaning a button has been pressed
        {
            //main menu
            gameState = 1;
        }
    }
}