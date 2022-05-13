using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;


public class ControllerTesting : MonoBehaviour
{
    Text testing;
    ActionBasedController controller;

    private void Awake()
    {        
        GameObject go = GameObject.Find("Text");
        testing = go.GetComponent<Text>();
        testing.text = "Controller Test...";

        controller = GetComponent<ActionBasedController>();
    }
    private void Update()
    {        
        if (controller.selectInteractionState.active)
        {
            testing.text = controller.gameObject.name;
        }
    }

}
