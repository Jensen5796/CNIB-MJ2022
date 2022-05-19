using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static float elapsedTime = 10;
    // Start is called before the first frame update
   // public HealthBar healthbar;
    void Start()
    {
       
    }

    private void Awake()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        elapsedTime -= Time.deltaTime;
        SumScore.UpdateCount(elapsedTime);
        //healthbar.Tick(elapsedTime);
    }

    public static float getElapsedTime()
    {
        return elapsedTime;
    }

}

