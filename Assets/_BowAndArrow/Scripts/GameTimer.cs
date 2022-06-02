using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static bool isPaused = false;
    public static float elapsedTime;
    public static float durationOfRound = 180;
    //3 min = 180 sec

    // Start is called before the first frame update
    // public HealthBar healthbar;
    void Start()
    {
       elapsedTime = durationOfRound;
    }

    private void Awake()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            elapsedTime -= Time.deltaTime;
            SumScore.UpdateTime(elapsedTime);
            if (elapsedTime <= 0)
            {
                CanvasManager.gameState = 7;
                CanvasManager.showEndGamePanel = true;
                CanvasManager.disableGameComponents = true;
                //CanvasManager.inGameMode = false;
            }
        }
        
        
        //healthbar.Tick(elapsedTime);
    }

    public static float getElapsedTime()
    {
        return elapsedTime;
    }
    public static float getDurationOfRound()
    {
        return durationOfRound;
    }
    public static void resetElapsedTime()
    {
        elapsedTime = durationOfRound;
    }

}

