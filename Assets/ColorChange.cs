using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    Color[] colors;
    Renderer thisRend;

    // Start is called before the first frame update
    void Start()
    {
        thisRend = GetComponent<Renderer>();
        colors = new Color[3];
        colors[0] = Color.white;
        colors[1] = Color.red;
        colors[2] = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeColor(char decision)
    {
        Start();
        if (decision == 'N')
        {
            thisRend.material.SetColor("_BaseColor", colors[0]);
        }
        else if (decision == 'L')
        {
            thisRend.material.SetColor("_BaseColor", colors[1]);
        } 
        else if (decision == 'R')
        {
            thisRend.material.SetColor("_BaseColor", colors[2]);
        }
        
    }
}
