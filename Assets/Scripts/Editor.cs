using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.UI.Extensions;
using noteInfo;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class Editor : MonoBehaviour
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
    private List<float> noteChart = new List<float>();
    private List<Notes.Direction> directionChart = new List<Notes.Direction>();
    public Beatmap map;
    [Header("Tempo")]
    public Image metronome;
    public TextMeshProUGUI currentBeat;
    [Header("Note editing")]
    public GameObject noteContent;
    public GameObject notePrefab;
    [Header("Song info")]
    public TMP_InputField songName;
    public TMP_InputField songArtist;
    public TMP_InputField songBPM;
    public TMP_InputField songBeatsInAdvance;
    public TextMeshProUGUI fileNameText;
    public Slider volumeSlider;
    private AudioSource song;
    private bool doAudioSkim = true;
    [HideInInspector]
    public float addTimeSkimDelay = 0;
    private bool isFieldOpen = false;
    [HideInInspector]
    public NoteInfoData[] noteInfo;
    private List<NoteInfoData> noteInfoList = new List<NoteInfoData>();
    private NoteInfoScroll scroll;
    //private CustomSongManager songManager;
    private bool hasStarted = false;
    void Start()
    {

        #if PLATFORM_ANDROID
            AndroidRuntimePermissions.RequestPermissions("android.permission.WRITE_EXTERNAL_STORAGE", "android.permission.READ_EXTERNAL_STORAGE");
        #endif
        Application.targetFrameRate = 60;
        //songManager = new CustomSongManager();
        song = GetComponent<AudioSource>();
        scroll = FindObjectOfType<NoteInfoScroll>();
        SongData songData = FindObjectOfType<SongData>();
        if (songData)
        {
            map = songData.beatmap;
            LoadMap();
            GetComponent<AudioSource>().clip = songData.audioClip;
            Destroy(songData.gameObject);
        }
        else map = ScriptableObject.CreateInstance<Beatmap>();
        Load();
        //calculate how many seconds is one beat
        //we will see the declaration of bpm later
        secPerBeat = 60f / bpm;

        //record the time when the song starts
        dsptimesong = (float)AudioSettings.dspTime;

        //start the song
        song.Play();
        hasStarted = true;
    }
    private void AddRichPresence()
    {
        var activity = new Discord.Activity
        {
            Assets = {
                LargeImage = "musicnoteicon"
            },
            State = "Editing a map",
            Timestamps ={
                Start = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            }
        };
        FindObjectOfType<DiscordController>().UpdateActivity(activity);
    }
    void LoadMap()
    {
        beatsShownInAdvance = map.beatsShownInAdvance;
        bpm = map.bpm;
        notes = map.notes;
        notesDirection = map.notesDirection;
        noteChart.AddRange(notes);
        directionChart.AddRange(notesDirection);
        songArtist.text = map.songArtist;
        songName.text = map.songName;
        songBPM.text = map.bpm.ToString();
        songBeatsInAdvance.text = map.beatsShownInAdvance.ToString();
        //AddRichPresence();
    }
    void Update()
    {
        if (!song.isPlaying)
            addTimeSkimDelay -= Time.deltaTime;

        //calculate the position in seconds
        songPosition = (float)(AudioSettings.dspTime - dsptimesong + addTimeSkimDelay);

        //calculate the position in beats
        songPosInBeats = songPosition / secPerBeat;
        HandleInput();
        UpdateUI();
        //if the beat is going down
        if (nextIndex < notes.Length && notes[nextIndex] < songPosInBeats + beatsShownInAdvance)
        {
            //TODO do dope ass beat UI
        }
    }
    public void AutoGenerate()
    {
        noteChart.Clear();
        directionChart.Clear();
        float beatSeconds = 0;
        float numberOfBeats = song.clip.length / secPerBeat;
        for(int i = 0; i < numberOfBeats; i++)
        {
            noteChart.Add(i);
            directionChart.Add(Notes.Direction.Up);
            beatSeconds = 0;
        }
        /*
        for(int i=0; i < song.clip.length;i++)
        {
            beatSeconds++;
            if (beatSeconds>=secPerBeat)
            {
                noteChart.Add(i / secPerBeat);
                directionChart.Add(Notes.Direction.Up);
                beatSeconds = 0;
            }
        }*/
        notes = noteChart.ToArray();
        notesDirection = directionChart.ToArray();
        Load();
    }
    public void OrderNotes()
    {
        float[] loadedNotes = noteChart.ToArray();
        Notes.Direction[] loadedDirection = directionChart.ToArray();
        for (int i = 0; i < loadedNotes.Length; i++)
        {

            for (int j = i; j < loadedNotes.Length; j++)
            {
                if (loadedNotes[i] > loadedNotes[j])
                {
                    float t = loadedNotes[i];
                    Notes.Direction tdir = loadedDirection[i];
                    loadedNotes[i] = loadedNotes[j];
                    loadedNotes[j] = t;
                    loadedDirection[i] = loadedDirection[j];
                    loadedDirection[j] = tdir;
                }
            }
        }
        noteChart.Clear();
        directionChart.Clear();
        notes = loadedNotes;
        notesDirection = loadedDirection;
        noteChart.AddRange(loadedNotes);
        Load();
        directionChart.AddRange(loadedDirection);
    }
    public void RemoveNote(int index)
    {
        noteChart.RemoveAt(index);
        directionChart.RemoveAt(index);
        notes = noteChart.ToArray();
        notesDirection = directionChart.ToArray();
        OrderNotes();
    }
    public void AddNote(Notes.Direction dir)
    {
        noteChart.Add(songPosInBeats);
        directionChart.Add(dir);
        Debug.Log(dir);
        OrderNotes();
    }
    public void NewNote()
    {
        AddNote(Notes.Direction.Up);

    }
    public void Load()
    {
        float[] loadedNotes = noteChart.ToArray();
        Notes.Direction[] loadedDirection = directionChart.ToArray();
        noteInfoList.Clear();
        for (int i=0;i<loadedNotes.Length;i++){
            NoteInfoData data = new NoteInfoData();
            data.index = i;
            data.notePoint = notes[i];
            data.direction = notesDirection[i];
            noteInfoList.Add(data);
        }
        noteInfo = noteInfoList.ToArray();
        if (hasStarted)
        {
            scroll.mVerticalDynamicScroll.ChangeList(noteInfoList, 0, false);

        }
    }
    public void UpdateNote(int index, float notePoint, Notes.Direction dir)
    {
        float[] loadedNotes = noteChart.ToArray();
        Notes.Direction[] loadedDirection = directionChart.ToArray();
        loadedNotes[index] = notePoint;
        loadedDirection[index] = dir;
        directionChart.Clear();
        directionChart.AddRange(loadedDirection);
        noteChart.Clear();
        noteChart.AddRange(loadedNotes);
        OrderNotes();
    }
    public void UpdateBPM() {
        bpm = float.Parse(songBPM.text);
        secPerBeat = 60f / bpm;
    }
    public void newSong()
    {
        /*
        map = new Beatmap();
        dsptimesong = (float)AudioSettings.dspTime;
        addTimeSkimDelay = 0;
        */
        CustomSongManager.ListSongs();
    }
    public void LoadSong()
    {
        CustomSongManager.songResult res = CustomSongManager.LoadSong(CustomSongManager.ListSongs()[0]);
        map = res.beatmap;
        song.clip = res.audio;
        noteChart.Clear();
        directionChart.Clear();
        LoadMap();
        song.Play();
        secPerBeat = 60f / bpm;
        dsptimesong = (float)AudioSettings.dspTime;
        addTimeSkimDelay = 0;
        Load();
        UpdateUI();
    }
    public void Save()
    {
        OrderNotes();
        map.songName = songName.text;
        map.songArtist = songArtist.text;
        map.bpm = bpm;
        map.beatsShownInAdvance = int.Parse(songBeatsInAdvance.text);
        map.notes = noteChart.ToArray();
        map.notesDirection = directionChart.ToArray();
        CustomSongManager.SaveSong(map, song.clip);
    }
    private void UpdateUI()
    {
        if (songPosInBeats > Mathf.Round(songPosInBeats) - 0.03f && songPosInBeats < Mathf.Round(songPosInBeats) + 0.05f)
        {
            metronome.color = Color.green;
        }
        else
        {
            metronome.color = Color.white;
        }
        if(song.isPlaying)
        currentBeat.text = "<mspace=16px>" + (Mathf.Round(songPosInBeats * 10f) / 10f).ToString() + "</mspace>";
        if(map.song!=null)
        fileNameText.text = map.song.name;
    }

    public void ForwardSong()
    {
        if (doAudioSkim)
        {
            song.time += 5;
            addTimeSkimDelay += 5;
            UpdateUI();
        }
    }
    public void RewindSong()
    {
        if (doAudioSkim)
        {
            if (song.time >= 5)
            {
                song.time -= 5;
                addTimeSkimDelay -= 5;
            }
            else
            {
                addTimeSkimDelay -= song.time;
                song.time -= song.time;
            }
            UpdateUI();
        }
    }
    public void ToggleMusic()
    {
        if (doAudioSkim)
        {
            if (song.isPlaying)
            {
                song.Pause();
                return;
            }
            if (!song.isPlaying)
            {
                song.UnPause();
                return;
            }
        }
    }
    public void UpdateVolume()
    {
        SetVolume(volumeSlider.value);
    }
    public void SetVolume(float value)
    {
        song.volume = value;
    }
    public void OpenField()
    {
        doAudioSkim = false;
    }
    public void CloseField()
    {
        doAudioSkim = true;
    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AddNote(Notes.Direction.Up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            AddNote(Notes.Direction.Down);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AddNote(Notes.Direction.Left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AddNote(Notes.Direction.Right);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleMusic();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(doAudioSkim)
            Save();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            RewindSong();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ForwardSong();
        }
    }
}

