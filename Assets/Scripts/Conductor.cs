using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using System;
using DiscordPresence;

public class Conductor : MonoBehaviour
{
    //the current position of the song (in seconds)
    public float songPosition;

    //the current position of the song (in beats)
    public float songPosInBeats;

    //the duration of a beat
    public float secPerBeat;

    //how much time (in seconds) has passed since the song started
    public float dsptimesong;

    //beats per minute of a song
    public float bpm;

    //keep all the position-in-beats of notes in the song
    public float[] notes;

    public Notes.Direction[] notesDirection;
    //the index of the next note to be spawned
    public int nextIndex = 0;

    public int beatsShownInAdvance = 1;

    public GameObject beatPrefab;
    public GameObject hitArea;
    public GameObject hitTarget;
    public GameObject noteDestroyEffect;
    public Beatmap currentBeatmap;

    private bool songFinished = false;

    void Start()
    {
        SongData songData = FindObjectOfType<SongData>();
        if (songData)
        {
            currentBeatmap = songData.beatmap;
            beatsShownInAdvance = songData.beatmap.beatsShownInAdvance;
            bpm = songData.beatmap.bpm;
            notes = songData.beatmap.notes;
            notesDirection = songData.beatmap.notesDirection;
            GetComponent<AudioSource>().clip = songData.audioClip;
            Destroy(songData.gameObject);
        }
        //AddRichPresence();
        //calculate how many seconds is one beat
        //we will see the declaration of bpm later
        secPerBeat = 60f / bpm;

        //record the time when the song starts
        dsptimesong = (float)AudioSettings.dspTime;

        //start the song
        GetComponent<AudioSource>().Play();
    }
    private void FixedUpdate()
    {
        if (nextIndex == notes.Length )
        {
            
            Debug.Log("songFinished");
            if (!songFinished)
            {
                StartCoroutine(FinishedSong());
                songFinished = true;
            }
        }
    }
    public IEnumerator FinishedSong()
    {
        yield return new WaitForSeconds(1+beatsShownInAdvance);
        Scoreboard scoreboard = FindObjectOfType<Scoreboard>();
        scoreboard.songArtist = currentBeatmap.songArtist;
        scoreboard.songName = currentBeatmap.songName;
        DontDestroyOnLoad(scoreboard.gameObject);
        SceneManager.LoadScene("Summary");
        yield return 0;
    }
    void Update()
    {
        //calculate the position in seconds
        songPosition = (float)(AudioSettings.dspTime - dsptimesong);

        //calculate the position in beats
        songPosInBeats = songPosition / secPerBeat;
        if (nextIndex < notes.Length && notes[nextIndex] < songPosInBeats + beatsShownInAdvance)
        {
            GameObject beat = Instantiate(beatPrefab);
            Notes note = beat.GetComponent<Notes>();
            note.SpawnPos = transform.position;
            note.RemovePos = hitArea.transform.position;
            note.HitPos = hitTarget.transform.position;
            note.TargetPos = hitTarget.transform.position;
            note.beatOfThisNote = notes[nextIndex];
            note.direction = notesDirection[nextIndex];
            note.boomEffect = noteDestroyEffect;
            nextIndex++;
            //Debug.Break();
        }
    }
}
