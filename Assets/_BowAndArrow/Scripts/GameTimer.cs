using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static float elapsedTime = 150;
    public int xTimer;
    public int yTimer;
    public SoundCues soundcue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        soundcue = GetComponent<SoundCues>();
    }
    // Update is called once per frame
    void Update()
    {
        elapsedTime -= Time.deltaTime;
        SumScore.UpdateCount(elapsedTime);

    }

}

