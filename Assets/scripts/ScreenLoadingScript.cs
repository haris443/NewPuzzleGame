using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public struct LoadingScreenUI
{
    public Image loadingBar;
    //public TextMeshProUGUI loadingBarPercentage;
}

public class ScreenLoadingScript : MonoBehaviour
{
    public static ScreenLoadingScript instance;

    public LoadingScreenUI loadingScreenUI;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(StartLoadingBar("DifficultySel"));
        
    }

    private void Update()
    {
       // loadingScreenUI.loadingBarPercentage.text = (int)(loadingScreenUI.loadingBar.fillAmount*100) + "%";
    }

    public IEnumerator StartLoadingBar(string sceneName)
    {
        float randomNum = 0;

        yield return new WaitForSeconds(1f);

        while (loadingScreenUI.loadingBar.fillAmount < 1)
        {
            randomNum = UnityEngine.Random.Range(randomNum, 1.1f);

            Tween operation=loadingScreenUI.loadingBar.DOFillAmount(randomNum, 1f);
            
            yield return new WaitForSeconds(2f);
        }

       if(sceneName.Equals("DifficultySel"))
       {
            //Debug.Log(sceneName);
            SceneManager.LoadScene(sceneName);
       }
        /*
        else
        {
            OrcBricks_UIManager.instance.GameStatesHandler(Enum.Parse<States>(sceneName));
        }*/
    }

}
