using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float buttonRange = 1f;
    public float shitHitRange = 0.5f;
    public float missCastRange = 0.5f;
    public Transform buttonPosition;
    private Animator anim;
    public GameObject prefab300;
    public GameObject prefab100;
    public GameObject prefabMiss;
    private Scoreboard scoreboard;
    private Vector2 beginTouchPosition, endTouchPosition;
    private Touch touch;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        scoreboard = FindObjectOfType<Scoreboard>();
        audioManager = GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }
    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    beginTouchPosition = touch.position;
                    break;
                case TouchPhase.Ended:
                    endTouchPosition = touch.position;
                    if (beginTouchPosition == endTouchPosition)
                        HitNote(Notes.Direction.Up);
                    Debug.Log("init x" + beginTouchPosition.x + " y" + beginTouchPosition.y);
                    Debug.Log("end x" + endTouchPosition.x + " y" + endTouchPosition.y);
                    break;
            }

        }
    }
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HitNote(Notes.Direction.Up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            HitNote(Notes.Direction.Down);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HitNote(Notes.Direction.Left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HitNote(Notes.Direction.Right);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
    public void hitUp()
    {
        HitNote(Notes.Direction.Up);
    }
    public void hitDown()
    {
        HitNote(Notes.Direction.Down);
    }
    public void hiLeft()
    {
        HitNote(Notes.Direction.Left);
    }
    public void hitRight()
    {
        HitNote(Notes.Direction.Right);
    }
    void HitNote(Notes.Direction note)
    {
        audioManager.Play("swing");
        bool hasHit = false;
        anim.SetTrigger("Up");
        RaycastHit2D hit = Physics2D.Raycast(buttonPosition.position, Vector2.right, buttonRange);
        Debug.DrawRay(buttonPosition.position, Vector2.right,Color.green,0.1f);
        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponent<Notes>().Hit(note);
            hasHit = true;
            return;
        }
        RaycastHit2D shitHit = Physics2D.Raycast(buttonPosition.position, Vector2.right, buttonRange+shitHitRange);
        if (shitHit.collider != null && !hasHit)
        {
            shitHit.collider.gameObject.GetComponent<Notes>().ShitHit(note);
            hasHit = true;
            return;
        }
        RaycastHit2D missCast = Physics2D.Raycast(buttonPosition.position, Vector2.right, buttonRange + shitHitRange + missCastRange);
        if (missCast.collider != null && !hasHit)
        {
            missCast.collider.gameObject.GetComponent<Notes>().Miss();
            Miss();
        }
    }
    public void GoodHit()
    {
        prefab300.GetComponent<Animator>().SetTrigger("Start");
        audioManager.Play("hitsound");
        scoreboard.AddScore(300);
    }
    public void ShitHit()
    {
        prefab100.GetComponent<Animator>().SetTrigger("Start");
        audioManager.Play("hitsound");
        scoreboard.AddScore(100);
    }
    public void Miss()
    {
        prefabMiss.GetComponent<Animator>().SetTrigger("Start");
        scoreboard.Miss();
    }
}
