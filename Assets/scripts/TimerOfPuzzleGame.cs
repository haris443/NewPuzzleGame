using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerOfPuzzleGame : MonoBehaviour
{
    public static TimerOfPuzzleGame Instance;
    public Text timerText;
    bool timeOver = true;
    public static float totalTime = 60.0f; // Total countdown time in seconds
    private float currentTime;

    private void Awake()
    {
        Instance = this;
        if (SpriteDivider.totalPieces == 9)
        {
            totalTime = 122f;
        }
        else if (SpriteDivider.totalPieces == 25)
        {
            totalTime = 241f;
        }
        else if (SpriteDivider.totalPieces == 49)
        {
            totalTime = 300f;
        }
    }
    private void Start()
    {

        currentTime = totalTime;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {

            currentTime = 0;
            SceneRelod();


            // Handle timer completion here (e.g., show a message, trigger an event, etc.)
        }

        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = timerString;
    }

    private void SceneRelod()
    {
        if (timeOver)
        {
            timeOver = false;
            //player fails to complete within time 
            GameManger.instance.imgParnet.SetActive(false);
            GameManger.instance.fullImage.SetActive(false);
            GameManger.instance.questionMark.SetActive(false);
            GameManger.instance.LosePanel.SetActive(true);


            //AdsManager.Instance.ShowAd(AdScenes.LevelFailed.ToString());
            
            
            // SceneManager.LoadScene("PuzzleGame");
            //GameManger.instance.BackButtonClcicked();
        }

    }
}
