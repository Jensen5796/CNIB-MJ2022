using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasManager : MonoBehaviour
{
    private RectTransform gameOver;
    private RectTransform mainMenu;
    private RectTransform credits;
    private RectTransform targetColSelectionDay;
    private RectTransform targetColSelectionNight;
    private RectTransform handSelection;
    private RectTransform demoPanel;

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
    private gameFlowCue soundcues;

    //demo mode object
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

    private static bool isDemoModeSelected = false; //whether user has chosen demo or not
    private bool isDayModeSelected = true;
    public static string LRHandSelection = "N";
    private char prevButtonState = 'N';
    private static GameObject targetSelection;
    public static bool showMainMenuPanel;
    public static bool showEndGamePanel;
    public static bool showHandSelection;
    public static bool disableGameComponents;
    public static bool inGameMode = false;
    public static bool inDemoMode = false;
    public static bool inSettingsDuringGame = false;
    private bool isAudioPlaying = false;

    private AudioClip[] mainMenuCues;
    private AudioClip[] handSettingCues;
    private AudioClip[] skyboxSettingCues;
    private AudioClip[] dayTargetCues;
    private AudioClip[] nightTargetCues;
    private AudioClip[] endGameCues;
    private AudioClip[] leftDemoCues;
    private AudioClip[] rightDemoCues;
    private AudioClip[] creditsCues;
    public static bool[] haveCuesPlayedForThisState;
    public static bool shouldArrowGiveFeedback;

    //sound cue
    private gameFlowCue gameCue;
    private Text scoreText;
    private float demoTimer;
    private bool isDemoAudioPlaying;

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
        demoPanel = GameObject.Find("Demo").GetComponent<RectTransform>();
        //skybox
        skyboxOption = GameObject.Find("Skybox Option").GetComponent<RectTransform>();
        scoreText = GameObject.Find("ScoreDisplay").GetComponent<Text>();

        //demo mode
        //Target for Demo Mode
        //demoTarget = GameObject.Find("DemoMode_SphericalTarget").GetComponent<Transform>();

        //Target for Demo Mode
        //demoTarget.gameObject.SetActive(false);

        gameOver.gameObject.SetActive(false);

        credits.gameObject.SetActive(false);
        targetColSelectionDay.gameObject.SetActive(false);
        targetColSelectionNight.gameObject.SetActive(false);
        handSelection.gameObject.SetActive(false);
        demoPanel.gameObject.SetActive(false);
        soundcues = this.GetComponent<gameFlowCue>();

        mainMenuCues = new AudioClip[] { soundcues.welcomeToMainMenu, soundcues.gameMode_DemoMode }; //1
        handSettingCues = new AudioClip[] { soundcues.LRSelection, soundcues.LRHanded  }; //2
        skyboxSettingCues = new AudioClip[] { soundcues.dayNight, soundcues.chooseDNMode  }; //3
        dayTargetCues = new AudioClip[] { soundcues.chooseTargetColors, soundcues.BYorPGdayMode }; //4
        nightTargetCues = new AudioClip[] { soundcues.chooseTargetColors, soundcues.WRorPGnightMode }; //5
        endGameCues = new AudioClip[] { soundcues.wellDone, soundcues.creditsorMainMenu }; //6
        leftDemoCues = new AudioClip[] { soundcues.fullDemoModeBowInLeft }; //7
        //leftDemoCues = new AudioClip[] { soundcues.enteringDemoMode, soundcues.leftHandedBow, soundcues.LHloadorReload}; //7
        rightDemoCues = new AudioClip[] { soundcues.fullDemoModeBowInRight }; //8
        //rightDemoCues = new AudioClip[] { soundcues.enteringDemoMode, soundcues.rightHandedBow, soundcues.RHloadorReload }; //8
        creditsCues = new AudioClip[] { soundcues.credits };
        //creditsCues //9
        haveCuesPlayedForThisState = new bool[10] { false, false, false, false, false, false, false, false, false, false };

        //skybox
        skyboxOption.gameObject.SetActive(false);
        //testing = GameObject.FindGameObjectsWithTag("TestingText");
        //setTestingText("testing");
        DisableGameComponents();
        mainMenu.gameObject.SetActive(true);
        StartCoroutine(PlayAudioSequence(mainMenuCues, mainMenu));
        
        
    }

    // Update is called once per frame
    private void Update()
    {
        //check gameState in switch statement, execute appropriate function
        if (showMainMenuPanel)
        {
            mainMenu.gameObject.SetActive(true);
            
            StartCoroutine(PlayAudioSequence(mainMenuCues, mainMenu));
            showMainMenuPanel = false;
            demoPanel.gameObject.SetActive(false);
            
        }
        if (showEndGamePanel)
        {
            
            //scoreText.text = "Reached this";
            scoreText.text = "Score: "+SumScore.Score.ToString();
            gameOver.gameObject.SetActive(true);
            StartCoroutine(PlayAudioSequence(endGameCues, gameOver));
            showEndGamePanel = false;
        }
        if (showHandSelection)
        {
            handSelection.gameObject.SetActive(true);
            
            StartCoroutine(PlayAudioSequence(handSettingCues, handSelection));
            showHandSelection = false;
        }
        if (disableGameComponents)
        {
            DisableGameComponents();
        }

        char checkState = ControllerResponse.getControllerResponse();
        if (prevButtonState == 'N' && checkState != 'N')
        {
            prevButtonState = checkState;
            switch (gameState)
            {
                case 1:
                    //haveCuesPlayedForThisState[1] = false;
                    handleMainMenu();
                    return;

                case 2:
                    //haveCuesPlayedForThisState[2] = false;
                    handleHandSelection();
                    return;

                case 3:
                    //haveCuesPlayedForThisState[3] = false;
                    handleEnvSelection();
                    return;

                case 4:
                    //haveCuesPlayedForThisState[4] = false;
                    handleTargetColSelectionDay();
                    return;

                case 5:
                    //haveCuesPlayedForThisState[5] = false;
                    InitiateDemoMode();
                    return;

                case 6:
                    //haveCuesPlayedForThisState[6] = false;
                    InitiateGameMode();
                    return;

                case 7:
                    //haveCuesPlayedForThisState[7] = false;
                    handleGameOver();
                    return;

                case 8:
                    //haveCuesPlayedForThisState[8] = false;
                    ShowCredits();
                    return;

                case 9:
                    //haveCuesPlayedForThisState[9] = false;
                    handleTargetColSelectionNight();
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
        //StartCoroutine(PlayAudioSequence(mainMenuCues, mainMenu));
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
            StartCoroutine(PlayAudioSequence(handSettingCues, handSelection));
        }
        else if (decision == 'R') //Play --> proceed thru settings
        {
            //hide menu screen
            mainMenu.gameObject.SetActive(false);

            //change game state
            gameState = 2;
            handSelection.gameObject.SetActive(true);
            StartCoroutine(PlayAudioSequence(handSettingCues, handSelection));
        }
        else
        {
            return;
        }
    }

    public void handleHandSelection()
    {
        showHandSelection = false; // bool used in CeilingSettings script - panel will still show during this function
        StartCoroutine(PlayAudioSequence(handSettingCues, handSelection));
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
            LRHandSelection = "L";

            //change game state
            gameState = 3;

            //hide menu
            handSelection.gameObject.SetActive(false);
            skyboxOption.gameObject.SetActive(true);
            StartCoroutine(PlayAudioSequence(skyboxSettingCues, skyboxOption));
        }
        else if (decision == 'R') //set right hand
        {
            //make setting change for R:
            LRHandSelection = "R";

            //change game state
            gameState = 3;

            //hide menu
            handSelection.gameObject.SetActive(false);
            skyboxOption.gameObject.SetActive(true);
            StartCoroutine(PlayAudioSequence(skyboxSettingCues, skyboxOption));
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
        //Color nightGroundColor = new Color(96, 98, 97, 255);
        //Color dayGroundColor = new Color(255, 255, 255, 255);
        if (decision == 'R') //set day mode
        {
            //day mode
            isDayModeSelected = true;
            //make setting change for R:
            //ground.material.SetColor("_Color", dayGroundColor);
            ground.material.SetColor("_BaseColor", Color.white);
            RenderSettings.skybox.SetFloat("_Exposure", .95f);

            

            //hide menu
            skyboxOption.gameObject.SetActive(false);
            if (isDayModeSelected) //day mode
            {
                gameState = 4;
                //show panel for daytime target options
                targetColSelectionDay.gameObject.SetActive(true);
                StartCoroutine(PlayAudioSequence(dayTargetCues, targetColSelectionDay));
            }
            else
            {
                gameState = 9;
                //show panel for nighttime target options
                targetColSelectionNight.gameObject.SetActive(true);
                StartCoroutine(PlayAudioSequence(nightTargetCues, targetColSelectionNight));
            }
            
        }
        else if (decision == 'L') //set night mode
        {
            //night mode
            isDayModeSelected = false;
            //make setting change for L:

            //ground.material.SetColor("_Color", nightGroundColor);

            ground.material.SetColor("_BaseColor", Color.gray);
            RenderSettings.skybox.SetFloat("_Exposure", .17f);

            
            

            //hide menu
            skyboxOption.gameObject.SetActive(false);
            if (isDayModeSelected) //day mode
            {
                gameState = 4;
                //show panel for daytime target options
                targetColSelectionDay.gameObject.SetActive(true);
                
                StartCoroutine(PlayAudioSequence(dayTargetCues, targetColSelectionDay));
            }
            else
            {
                gameState = 9;
                //show panel for nighttime target options
                targetColSelectionNight.gameObject.SetActive(true);
                StartCoroutine(PlayAudioSequence(nightTargetCues, targetColSelectionNight));
            }
        }
        else
        {
            return;
        }
    }

    public void handleTargetColSelectionDay()
    {
        //existing targets should already be destroyed from Disabling game components at beginning of hand selection step

        char decision = ControllerResponse.getControllerResponse();
        ExecuteTargetColSelectionDecisionDay(decision);
    }

    private void ExecuteTargetColSelectionDecisionDay(char decision)
    {
        tester.text += " In Target color Selection";

        if (decision == 'L') //target col choice 1
        {
            //make setting change for L:  **make check for day or night mode** and changes need to be made to prefab
            //** make a script to hold the prefab options - need to drag them into inspector, cannot reference from script
            //** then select prefab from holder script
            
                //left + day = yellow/black target
                targetSelection = TargetPrefabHolder.getLeftDayTarget();
                targetColSelectionDay.gameObject.SetActive(false);
            

            //change game state
            if (isDemoModeSelected)
            {
                gameState = 5;
                //GetComponent<AudioSource>().PlayOneShot(soundcues.enteringDemoMode);
                demoPanel.gameObject.SetActive(true);
                InitiateDemoMode();
            }
            else
            {
                gameState = 6;
                if (inSettingsDuringGame)
                {
                    ResumeGame();
                }
                else
                {
                    GetComponent<AudioSource>().PlayOneShot(soundcues.enteringGameMode);
                    InitiateGameMode();
                }
            }
        }
        else if (decision == 'R') //target col choice 2
        {
            //make setting change for R: **make check for day or night mode** changes need to be made to prefab
            
                //right + day = green/pink target
                targetSelection = TargetPrefabHolder.getRightDayTarget();
                targetColSelectionDay.gameObject.SetActive(false);
            

            //change game state
            if (isDemoModeSelected)
            {
                gameState = 5;
                //GetComponent<AudioSource>().PlayOneShot(soundcues.enteringDemoMode);
                demoPanel.gameObject.SetActive(true);
                InitiateDemoMode();
                
            }
            else
            {
                gameState = 6;
                if (inSettingsDuringGame)
                {
                    ResumeGame();
                }
                else
                {
                    GetComponent<AudioSource>().PlayOneShot(soundcues.enteringGameMode);
                    InitiateGameMode();
                }
            }
        }
        else
        {
            return;
        }
    }

    public void handleTargetColSelectionNight()
    {
        //existing targets should already be destroyed from Disabling game components at beginning of hand selection step

        char decision = ControllerResponse.getControllerResponse();
        ExecuteTargetColSelectionDecisionNight(decision);
    }

    private void ExecuteTargetColSelectionDecisionNight(char decision)
    {
        tester.text += " In Target color Selection";

        if (decision == 'L') //target col choice 1
        {
            //make setting change for L:  **make check for day or night mode** and changes need to be made to prefab
            //** make a script to hold the prefab options - need to drag them into inspector, cannot reference from script
            //** then select prefab from holder script
            
            //left + night = white/red target
            targetSelection = TargetPrefabHolder.getLeftNightTarget();
            targetColSelectionNight.gameObject.SetActive(false);
            

            //change game state
            if (isDemoModeSelected)
            {
                gameState = 5;
                //GetComponent<AudioSource>().PlayOneShot(soundcues.enteringDemoMode);
                demoPanel.gameObject.SetActive(true);
                InitiateDemoMode();
            }
            else
            {
                gameState = 6;
                if (inSettingsDuringGame)
                {
                    ResumeGame();
                }
                else
                {
                    GetComponent<AudioSource>().PlayOneShot(soundcues.enteringGameMode);
                    InitiateGameMode();
                }
            }
        }
        else if (decision == 'R') //target col choice 2
        {
            
            
            //right + night = green/pink target
            targetSelection = TargetPrefabHolder.getRightNightTarget();
            targetColSelectionNight.gameObject.SetActive(false);
            

            //change game state
            if (isDemoModeSelected)
            {
                gameState = 5;
                //GetComponent<AudioSource>().PlayOneShot(soundcues.enteringDemoMode);
                demoPanel.gameObject.SetActive(true);
                InitiateDemoMode();
            }
            else
            {
                gameState = 6;
                if (inSettingsDuringGame)
                {
                    ResumeGame();
                }
                else
                {
                    GetComponent<AudioSource>().PlayOneShot(soundcues.enteringGameMode);
                    InitiateGameMode();
                }
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
    //    //enable ground

    //}
    private void InitiateDemoMode()
    {
        
        if (!inDemoMode)
        {
            shouldArrowGiveFeedback = false;
            isDemoAudioPlaying = true;
            demoTimer = Time.fixedTime;
            isDemoModeSelected = false;
            if (LRHandSelection == "R")
            {
                StartCoroutine(PlayAudioSequence(rightDemoCues, demoPanel));
            }
            else if (LRHandSelection == "L") 
            {
                StartCoroutine(PlayAudioSequence(leftDemoCues, demoPanel));
            }
            inDemoMode = true;
            
            tester.text += " In Demo Mode";
            ground.enabled = true;

            //enable quiver
            quiver.enabled = true;

            //enable skybox (day/night) - doesn't have to deal with this since the skybox will stay on

            //enable target for demo
            //thought - will need a separate demo target script (short - just instantiate target in one position)
            // -- only way to make the target in demo be the color they selected in settings
            //demoTarget.gameObject.SetActive(true);
            tm.enabled = true;
            TargetManager.RestartTargetManager();

            //enable bow and arrow
            //bowHandScript.menuOption = "L";
            bowHandScript.menuOption = LRHandSelection;

            

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
        if (isDemoAudioPlaying)
        {
            if ((LRHandSelection == "L") && (Time.fixedTime - demoTimer) >= 22.9)
            {
                shouldArrowGiveFeedback = true;
                isDemoAudioPlaying = false;
            }
            else if ((LRHandSelection == "R") && (Time.fixedTime - demoTimer) >= 23.2)
            {
                shouldArrowGiveFeedback = true;
                isDemoAudioPlaying = false;
            }
        }
        

    }

    private void InitiateGameMode()
    {
        if (!inGameMode)
        {
            shouldArrowGiveFeedback = true;
            inGameMode = true;
            tester.text += " In Game Mode";
            

            tm.enabled = true;
            TargetManager.RestartTargetManager();
            //tester.text += targetSelection.gameObject.name;

            SumScore.Reset();
            scoreboard.enabled = true;
            healthbar.enabled = true;
            gameTimer.enabled = true;
            GameTimer.resetElapsedTime();

            // will I need to start the health bar (connected to game timer now, might be something more to do here)

            ground.enabled = true;
            quiver.enabled = true;
            bowHandScript.menuOption = LRHandSelection;
        }
    }

    private void ResumeGame()
    {
        if (inSettingsDuringGame)
        {
            inSettingsDuringGame = false;

            tm.enabled = true;
            TargetManager.RestartTargetManager();

            scoreboard.enabled = true;
            healthbar.enabled = true;
            GameTimer.isPaused = false;

            ground.enabled = true;
            quiver.enabled = true;
            bowHandScript.menuOption = LRHandSelection;
        }
    }

    public void handleGameOver()
    {
        inGameMode = false;
        Text scoreText = GameObject.Find("ScoreDisplay").GetComponent<Text>();
        scoreText.text = SumScore.Score.ToString();
        //will need to call this function from the GameTimer script somehow
        showEndGamePanel = false; //bool used with GameTimer script to trigger panel to show in first place - the panel will still be showing in this function even if this is false
        gameOver.gameObject.SetActive(true);
        char decision = ControllerResponse.getControllerResponse();
        //tester.text = decision.ToString();
        //setTestingText(decision.ToString());

        DisableGameComponents();
        GetExecuteEndGameDecision(decision);
    }

    private void DisableGameComponents()
    {
        disableGameComponents = false;
        if (bowHandScript.menuOption != "N")
        {
            bowHandScript.menuOption = "N"; // should disable bows and hands

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

            if (!inSettingsDuringGame)
            {
                gameTimer.enabled = false;
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
            StartCoroutine(PlayAudioSequence(creditsCues, credits));
        }
        else if (decision == 'R')
        {
            gameState = 1; //main menu
            gameOver.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
            StartCoroutine(PlayAudioSequence(mainMenuCues, mainMenu));
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
        GetComponent<AudioSource>().PlayOneShot(gameCue.dayNight);
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().PlayOneShot(gameCue.chooseDNMode);
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
            RenderSettings.skybox.SetFloat("_Exposure", .95f);
        }
        else if (decision == 'R')
        {
            //disable left/right panel
            //night
            RenderSettings.skybox.SetFloat("_Exposure", .17f);
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
            credits.gameObject.SetActive(false);
            //mainMenu.gameObject.SetActive(true);
            showMainMenuPanel = true;
        }
    }

    public static GameObject getTargetSelection()
    {
        return targetSelection;
    }

    public static bool getIsDemoModeSelected()
    {
        return isDemoModeSelected;
    }

    public static void DisableGameComponentsWrapper()
    {
        disableGameComponents = true;
    }

   IEnumerator PlayAudioSequence(AudioClip[] sequence, RectTransform panel)
    {
        if (!haveCuesPlayedForThisState[gameState])
        {
           
            int count = 0;
            while (count < sequence.Length && panel.gameObject.activeInHierarchy)
            {
                if (!panel.gameObject.GetComponent<AudioSource>().isPlaying)
                {
                    panel.gameObject.GetComponent<AudioSource>().PlayOneShot(sequence[count]);
                    count++;
                
                }
                
                haveCuesPlayedForThisState[gameState] = true;
                
                yield return null;
            }
            
            //reset the array
            for (int i = 0; i < haveCuesPlayedForThisState.Length; i++)
            {
                if (!(i == gameState))
                {
                    haveCuesPlayedForThisState[i] = false;
                }
            }
            
        }
        else
        {
            
            yield return null;
        }
        


    }
    IEnumerator PlayAudioSequence(AudioClip[] sequence)
    {
        if (!isAudioPlaying)
        {
            isAudioPlaying = true;
            int count = 0;
            while (count < sequence.Length)
            {
                if (!GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().PlayOneShot(sequence[count]);
                    count++;
                }
                yield return null;
            }
            isAudioPlaying = false;
        }
        else
        {
            yield return null;
        }


    }
}