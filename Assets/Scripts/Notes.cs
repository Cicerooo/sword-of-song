using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour
{
    public Vector2 SpawnPos;
    public Vector2 HitPos;
    public Vector2 RemovePos;
    public Vector2 TargetPos;
    public float beatOfThisNote;
    private float songPosInBeats;
    private int BeatsShownInAdvance;
    private Conductor conductor;
    public enum Direction{Up,Down,Left,Right}
    public Direction direction = new Direction();
    private bool exploded = false;
    public GameObject boomEffect;
    public Player player;
    void Start()
    {
        player = FindObjectOfType<Player>();
        conductor = FindObjectOfType<Conductor>();
        SpawnPos = GameObject.FindGameObjectWithTag("Controller").transform.position;
        //RemovePos = GameObject.FindGameObjectWithTag("Player").transform.position;
        BeatsShownInAdvance = conductor.beatsShownInAdvance;
        beatOfThisNote = conductor.notes[conductor.nextIndex - 1];
        direction = conductor.notesDirection[conductor.nextIndex - 1];
        switch (direction)
        {
            case Direction.Up:
                transform.rotation = Quaternion.Euler(Vector3.zero);
                break;
            case Direction.Down:
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                break;
            case Direction.Left:
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
            case Direction.Right:
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                break;
        }
    }
    void Update()
    {
        songPosInBeats = conductor.songPosInBeats;
        if(!exploded)
        transform.position = Vector2.Lerp(
            SpawnPos,
            TargetPos,
            ((BeatsShownInAdvance - (beatOfThisNote - songPosInBeats)) / BeatsShownInAdvance)
        );
        if (transform.position.x == HitPos.x && !exploded)
        {
            FindObjectOfType<Player>().Miss();
            TargetPos = RemovePos;
        }
        if (transform.position.x == RemovePos.x&&!exploded)
        {
            GoBoom();
        }
    }
    public void Hit(Direction dir)
    {
        if (dir == direction)
        {
            Debug.Log("hit");
            exploded = true;
            player.GoodHit();
            GoBoom();
        }
        else
        {
            Debug.Log("god this guy's bad");
            exploded = true;
            player.Miss();
            Miss();
        }
    }
    public void ShitHit(Direction dir)
    {
        if (dir == direction)
        {
            Debug.Log("bad hit");
            exploded = true;
            player.ShitHit();
            GoBoom();
        }
        else {
            player.Miss();
            Miss();
        }
    }
    public void Miss()
    {
        Debug.Log("missed it dammit");
        GoBoom();
    }
    private void GoBoom()
    {
        exploded = true;
        GameObject boom = Instantiate(boomEffect);
        boom.transform.position = transform.position;
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        
    }

}
