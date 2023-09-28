using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<AudioClip> audioClips;
    public AudioSource audioSource;
    public static SoundManager instance;
    void Start()
    {
         audioSource = this.gameObject.GetComponent<AudioSource>();
        if (instance!=null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    public void playSound(int i)
    {
        audioSource.PlayOneShot(audioClips[i]);
    }
    
}
