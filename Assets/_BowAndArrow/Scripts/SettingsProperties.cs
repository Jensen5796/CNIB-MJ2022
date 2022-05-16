using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsProperties : MonoBehaviour
{
    public char LeftOrRightHand;
    // player uses hand L or R
    public char DayOrNightScene;
    // scene setting D or N
    public char TargetColorAorB;
    // color option A or B

    // Start is called before the first frame update
    void Start()
    {
        //set some default values on the script
        LeftOrRightHand = 'R';
        DayOrNightScene = 'D';
        TargetColorAorB = 'A';
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
