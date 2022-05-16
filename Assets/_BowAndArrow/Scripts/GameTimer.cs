using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
   
    public GameObject timerEnded;
    public GameObject scoreBoard;
    public GameObject timerBar;

    public GameObject MainMenu;
    public GameObject Credits;
    
    public static float elapsedTime = 5;
    // Start is called before the first frame update
   // public HealthBar healthbar;
    void Start()
    {
        timerEnded = GameObject.Find("MenuCredits");
        scoreBoard = GameObject.Find("sumScore");
        timerBar = GameObject.Find("health");

        MainMenu = GameObject.Find("Main Menu");
        
        timerEnded.SetActive(false);
        MainMenu.SetActive(false); 

    }

    private void Awake()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        elapsedTime -= Time.deltaTime;


        if (elapsedTime <= 0) 
        {
            RestartGame();
        }

        SumScore.UpdateCount(elapsedTime);

        //healthbar.Tick(elapsedTime);
    }
    public void RestartGame()
    {

        SceneManager.LoadScene("TimerEnds");
        //timerEnded.SetActive(true);
        scoreBoard.SetActive(false);
        timerBar.SetActive(false);

    }
}

