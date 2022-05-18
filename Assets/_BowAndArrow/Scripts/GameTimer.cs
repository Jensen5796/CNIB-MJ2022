using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static float elapsedTime = 20;
    // Start is called before the first frame update
   // public HealthBar healthbar;
    void Start()
    {
        GameObject.Find("CanvasManager").SetActive(false);
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
        if (elapsedTime <= 0)
        {
            endTimeTest();
        }
    }

    void endTimeTest()
    {
        GameObject.Find("TestCube").SetActive(false);
        

    }

    public static float getElapsedTime()
    {
        return elapsedTime;
    }

}

