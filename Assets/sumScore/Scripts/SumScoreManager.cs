using UnityEngine;
using UnityEngine.UI;

/// <summary>Manager for SumScore accessible from inspector</summary>
/// <remarks>
/// Attach to game object in scene. 
/// This is a singleton so only one instance can be active at a time.
/// </remarks>
public class SumScoreManager : MonoBehaviour {

    public static SumScoreManager instance = null;  // Static instance for singleton

    public int initialScore = 0;
    public bool storeHighScore = true, allowNegative = true;
    public Text field; // Text field displaying current score
    public Text highScoreField; // Text field displaying high score
    public SoundCues soundcue;

    public bool thousand = false;
    public bool hundred = false;
    public bool ten = false;
    public bool currentScore = false;
    public bool total = false;
    public bool countdown = false;

    public int xTimer;
    public int yTimer;
    void Awake() {

        soundcue = GetComponent<SoundCues>();

        // Ensure only one instance is running
        if (instance == null)
            instance = this; // Set instance to this object
        else
            Destroy(gameObject); // Kill yo self
        // Make sure the linked references didn't go missing
        if (field == null)
            Debug.LogError("Missing reference to 'field' on <b>SumScoreManager</b> component");
        if (storeHighScore && highScoreField == null)
            Debug.LogError("Missing reference to 'highScoreField' on <b>SumScoreManager</b> component");
    }

    void Start() {
        SumScore.Reset(); // Ensure score is 0 when object loads
        if (initialScore != 0)
            SumScore.Add(initialScore);  // Set initial score
        if (storeHighScore) {
            if (PlayerPrefs.HasKey("sumHS")) { 
                // Set high score value and tell manager
                SumScore.HighScore = PlayerPrefs.GetInt("sumHS");
                //UpdatedHS();
            }
            else
                SumScore.HighScore = 0;
        }

        Updated(); // Set initial score in UI
    }

    /// <summary>Notify this manager of a change in score</summary>
    public void Updated () {
        field.text = SumScore.Score.ToString("0"); // Post new score to text field


    }

