using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class AutoRotate : MonoBehaviour
{

    Text test;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject go = GameObject.Find("Text2");
        test = go.GetComponent<Text>();


        Transform thisTransform = GetComponent<Transform>();
        float angleToRotate = FindAngle();
        thisTransform.Rotate(0, angleToRotate, 0);

       
        test.text = "Rotated: " + angleToRotate.ToString();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float FindHypotenuse()
    {
        float z = transform.position.z;
        float x = transform.position.x;
        float hyp = Mathf.Sqrt(Mathf.Pow(z, 2) + Mathf.Pow(x, 2));
        return hyp;
    }

    private float FindAngle()
    {
        float angle = 0;
        float z = transform.position.z;
        float x = transform.position.x;

        //Quadrant 1
        if (x > 0 && z > 0)
        {
            angle = Mathf.Cos(z / FindHypotenuse());
            
        }
        //Quadrant 2
        if (x < 0 && z > 0)
        {
            angle = Mathf.Cos(z / FindHypotenuse());
            angle *= -1;
        }
        //Quadrant 3
        if (x < 0 && z < 0)
        {
            angle = Mathf.Cos(z / FindHypotenuse());
            angle += 90;
            angle *= -1;
        }
        //Quadrant 4
        if (x > 0 && z < 0)
        {
            angle = Mathf.Cos(z / FindHypotenuse());
            angle += 90;
        }

        return angle;
    }
}
