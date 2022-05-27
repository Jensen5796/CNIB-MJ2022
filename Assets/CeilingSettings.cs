using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingSettings : MonoBehaviour
{
    public static bool isCeilingActive;
    private bool inDemoMode;
    private BoxCollider ceiling;


    // Start is called before the first frame update
    void Start()
    {
        ceiling = GameObject.Find("settings").GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        updateCeilingStatus();

        Arrow[] arrows = ceiling.GetComponentsInChildren<Arrow>();
        if (arrows.Length > 0)
        {
            //if any arrows are found as children of ceiling, execute action based in current game mode

            if (inDemoMode)
            {
                endDemoMode();
            }
            else
            {
                showInGameSettings();
            }

        }


    }
    
    private void updateCeilingStatus()
    {
        //game state 5 is demo mode; game state 6 is game mode
        if (CanvasManager.gameState == 5 || CanvasManager.gameState == 6)
        {
            isCeilingActive = true;
            if (CanvasManager.gameState == 5)
            {
                inDemoMode = true;
            }
            else
            {
                inDemoMode =false;
            }
        }
        else
        {
            isCeilingActive = false;
        }
    }

    private void endDemoMode()
    {
        CanvasManager.DisableGameComponentsWrapper(); //should also destroy demo target because it has "Target" tag
        CanvasManager.gameState = 1;
        CanvasManager.showMainMenuPanel = true;
        CanvasManager.inDemoMode = false;

    }
    private void showInGameSettings()
    {
        CanvasManager.inSettingsDuringGame = true;
        GameTimer.isPaused = true;
        CanvasManager.DisableGameComponentsWrapper();
        CanvasManager.gameState = 2;
        CanvasManager.showHandSelection = true;
        
    }
}
