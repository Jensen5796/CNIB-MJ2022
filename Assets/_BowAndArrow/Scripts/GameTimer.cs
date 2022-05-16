using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static bool Restart = false;
    public GameObject TimerEnded;
    public static float elapsedTime = 5;
    // Start is called before the first frame update
   // public HealthBar healthbar;
    void Start()
    {
        TimerEnded = GameObject.Find("MenuCredits");
        TimerEnded.SetActive(false);
        
    }

    private void Awake()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        elapsedTime -= Time.deltaTime;


            RestartGame(elapsedTime);



        //healthbar.Tick(elapsedTime);
    }
    public void RestartGame(float elapsed)
    {
        if (elapsed == 0) 
        {
            TimerEnded.SetActive(true);
            Debug.Log("Menu Credits activated");
        }

        SumScore.UpdateCount(elapsedTime);
    }
}

