using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;

public class SpriteDivider : MonoBehaviour
{
    public Sprite originalSprite;
    public GameObject piecePrefab;
    public GameObject parentOfPieces;
    public static int totalPieces = 9;
    public AudioClip shuffle; 
    public static SpriteDivider instance;
    public bool check=true;
    public int  Counter=0;
    private AudioSource audioSource;
    public  List<int> result;
    public List<GameObject> pieceList;

    void Start()
    {
       

        //StartCoroutine(MakeDelay());
        audioSource = GetComponent<AudioSource>();
        getUniqueRandomArray(0, totalPieces, totalPieces);
        //GenratingPieces();
    }
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        

    }
    public void GenratingPieces()
    {
        pieceList = new List<GameObject>();
         
        int numRowsCols = Mathf.RoundToInt(Mathf.Sqrt(totalPieces)); // Calculate the number of rows and column
        int pieceWidth = originalSprite.texture.width / numRowsCols;
        int pieceHeight = originalSprite.texture.height / numRowsCols;
        float floatPieceWidth = originalSprite.texture.width / numRowsCols;

        Vector3 startPosition = transform.position + new Vector3(-((numRowsCols - 1) * pieceWidth) / 2f, ((numRowsCols - 1) * pieceHeight) / 2f, 0f);

         pieceList = new List<GameObject>();
        int pieceCount = 0;
        for (int row = 0; row < numRowsCols; row++)
        {
            //pieceList.Add(new List<GameObject>());

            for (int col = 0; col < numRowsCols; col++)
            {

                Texture2D pieceTexture = new Texture2D(pieceWidth, pieceHeight);
                pieceTexture.filterMode = FilterMode.Point;

                Color[] pixels = originalSprite.texture.GetPixels(col * pieceWidth, (numRowsCols - 1 - row) * pieceHeight, pieceWidth, pieceHeight);
                pieceTexture.SetPixels(pixels);
                pieceTexture.Apply();

                Material material = new Material(Shader.Find("Sprites/Default"));
                material.mainTexture = pieceTexture;

                float x = startPosition.x + col * pieceWidth;
                float y = startPosition.y - row * pieceHeight;

                Vector3 position = new Vector3(x, y, startPosition.z);

                GameObject piece = Instantiate(piecePrefab, parentOfPieces.transform);
                ++pieceCount;
                piece.name = pieceCount.ToString();
                pieceList.Add(piece);
                //pieceList[row].Add(piece);
                piece.AddComponent<BoxCollider2D>();
                piece.GetComponent<BoxCollider2D>().size = new Vector3(floatPieceWidth / 100, floatPieceWidth / 100 );
                //piece.GetComponent<BoxCollider2D>().size = new Vector2(pieceWidth / Camera.main.pixelWidth, pieceHeight / Camera.main.pixelHeight);
                SpriteRenderer spriteRenderer = piece.GetComponent<SpriteRenderer>();


                if (row < (numRowsCols / 2))
                {
                    piece.transform.position = new Vector3(piece.transform.position.x, (((numRowsCols / 2) - row) * floatPieceWidth / 100), piece.transform.position.z);

                }
                else if (row > (numRowsCols / 2))
                {
                    piece.transform.position = new Vector3(piece.transform.position.x, ((row - (numRowsCols / 2)) * (0 - (floatPieceWidth / 100))), piece.transform.position.z);
                }

                if (col < (numRowsCols / 2))
                {
                    piece.transform.position = new Vector3((((numRowsCols / 2) - col) * (0 - (floatPieceWidth / 100))), piece.transform.position.y, piece.transform.position.z);
                }
                else if (col > (numRowsCols / 2))
                {
                    piece.transform.position = new Vector3(((col - (numRowsCols / 2)) * floatPieceWidth / 100), piece.transform.position.y, piece.transform.position.z);
                }

                spriteRenderer.sprite = Sprite.Create(pieceTexture, new Rect(0, 0, pieceWidth, pieceHeight), new Vector2(0.5f, 0.5f));
                spriteRenderer.material = material;
            }
        }
    }

    public List<int> getUniqueRandomArray(int min, int max, int count)
    {
        result = new List<int>();
        List<int> numbersInOrder = new List<int>();
        for (int x = min; x < max; x++)
        {
            numbersInOrder.Add(x);
        }
        for (int x = 0; x < count; x++)
        {
            
            int randomIndex = UnityEngine.Random.Range(0, numbersInOrder.Count);
            
            result.Add(numbersInOrder[randomIndex]);
            numbersInOrder.RemoveAt(randomIndex);
        }
        int temp = result[(max - 1) / 2];
        int index = result.FindIndex(x => x == (max - 1) / 2);
        result[(max - 1) / 2] = result[index];
        result[index] = temp;
        //Debug.Log((max-1)/2);

        // for freezzing tiles left and right from center upon their position
        if (totalPieces > 9)
        {
            int tempLeft = result[((max - 1) / 2) - 1];
            int indexLeft = result.FindIndex(x => x == ((max - 1) / 2) - 1);
            result[((max - 1) / 2) - 1] = result[indexLeft];
            result[indexLeft] = tempLeft;

            int tempRight = result[((max - 1) / 2) + 1];
            int indexRight = result.FindIndex(x => x == ((max - 1) / 2) + 1);
            result[((max - 1) / 2) + 1] = result[indexRight];
            result[indexRight] = tempRight;
        }



        return result;
    }
    public void SwappingPos()
    {
        audioSource.PlayOneShot(shuffle);
        for (int i = 0; i < totalPieces; i++)
        {
            //Debug.Log(result[i]); 



            Vector3 tempPos1 = pieceList[i].transform.position;
            Vector3 tempPos2 = pieceList[result[i]].transform.position;
            pieceList[result[i]].transform.DOMove(tempPos1, 1f);

            if (i == totalPieces/2 )
            {
                pieceList[i].GetComponent<BoxCollider2D>().enabled = false;
                //pieceList[i].GetComponent<SpriteRenderer>().sortingOrder =5;
            }

        }
        //SpriteDivider.instance.check = false;
        StartCoroutine(MakingFalseCheckInPiece());
      //  MakingFalseCheckInPiece();
        //pieceList[((totalPieces / 2) + 1)].transform.position = Vector3.zero;
    }

    IEnumerator MakingFalseCheckInPiece()
    {
        yield return new WaitForSeconds(2f);
       // Debug.Log("MakingFalseCheckInPiece");
        for (int i = 0; i < totalPieces; i++)
        {
           pieceList[i].GetComponent<DragingPieces>().check = false;  
           pieceList[i].GetComponent<BoxCollider2D>().enabled = true;  
        }

    }
}
