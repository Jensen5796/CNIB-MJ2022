using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCredits : MonoBehaviour
{
    public static bool Restart = false;
    public GameObject TimerEnded;

    public GameTimer elapsed;
    // Start is called before the first frame update
    void Start()
    {
        TimerEnded.SetActive(false);
    }
    public void RestartGame()
    {
        TimerEnded = GameObject.Find("MenuCredits");
        TimerEnded.SetActive(true);
        Debug.Log("Menu Credits activated");
    }
    // Update is called once per frame
    void Update()
    {
       

    }
}
