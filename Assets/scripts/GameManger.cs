using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.UI;

using System;

public class GameManger : MonoBehaviour
{
    public int counter = 0;
    
    public static GameManger instance;
    public bool gameCheck = false;
    public bool availableHint = false;
    public GameObject imgParnet;
    //public int tempVar=0;
    public GameObject WinPanel;
    public GameObject LosePanel;
    AudioSource audioSource;
    public AudioClip[] audioClips;
    public Text hintBtnText;
    public GameObject timer;
    public GameObject hintBtn;
    public GameObject hintRewardBtn;
    public GameObject fullImage;
    public GameObject questionMark;
    public GameObject rewardQuestionMark;
    public Text questionMarkText;
    static int temp = 0;
    public bool enableRewardedHint;
    static bool allDownloaded = false;
    public RewardTypes rewardType;

   
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        counter = 0;
        hintBtnText.text = PlayerPrefs.GetInt("puzzleHint",2).ToString();
        questionMarkText.text = PlayerPrefs.GetInt("questionMarkHint",2).ToString();

        LoadingNextImages();
        audioSource = GetComponent<AudioSource>();
        Application.targetFrameRate = 60;

    }
    void Update()
    {
        GameOver();
        EnablingRewardedHint();
        EnablingRewardedQM();
    }
    void LoadingNextImages()
    {
        if (temp >= ChooseDifficulty.num && temp < DataRetriever.retrievedNumber)
        {
            temp += 2;
            Debug.Log(temp + " temp");
            ImageLoader.startInd = ImageLoader.EndInd;
            ImageLoader.EndInd += 2;
            ImageLoader.Instance.LoaMoreImages();
            Debug.Log("we load img froms start ");
        }
    }

    // Update is called once per frame
    
    public void RepalyGame() 
    {
        SoundManager.instance.playSound(0);
        SceneManager.LoadScene("PuzzleGame");
    }

    public void BackBtnClicked()
    {
        SoundManager.instance.playSound(0);
        SceneManager.LoadScene("DifficultySel");
    }
    public void GameOver()
    {
       
        if (counter >= SpriteDivider.totalPieces && gameCheck)
        {
             //Debug.Log("now we check for game over ");;
            for (int i = 0; i < SpriteDivider.totalPieces; i++)
            {
                SpriteDivider.instance.pieceList[i].GetComponent<BoxCollider2D>().enabled = false;
            }
            timer.SetActive(false);
            //Debug.Log("game is over");
            gameCheck = false;
            questionMark.SetActive(false);
            fullImage.SetActive(false);
            StartCoroutine(MakeDelay());

        }
    }
    
    IEnumerator MakeDelay()
    {
        yield return new WaitForSeconds(1.5f);
        ImageEnlargeOnGameOver();
        yield return new WaitForSeconds(4f);

      //  AdsManager.Instance.ShowAd(AdScenes.LevelComplete.ToString());

        imgParnet.SetActive(false);
        WinPanel.SetActive(true);

    }
    void ImageEnlargeOnGameOver()
    {
        audioSource.PlayOneShot(audioClips[0]);
        imgParnet.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1f);
        if (ChooseDifficulty.num<DataRetriever.retrievedNumber)
        {
            ChooseDifficulty.num++;
            //ChooseDifficulty.instance.unlockLevels++;
            PlayerPrefs.SetInt("unlockImg", ChooseDifficulty.num);

        }
        else
        {
            ChooseDifficulty.num=0;
            allDownloaded = true;
        }
    }
    public void RewardedHint()
    {
        rewardType =  RewardTypes.SlideHint;
       // AdsManager.Instance.ShowAd(AdScenes.RewardAdButton.ToString());
    }
    public void RewardedQM()
    {
        rewardType = RewardTypes.QmHint;
       // AdsManager.Instance.ShowAd(AdScenes.RewardAdButton.ToString());
    }
    void EnablingRewardedHint()
    {
        if (PlayerPrefs.GetInt("puzzleHint", 2) > 0)
        {

            hintRewardBtn.SetActive(false);
            hintBtn.SetActive(true);
        }
        else
        {
            hintRewardBtn.SetActive(true);
            hintBtn.SetActive(false);
        }
    }
    void EnablingRewardedQM()
    {
        if (PlayerPrefs.GetInt("questionMarkHint", 2) > 0)
        {

            rewardQuestionMark.SetActive(false);
            questionMark.SetActive(true);
        }
        else
        {
            rewardQuestionMark.SetActive(true);
            questionMark.SetActive(false);
        }
    }
    public void DisplayImageOnQM()
    {
        int var = PlayerPrefs.GetInt("questionMarkHint", 2);
        var--;

        PlayerPrefs.SetInt("questionMarkHint", var);
        questionMarkText.text = var.ToString();

        SoundManager.instance.playSound(0);
        imgParnet.SetActive(false);
        fullImage.GetComponent<Image>().enabled = true;
        fullImage.transform.DOScale(new Vector3(4, 4, 0), 1f);
        StartCoroutine(BackToSMLImage());
    }
    public IEnumerator BackToSMLImage()
    {
        yield return new WaitForSeconds(3);
        fullImage.transform.DOScale(new Vector3(0, 0, 0), 1f);
        yield return new WaitForSeconds(1);
        imgParnet.SetActive(true);
    }
    public async void GiveHint()
    {
        int tempVar = PlayerPrefs.GetInt("puzzleHint", 2);
       
        //SoundManager.instance.playSound(0);
        if (availableHint && tempVar >= 1)
        {
            tempVar--;
            
            PlayerPrefs.SetInt("puzzleHint", tempVar);
            hintBtnText.text = tempVar.ToString();
           

            SpriteDivider spriteDivider = SpriteDivider.instance; // Cache the SpriteDivider component

            for (int i = 0; i < SpriteDivider.totalPieces; i++)
            {
                DragingPieces draggingPieces = spriteDivider.pieceList[i].GetComponent<DragingPieces>();

                if (!draggingPieces.DontMove)
                {
                    BoxCollider2D boxCollider = spriteDivider.pieceList[i].GetComponent<BoxCollider2D>();
                    boxCollider.enabled = false;
                    spriteDivider.pieceList[i].GetComponent<SpriteRenderer>().sortingOrder = 4;

                    for (int j = 0; j < SpriteDivider.totalPieces; j++)
                    {
                        if (draggingPieces.CorrectPos == spriteDivider.pieceList[j].transform.position)
                        {
                            BoxCollider2D targetCollider = spriteDivider.pieceList[j].GetComponent<BoxCollider2D>();
                            targetCollider.enabled = false;
                            spriteDivider.pieceList[j].GetComponent<SpriteRenderer>().sortingOrder = 4;
                            spriteDivider.pieceList[j].transform.DOMove(spriteDivider.pieceList[i].transform.position, .6f);
                            await Task.Delay(600);
                            targetCollider.enabled = true;
                            spriteDivider.pieceList[j].GetComponent<SpriteRenderer>().sortingOrder = 1;
                        }
                    }

                    audioSource.PlayOneShot(audioClips[1]);
                    spriteDivider.pieceList[i].transform.DOMove(draggingPieces.CorrectPos, .6f);
                    boxCollider.enabled = true;
                    spriteDivider.pieceList[i].GetComponent<SpriteRenderer>().sortingOrder = 1;
                    break;                      
                }
            }
        }
    }
    public void MoreGamesBtnClicked()
    {
        //SoundManager.instance.soundPlayers.buttonAudioSource.PlayOneShot(SoundManager.instance.gameSounds.ButtonSounds[0]);
        SoundManager.instance.playSound(0);
        Application.OpenURL("https://play.google.com/store/apps/developer?id=PhantomPlay");

    }
    public void RateUsBtnClicked()
    {
        SoundManager.instance.playSound(0);
        //SoundManager.instance.soundPlayers.buttonAudioSource.PlayOneShot(SoundManager.instance.gameSounds.ButtonSounds[0]);

        Application.OpenURL("https://play.google.com/store/apps/details?id=com.phantomplay.horror.story");

    }

}

[Serializable]
public enum RewardTypes
{
    SlideHint,
    QmHint
}
