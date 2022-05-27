using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetManager : MonoBehaviour
{
    //This script manages the number of targets in play, generation of new targets, and auto-orienting them toward player

    //properties
    public int numberTargetsDesired;
    private int numberTargetsCurrent;

    public GameObject targetPrefab;
    public float yAxisOffset = 1.0f;
    public float timeToDestroyTarget;

    private float destroyTargetStartTime = 0;
    private bool destroyingTarget;
    private bool beginDestroyingTarget;

    private Text test;
    private Text test2;
    private bool[] targetsInQuadrants;
    public static bool restartTargetManager;

    private void Awake()
    {
        restartTargetManager = false;
        //set up debugging text
        //GameObject go = GameObject.Find("Text2");
        //test = go.GetComponent<Text>();
        GameObject go2 = GameObject.Find("TestText");
        test2 = go2.GetComponent<Text>();
        //test.text = "testing";

        //boolean array to track which quadrants have targets
        //using positions 1-4 to match with quadrants (0 will be false or empty)
        targetsInQuadrants = new bool[5] { false, false, false, false, false };
        targetPrefab = CanvasManager.getTargetSelection();
        //test2.text = targetPrefab.name;
        Start();

        
    }
    // Start is called before the first frame update
    void Start()
    {
        //generate desired number of targets
        for (int i = 0; i < numberTargetsDesired; i++)
        {
            generateTarget();
        }

        //debugging function to see what rotation the targets are generated in
        //testTargetPos();

    }

    // Update is called once per frame
    void Update()
    {
        if (restartTargetManager)
        {
            Awake();
            return;
        }
        //check to see if targets are hit
        GameObject[] activeTargets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject target in activeTargets)
        {

            Arrow[] arrows = target.GetComponentsInChildren<Arrow>();
            if (arrows.Length > 0)
            {
                //if any arrows are found as children of target, destroy target in 2 seconds
                
                destroyTarget(target);
                
            }
        }
        //if less than desired number, generate a target
        if (numberTargetsCurrent < numberTargetsDesired)
        {
            generateTarget();
        }
    }

    //debugging function
    private void testTargetPos()
    {
        //test.text = "";
        GameObject[] activeTargets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject target in activeTargets)
        {
            Quaternion rot = target.transform.rotation;
            float rotation = rot.eulerAngles.y;
            float xVal = target.transform.position.x;
            float zVal = target.transform.position.z;

            //test.text += "; rot=" + rotation;
        }
    }


    

    private bool generateTarget()
    {
        bool targetCreatedSuccessfully = false;

        // choose quadrant to generate for, return random angle from that quadrant's range
        int angle = findEmptyQuadrant();

        //set target to zero position and rotate it by angle
        Vector3 startPosition = Vector3.zero;
        startPosition.Set(0, yAxisOffset, 0);

        // Instantiate target
        Instantiate(targetPrefab, startPosition, Quaternion.Euler(0, angle, 0));
        // increase number of active targets
        numberTargetsCurrent++;
        targetCreatedSuccessfully = true;
        //testTargetPos();

        return targetCreatedSuccessfully;
    }

    private void destroyTarget(GameObject target)
    {
        //test.text = "in destroy function";
        float currentTime = Time.fixedTime;
        if (destroyTargetStartTime == 0)
        {
            //test.text = "in beginning of destroy function";
            
            destroyTargetStartTime = Time.fixedTime;
            destroyingTarget = true;
        }
        if (destroyingTarget == true)
        {
            //test.text = "in middle of destroy function; start time is " + destroyTargetStartTime.ToString() + " current time is: "+currentTime.ToString();
            //if current time - start time = 2 sec,
            if (currentTime - destroyTargetStartTime >= timeToDestroyTarget)
            {
                // get rotation for target
                Quaternion rot = target.transform.rotation;
                float rotation = rot.eulerAngles.y;

                // compare to find quadrant, set corresponding position in array to false
                if (rotation >= 0 && rotation < 90)
                {
                    //quadrant 1
                    targetsInQuadrants[1] = false;
                }
                else if (rotation >= 90 && rotation < 180)
                {
                    //quadrant 2
                    targetsInQuadrants[2] = false;
                }
                else if (rotation >= 180 && rotation < 270)
                {
                    //quadrant 3
                    targetsInQuadrants[3] = false;
                }
                else if (rotation >= 270 && rotation < 360)
                {
                    //quadrant 4
                    targetsInQuadrants[4] = false;
                }

                Destroy(target);
                numberTargetsCurrent--;
                destroyingTarget = false;
                destroyTargetStartTime = 0;
            }
            
        }

        // get rotation for target
        //Quaternion rot = target.transform.rotation;
        //float rotation = rot.eulerAngles.y;

        //// compare to find quadrant, set corresponding position in array to false
        //if (rotation >= 0 && rotation < 90)
        //{
        //    //quadrant 1
        //    targetsInQuadrants[1] = false;
        //}
        //else if (rotation >= 90 && rotation < 180)
        //{
        //    //quadrant 2
        //    targetsInQuadrants[2] = false;
        //}
        //else if (rotation >= 180 && rotation < 270)
        //{
        //    //quadrant 3
        //    targetsInQuadrants[3] = false;
        //}
        //else if (rotation >= 270 && rotation < 360)
        //{
        //    //quadrant 4
        //    targetsInQuadrants[4] = false;
        //}
        
        
        //Destroy(target, 2);
        //numberTargetsCurrent--;


    }
    IEnumerator waitToDestroyTarget()
    {
        yield return new WaitForSeconds(2);
    }


    private void destroyTarget2(GameObject target)
    {

        // get rotation for target
        Quaternion rot = target.transform.rotation;
        float rotation = rot.eulerAngles.y;

        // compare to find quadrant, set corresponding position in array to false
        if (rotation >= 0 && rotation<90)
        {            
            //quadrant 1
            targetsInQuadrants[1] = false;
        }
        else if (rotation >= 90 && rotation < 180)
        {
            //quadrant 2
            targetsInQuadrants[2] = false;
        }
        else if (rotation >= 180 && rotation < 270)
        {
            //quadrant 3
            targetsInQuadrants[3] = false;
        }
        else if (rotation >= 270 && rotation < 360)
        {
            //quadrant 4
            targetsInQuadrants[4] = false;
        }

        Destroy(target);
        numberTargetsCurrent--;

    }

    //private Vector3 calculateTargetPosition(int angle)
    //{
    //    //old function, was used in previous version


    //    Vector3 targetPosition = Vector3.zero;

    //    float xValue = Mathf.Cos(angle) * targetDistanceFromPlayer;
    //    float zValue = Mathf.Sin(angle) * targetDistanceFromPlayer;

    //    test2.text += "; xValue= " + xValue + "; zValue= " + zValue;
    //    targetPosition.Set(xValue, yAxisOffset, zValue);
    //    return targetPosition;
    //}

    
    private float findYRotationAngle2(float xValue, float zValue)
    {
        //old function, was used in previous version - not accurate!
        float angle;

        Vector3 player = Vector3.zero;
        player.Set(0, yAxisOffset, 0);
        Vector3 target = Vector3.zero;
        target.Set(xValue, yAxisOffset, zValue);
        Vector3 playerDirection = player - target;
        angle = Vector3.Angle(Vector3.back, playerDirection);

        if (xValue < 0 && zValue > 0) //q2
        {
            return (angle *= -1);
        }
        else if (xValue > 0 && zValue > 0) //q1
        {
            return angle;
        }
        else if (xValue < 0 && zValue < 0) //q3
        {
            return (angle);
        }
        else if (xValue > 0 && zValue < 0) //q4
        {
            return -(angle);
        }
        else //xValue = 0
        {
            return 0.0f;
        }
    }

    
    private int findEmptyQuadrant()
    {
        //finds first empty quadrant and returns a random angle in that quadrant
        int emptyQuadrant = 0;

        for (int i = 1; i < 5; i++)
        {
            if (targetsInQuadrants[i] == false)
            {
                emptyQuadrant = i;
                break;
            }
        }

        int angle = getRandomAngle(emptyQuadrant);
        return angle;
    }

    private int getRandomAngle(int quadrant)
    {
        int angle;
        switch (quadrant)
        {
            case 1:
                angle = Random.Range(0, 90);
                targetsInQuadrants[1] = true;
                break;
            case 2:
                angle = Random.Range(90, 180);
                targetsInQuadrants[2] = true;
                break;
            case 3:
                angle = Random.Range(180, 270);
                targetsInQuadrants[3] = true;
                break;
            case 4:
                angle = Random.Range(270, 360);
                targetsInQuadrants[4] = true;
                break;
            default:
                //angle = Random.Range(0, 360);
                angle = 0;
                break;
        }
        
        //test.text += "; q=" + quadrant + "; angle="+angle;
        return angle;
    }

    public static void RestartTargetManager()
    {
        restartTargetManager = true;
    }
}
