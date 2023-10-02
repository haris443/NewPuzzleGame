using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;

public class DragingPieces : MonoBehaviour
{
    public static DragingPieces instance;
    private bool isDragging = false;
    public bool check = true;
    public bool DontMove = false;
    private BoxCollider2D boxCollider;
    private Vector3 offset;
    private Vector3 originalPosition;
    private SpriteRenderer spriteRenderer;
    public Vector3 CorrectPos;
    private Animator animator;
    public AudioSource audioSource;
    public AudioClip swapSound; 
    public AudioClip beep;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        check = true;
        animator = GetComponent<Animator>();
        audioSource =transform.parent.GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        CorrectPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    

    public void Update()
    {
        
        CheckingPieceOnRightPos();
        if (isDragging && !DontMove)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)) + offset;
            transform.position = newPosition;

        }
    }
    public void CheckingPieceOnRightPos()
    {
        if (this.transform.position.x == CorrectPos.x && this.transform.position.y == CorrectPos.y && check == false)
        {
            correctPositiohEnlarging();
            //Debug.Log("we make true dontMove");
            DontMove = true;
            //this.GetComponent<BoxCollider2D>().enabled = false;
            check = true; // it is only used to make sure update dont repeat this cond again and 
            GameManger.instance.counter++;
        }

    }

    public void OnMouseDown()
    {
        if (DontMove == true)
        {
            animator.SetTrigger("jerk");
            //Debug.Log("anim is playing ");
            audioSource.PlayOneShot(beep);
        }

        spriteRenderer.sortingOrder = 03;
        isDragging = true;
        originalPosition = transform.position;
        this.transform.position = new Vector3(transform.position.x, transform.position.y, 1);

        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));


    }

    public async void OnMouseUp()
    {
        isDragging = false;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("tiles") && hit.collider.gameObject != gameObject && this.DontMove ==false && hit.collider.gameObject.GetComponent<DragingPieces>().DontMove == false)
        {
            audioSource.PlayOneShot(swapSound);
            boxCollider.enabled = false;
            hit.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;   
            /*transform.position = hit.collider.transform.position;
            hit.collider.transform.position = originalPosition;*/
            
            hit.collider.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 4;
            transform.DOMove(hit.collider.transform.position,.5f);
            hit.collider.transform.DOMove(originalPosition,.5f);
            await Task.Delay(500);

            boxCollider.enabled = true;
            hit.collider.gameObject.GetComponent<BoxCollider2D>().enabled = true;

            this.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            spriteRenderer.sortingOrder = 01;
            hit.collider.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            
        }
        else 
        {
            // Return to initial position if not dropped on another tile
            transform.position = originalPosition;
            this.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            spriteRenderer.sortingOrder = 01;
        }       
    }
    public async void correctPositiohEnlarging()
    {
        this.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), .5f);
        await Task.Delay(500);
        this.transform.DOScale(Vector3.one, .5f);
    }
}





