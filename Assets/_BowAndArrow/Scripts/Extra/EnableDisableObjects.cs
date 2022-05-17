using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EnableDisableObjects : MonoBehaviour
{

    public GameObject scene;
    public Material day_skybox;
    public Material night_skybox;
    public Material timerEnds_skybox;
    public Material mainMenu_skybox;

    // Start is called before the first frame update
    void Start()
    {
        scene = GameObject.Find("Scene");
        
    }

    // Update is called once per frame
    void disableObject ()
    {
        //scene.SetActive(false);
        

    }

    private void enableObject()
    {
        //scene.SetActive(true);
        //make a statement where the skybox will be picked based on the scene/panel
        RenderSettings.skybox = day_skybox;
    }
}