    /// <summary>Notify this manager of a change in high score</summary>
    public void UpdatedHS (float elapsed) {
       /* if (elapsed < 10)
        {
            highScoreField.Color = Color.Red;
        }*/
        if (elapsed < 0)
        {
            //end game here
            //Destroy(gameObject);
            return;
        }
        //if(storeHighScore)
        // highScoreField.text = SumScore.HighScore.ToString("0"); // Post new high score to text field

        xTimer = (int)elapsed % 60;
        yTimer = (int)elapsed / 60;
        highScoreField.text = yTimer.ToString("00") + ":"+ xTimer.ToString("00");

        if (yTimer == 0 && xTimer == 10) 
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                if (!countdown)
                {
                    GetComponent<AudioSource>().PlayOneShot(soundcue.clip_tenSec);
                    countdown = true;
                }
            }
        }

        //two minute
        if (yTimer == 2 && xTimer == 3)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                if (!currentScore)
                {

                    GetComponent<AudioSource>().PlayOneShot(soundcue.clip_currentScore);

                    currentScore = true;

                }

            }

        }
        else if (yTimer == 2 && xTimer == 2)
        {
            if (!thousand)
            {

                scoringCue_greaterThan900();

                thousand = true;

            }
        }
        else if (yTimer == 2 && xTimer == 1)
        {
            if (!hundred)
            {

                scoringCue_greaterThan90_lessThan1000();

                hundred = true;

            }

        }
        else if (yTimer == 2 && xTimer == 0) 
        {
            if (!ten)
            {

                scoringCue_lessThan100();
                ten = true;

            }
        }

        //one minute
       else if (yTimer == 1 && xTimer == 3)
        {
            currentScore = false;
            thousand = false;
            hundred = false;
            ten = false;
            if (!GetComponent<AudioSource>().isPlaying)
            {
                if (!currentScore)
                {

                    GetComponent<AudioSource>().PlayOneShot(soundcue.clip_currentScore);

                    currentScore = true;

                }

            }

        }
        else if (yTimer == 1 && xTimer == 2)
        {
            if (!thousand)
            {

                scoringCue_greaterThan900();

                thousand = true;

            }
        }
        else if (yTimer == 1 && xTimer == 1)
        {
            if (!hundred)
            {

                scoringCue_greaterThan90_lessThan1000();

                hundred = true;

            }

        }
        else if (yTimer == 1 && xTimer == 0)
        {
            if (!ten)
            {

                scoringCue_lessThan100();
                ten = true;

            }
        }

        //10 sec
        else if (yTimer == 0 && xTimer == 4)
        {
            total = false;
            thousand = false;
            hundred = false;
            ten = false;
            if (!GetComponent<AudioSource>().isPlaying)
            {
                if (!currentScore)
                {

                    GetComponent<AudioSource>().PlayOneShot(soundcue.clip_totalScore);

                    total = true;

                }

            }

        }
        else if (yTimer == 0 && xTimer == 2)
        {
            if (!thousand)
            {

                scoringCue_greaterThan900();

                thousand = true;

            }
        }
        else if (yTimer == 0 && xTimer == 1)
        {
            if (!hundred)
            {

                scoringCue_greaterThan90_lessThan1000();

                hundred = true;

            }

        }
        else if (yTimer == 0 && xTimer == 0)
        {
            if (!ten)
            {

                scoringCue_lessThan100();
                ten = true;

            }

        }
    }

    public void scoringCue_lessThan100() 
    {
        
        int num = SumScore.Score;
        while (num >= 100) 
        {
            num -= 100;
        }

            if (num == 0)
            {
                if (yTimer == 0 && xTimer == 0)
                {
                    GetComponent<AudioSource>().PlayOneShot(soundcue.clip_keepPracticing);
                }
                else 
                {
                GetComponent<AudioSource>().PlayOneShot(soundcue.clip_keepTrying) ;
                }

            }
            else if (num == 10)
            {
                GetComponent<AudioSource>().PlayOneShot(soundcue.clip_10);
            }
            else if (num == 20)
            {
                GetComponent<AudioSource>().PlayOneShot(soundcue.clip_20);
            }
            else if (num == 30)
            {
                GetComponent<AudioSource>().PlayOneShot(soundcue.clip_30);
            }
            else if (num == 40)
            {
                GetComponent<AudioSource>().PlayOneShot(soundcue.clip_40);
            }
            else if (num == 50)
            {
                GetComponent<AudioSource>().PlayOneShot(soundcue.clip_50);
            }
            else if (num == 60)
            {
                GetComponent<AudioSource>().PlayOneShot(soundcue.clip_60);
            }
            else if (num == 70)
            {
                GetComponent<AudioSource>().PlayOneShot(soundcue.clip_70);
            }
            else if (num == 80)
            {
                GetComponent<AudioSource>().PlayOneShot(soundcue.clip_80);
            }
            else if (num == 90)
            {
                GetComponent<AudioSource>().PlayOneShot(soundcue.clip_90);
            }

        }
    public void scoringCue_greaterThan90_lessThan1000() 
    {
        int num = SumScore.Score;

        while (num >= 1000) 
        {
            num -= 1000;
        }
        if (num >= 100 && num < 200)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_100);
        }
        else if (num >= 200 && num < 300)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_200);
        }
        else if (num >= 300 && num < 400)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_300);
        }
        else if (num >= 400 && num < 500)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_400);
        }
        else if (num >= 500 && num < 600)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_500);
        }
        else if (num >= 600 && num < 700)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_600);
        }
        else if (num >= 700 && num < 800)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_700);
        }
        else if (num >= 800 && num < 900)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_800);
        }
        else if (num >= 900 && num < 1000)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_900);
        }
 
    }
    public void scoringCue_greaterThan900() 
    {

        if (SumScore.Score >= 1000 && SumScore.Score < 2000)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_1000);
        }
        else if (SumScore.Score >= 2000 && SumScore.Score < 3000)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_2000);
        }
        else if (SumScore.Score >= 3000 && SumScore.Score < 4000)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_3000);
        }
        else if (SumScore.Score >= 4000 && SumScore.Score < 5000)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_4000);
        }
        else if (SumScore.Score >= 5000 && SumScore.Score < 6000)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_5000);
        }
        else if (SumScore.Score >= 6000 && SumScore.Score < 7000)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_6000);
        }
        else if (SumScore.Score >= 7000 && SumScore.Score < 8000)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_7000);
        }
        else if (SumScore.Score >= 8000 && SumScore.Score < 9000)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_8000);
        }
        else if (SumScore.Score >= 9000 && SumScore.Score < 10000)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_9000);
        }
        else if (SumScore.Score >= 10000)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_10000);
        }
    }

    


}
