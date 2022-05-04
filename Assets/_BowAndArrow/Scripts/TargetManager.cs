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
    public float targetDistanceFromPlayer;

    private Text test;
    private bool[] targetsInQuadrants;

    private void Awake()
    {
        //set up debugging text
        GameObject go = GameObject.Find("Text2");
        test = go.GetComponent<Text>();

        //boolean array to track which quadrants have targets
        //using positions 1-4 to match with quadrants (0 will be false or empty)
        targetsInQuadrants = new bool[5] { false, false, false, false, false };

        //this.Start();

        
    }
    // Start is called before the first frame update
    void Start()
    {
        //generateTarget();

        //generate desired number of targets
        for (int i = 0; i < numberTargetsDesired; i++)
        {
            generateTarget();
        }

    }

    // Update is called once per frame
    void Update()
    {
        //check to see if targets are hit
        GameObject[] activeTargets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject target in activeTargets)
        {
            
            Arrow[] arrows = target.GetComponentsInChildren<Arrow>();
            if (arrows.Length > 0)
            {
                //if any arrows are found as children of target, destroy target in 2 seconds
                destroyTarget(target);
                Destroy(target);
                numberTargetsCurrent--;

                //if less than desired number, generate a target
                if (numberTargetsCurrent < numberTargetsDesired)
                {
                    generateTarget();
                }

            }
        }

        

    }

    private bool generateTarget()
    {
        bool targetCreatedSuccessfully = false;

        // randomly generate positions
        Vector3 targetPosition = getRandomPosition();

        // calculate rotation based on position
        float yRot = findYRotationAngle2(targetPosition.x, targetPosition.z);

        // Instantiate target
        Instantiate(targetPrefab, targetPosition, Quaternion.Euler(0, yRot, 0));
        numberTargetsCurrent++;
        targetCreatedSuccessfully = true;

        test.text = "x=" + targetPosition.x + "; z=" + targetPosition.z + "; rot=" + yRot + "; numTargs="+numberTargetsCurrent;
        return targetCreatedSuccessfully;

    }

    private void destroyTarget(GameObject target)
    {
        // get x and y values to determine which quadrant target was in
        float xVal = target.transform.position.x;
        float zVal = target.transform.position.z;

        // destroy target and decrement target counter
        //Destroy(target);
        //numberTargetsCurrent--;

        // compare to find quadrant, set corresponding position in array to false
        if (xVal > 0 && zVal > 0)
        {            
            //quadrant 1
            targetsInQuadrants[1] = false;
        }
        else if (xVal < 0 && zVal > 0)
        {
            //quadrant 2
            targetsInQuadrants[2] = false;
        }
        else if (xVal < 0 && zVal < 0)
        {
            //quadrant 3
            targetsInQuadrants[3] = false;
        }
        else if (xVal > 0 && zVal < 0)
        {
            //quadrant 4
            targetsInQuadrants[4] = false;
        }

        ////if less than desired number, generate a target
        //if (numberTargetsCurrent < numberTargetsDesired)
        //{
        //    generateTarget();
        //}

    }

    private Vector3 getRandomPosition()
    {
        //get random x value and calculate z value based on desired target distance
        
        float xValue = Random.Range(0, targetDistanceFromPlayer);
        float zValue = getZValue(xValue);
        Vector3 targetPosition = assignTargetQuadrant(xValue, zValue);

        return targetPosition;
    }
    private float getZValue(float xValue)
    {
        //note: this will always return a value in quadrant 1: will need to randomly assign quadrant elsewhere
        float zValue;
        //special cases: max targetDistanceFromPlayer was random value assigned to x
        if (xValue == targetDistanceFromPlayer)
        {
            zValue = 0.0f;
        }
        //special case: random value assigned to x is 0
        else if (xValue == 0)
        {
            zValue = (float)targetDistanceFromPlayer;
        }
        //otherwise use pythagoras to calculate missing leg in right angle triangle:
        else
        {
            zValue = Mathf.Sqrt(Mathf.Pow(targetDistanceFromPlayer, 2) - Mathf.Pow(xValue, 2));
        }
        return zValue;
    }
    private Vector3 assignTargetQuadrant(float xValue, float zValue)
    {
        Vector3 targetPosition = Vector3.zero;
        int quadrant = findEmptyQuadrant();
        switch (quadrant)
        {
            case 1:
                //quadrant 1: positive x, positive z
                targetPosition.Set(xValue, yAxisOffset, zValue);
                targetsInQuadrants[1] = true;
                return targetPosition;

            case 2:
                //quadrant 2: negative x, positive z
                targetPosition.Set(-xValue, yAxisOffset, zValue);
                targetsInQuadrants[2] = true;
                return targetPosition;

            case 3:
                //quadrant 3: negative x, negative z
                targetPosition.Set(-xValue, yAxisOffset, -zValue);
                targetsInQuadrants[3] = true;
                return targetPosition;

            case 4:
                //quadrant 4: positive z, negative z
                targetPosition.Set(xValue, yAxisOffset, -zValue);
                targetsInQuadrants[4] = true;
                return targetPosition;
        }

        return targetPosition;
    }

    //private float FindHypotenuse(float x, float z)
    //{
        
    //    float hyp = Mathf.Sqrt(Mathf.Pow(z, 2) + Mathf.Pow(x, 2));
    //    return hyp;
    //}

    private float findYRotationAngle2(float xValue, float zValue)
    {
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
            return (angle+180);
        }
        else if (xValue > 0 && zValue < 0) //q4
        {
            return -(angle+180);
        }
        else //xValue = 0
        {
            return 0.0f;
        }
    }

    //private float findYRotationAngle(float xValue, float zValue)
    //{
    //    float angle = 0;
    //    float z = zValue;
    //    float x = xValue;

    //    //Quadrant 1
    //    if (x > 0 && z > 0)
    //    {
    //        angle = Mathf.Cos(z / FindHypotenuse(x, z));

    //    }
    //    //Quadrant 2
    //    if (x < 0 && z > 0)
    //    {
    //        angle = Mathf.Cos(z / FindHypotenuse(x, z));
    //        angle *= -1;
    //    }
    //    //Quadrant 3
    //    if (x < 0 && z < 0)
    //    {
    //        angle = Mathf.Cos(z / FindHypotenuse(x, z));
    //        angle += 90;
    //        angle *= -1;
    //    }
    //    //Quadrant 4
    //    if (x > 0 && z < 0)
    //    {
    //        angle = Mathf.Cos(z / FindHypotenuse(x, z));
    //        angle += 90;
    //    }

    //    return angle;
    //}

    private int findEmptyQuadrant()
    {
        //finds first empty quadrant and returns it
        int emptyQuadrant = 0;

        for (int i = 1; i < 5; i++)
        {
            if (targetsInQuadrants[i] == false)
            {
                emptyQuadrant = i;
                break;
            }
        }

        return emptyQuadrant;
    }
}
