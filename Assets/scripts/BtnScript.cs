using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class BtnScript : MonoBehaviour
{
    public int levelNumOfBtn;
  
    public bool locked;
    public Image lockImg;

    void Start()
    {
        locked = true;

    }
    public void playLevel()
    {
       // if (!locked)
        {
            SoundManager.instance.playSound(0);
            ChooseDifficulty.num = levelNumOfBtn;
            SceneManager.LoadScene("PuzzleGame");

        }   
    }
}
