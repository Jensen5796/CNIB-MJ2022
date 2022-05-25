using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static float elapsedTime = 180;
    //public static float durationOfRound = 180;

    // Start is called before the first frame update
    // public HealthBar healthbar;
    void Start()
    {
       //elapsedTime = durationOfRound;
    }

    private void Awake()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        elapsedTime -= Time.deltaTime;
        SumScore.UpdateCount(elapsedTime);
        if (elapsedTime <= 0 && CanvasManager.gameStateInitiated[6])
        {
            CanvasManager.gameState = 7;
        }
        
        //healthbar.Tick(elapsedTime);
    }

    public static float getElapsedTime()
    {
        return elapsedTime;
    }
    //public static float getDurationOfRound()
    //{
    //    return durationOfRound;
    //}

}

