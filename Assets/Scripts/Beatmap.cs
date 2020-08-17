using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Beatmap", menuName = "Beatmap")]
public class Beatmap : ScriptableObject
{
    public string songName;
    public string songArtist;
    // Approach rate
    public int beatsShownInAdvance = 1;
    public float bpm;
    public float[] notes;
    public Notes.Direction[] notesDirection;
    public AudioClip song;
    public string audioFileName;
}
