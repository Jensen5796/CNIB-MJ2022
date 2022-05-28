using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPrefabHolder : MonoBehaviour
{
    public GameObject LeftDayTarget;
    public GameObject RightDayTarget;
    public GameObject LeftNightTarget;
    public GameObject RightNightTarget;

    private static GameObject ReturnedLeftDayTarget;
    private static GameObject ReturnedRightDayTarget;
    private static GameObject ReturnedLeftNightTarget;
    private static GameObject ReturnedRightNightTarget;



    // Start is called before the first frame update
    void Start()
    {
    ReturnedLeftDayTarget = LeftDayTarget;
    ReturnedRightDayTarget = RightDayTarget;
    ReturnedLeftNightTarget = LeftNightTarget;
    ReturnedRightNightTarget = RightNightTarget;
}

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject getLeftDayTarget()
    {
        return ReturnedLeftDayTarget;
    }
    public static GameObject getRightDayTarget()
    {
        return ReturnedRightDayTarget;
    }
    public static GameObject getLeftNightTarget()
    {
        return ReturnedLeftNightTarget;
    }
    public static GameObject getRightNightTarget()
    {
        return ReturnedRightNightTarget;
    }
}
