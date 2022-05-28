using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameFlowCue : MonoBehaviour
{
    [Header("Game Flow Audio Cue")]

    //Main Menu
    public AudioClip gameMode_DemoMode;
    public AudioClip welcomeToMainMenu;
    
    //L/R Hand Selection
    public AudioClip LRHanded; // are you left or right handed?
    public AudioClip LRSelection; //choose left or right

    //Skybox Selection
    public AudioClip chooseDNMode; //choose day or night mode
    public AudioClip dayNight; // Do you want a day or night environment?

    //Target Color Selection
    public AudioClip BYorPGdayMode; //Blue and yellow or pink and green for day mode
    public AudioClip WRorPGnightMode; //Whire and red or pink and green for night mide
    public AudioClip chooseTargetColors;
    public AudioClip enteringDemoMode;
    public AudioClip enteringGameMode;

    //Demo Mode cues
    public AudioClip demoMode; // you are now in demo mode
    public AudioClip leftHandedBow; // left handed
    public AudioClip rightHandedBow; // right handed
    public AudioClip RHloadorReload; //right handed load/reload
    public AudioClip LHloadorReload; //left handed load/reload
    public AudioClip targetScores; //100, 50, 20 points
    public AudioClip targetSoundCue; //you will hear a guidance...

    //Game Mode cues
    public AudioClip thirtySecsLeft;

    //End of Game
    public AudioClip creditsorMainMenu;
    public AudioClip wellDone;

    //Credits
    public AudioClip credits;
}
