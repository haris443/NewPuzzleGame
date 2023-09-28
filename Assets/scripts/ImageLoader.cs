using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.IO;
using Firebase.Database;

using UnityEngine.UI;


public class ImageLoader : MonoBehaviour
{
    public static ImageLoader Instance;
    public Sprite defaultImage;
    FirebaseStorage storage;
    StorageReference storageReference;
    public bool isImagesLoaded = false;
    public static int startInd = 0;
    public static int EndInd = 2;
   // public static int imageNumber = 0;
    private string localImagesPath; // Path to store the images locally
    public List<Sprite> sprites = new List<Sprite>();
    DatabaseReference reference;

    private void Awake()
    {
        Instance = this;
        
        //EndInd = PlayerPrefs.GetInt("unlockImg",2);
       // Debug.Log(EndInd+"end index val");
        //Debug.Log(PlayerPrefs.GetInt("unlockImg") + "  unlockImage value");
        // Initialize Firebase storage
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://puzzle-game-d897c.appspot.com/PuzzleGameImages");

        localImagesPath = Application.persistentDataPath;

        // Call the method to download images on game start
        DownloadImagesOnStart();

    }

    // Call this method to download images on game start
    void DownloadImagesOnStart()
    {
       // Debug.Log("Attempting to download images...");
        StartCoroutine(LoadLocalImages());
     

    }

   IEnumerator LoadLocalImages()
    {
        isImagesLoaded = false;
        sprites.Clear();
        int temp = PlayerPrefs.GetInt("unlockImg", 2)+2;
        for (int startInd = 0;startInd<temp;startInd++)
        {
            string localImagePath = Path.Combine(localImagesPath, Path.GetFileName("image" + startInd +".jpg"));

            if (File.Exists(localImagePath))
            {
                //Debug.Log("Image exists locally. Loading from local path: " + localImagePath);
                Texture2D texture = LoadLocalTexture(localImagePath);
                if (texture != null)
                {
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    sprites.Add(sprite);
                    
                }
                // SETTING how many images we have
                //PlayerPrefs.SetInt("unlockImg", EndInd);
            }
            else
            {
                // Get reference to the image
               // StorageReference imageReference = storage.GetReferenceFromUrl(imageUrls[startInd]);
                //StorageReference imageReference = storageReference.Child(Path.GetFileName(imageUrls[startInd])); from git
                StorageReference imageReference = storageReference.Child(Path.GetFileName( "image" + startInd + ".jpg"));
               //Debug.Log("********        " +  imageReference);
                // Get the download URL of the file
                imageReference.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                    {
                        string downloadUrl = task.Result.ToString();
                       // Debug.Log("Downloading image from URL: " + downloadUrl);
                        StartCoroutine(LoadImageAndAssignToSprite(downloadUrl, localImagePath));
                    }
                    else
                    {
                        Debug.LogError("Failed to get download URL: " + task.Exception);
                    }
                });
            }

        }
       
        yield return null;
        StartCoroutine(StartGame());
        // All images have been loaded
        isImagesLoaded = true;
    }

    IEnumerator LoadImageAndAssignToSprite(string mediaUrl, string localImagePath)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("Error downloading image: " + request.error);
        }
        else
        {
            // Convert the downloaded texture to a sprite
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            sprites.Add(sprite);

            // Save the downloaded image locally
            File.WriteAllBytes(localImagePath, texture.EncodeToPNG());

            

        }
    }

    Texture2D LoadLocalTexture(string localImagePath)
    {
        Texture2D texture = new Texture2D(2, 2); // Create a new texture
        byte[] bytes = File.ReadAllBytes(localImagePath); // Read the image bytes from the file
        if (texture.LoadImage(bytes)) // Load the image data into the texture
            return texture;
        else
            return null;
    }


    IEnumerator StartGame()
    {
        if (sprites.Count > 0 )
        {
            // Debug.Log(imageNumber + "*******");
            // Assign the sprite to the SpriteRenderer component
            
            SpriteDivider.instance.originalSprite = sprites[ChooseDifficulty.num];
            GameManger.instance.fullImage.transform.GetChild(0).GetComponent<Image>().sprite = sprites[ChooseDifficulty.num];
           
          
        }
        if(sprites.Count <= 0)
        {
            SpriteDivider.instance.originalSprite = defaultImage;
            Debug.Log("we allot default image");
            GameManger.instance.fullImage.transform.GetChild(0).GetComponent<Image>().sprite = defaultImage;
        }
            SpriteDivider.instance.GenratingPieces();
            yield return new WaitForSeconds(1.5f);
            SpriteDivider.instance.SwappingPos();
            yield return new WaitForSeconds(5f);
            GameManger.instance.gameCheck = true;
            GameManger.instance.availableHint = true;
    }

    public void LoaMoreImages()
    {
 
        for (int i = startInd; i < EndInd; i++)
        {
         string localImagePath = Path.Combine(localImagesPath, Path.GetFileName("image" + i + ".jpg"));
            // Get reference to the image
            //StorageReference imageReference = storage.GetReferenceFromUrl(imageUrls[i]); 
            StorageReference imageReference = storageReference.Child(Path.GetFileName("image" + i + ".jpg")); 
            /*StorageReference imageReference = storageReference;
            imageReference.sn*/

            // Get the download URL of the file
            imageReference.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                {
                    string downloadUrl = task.Result.ToString();
                    //Debug.Log("Downloading image from URL: " + downloadUrl);
                    StartCoroutine(LoadImageAndAssignToSprite(downloadUrl, localImagePath));
                    // SETTING how many images we have
                   // PlayerPrefs.SetInt("unlockImg", EndInd+3);
                    Debug.Log("");
                }
                else
                {
                    Debug.LogError("Failed to get download URL: " + task.Exception);
                }
            });
        }
    }
}



