using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SongManagerComponent : MonoBehaviour
{
    private string path;

    private void Start()
    {
        path = Application.persistentDataPath + "/songs/";
    }
    private void InitPath()
    {
        path = Application.persistentDataPath + "/songs/";
    }
    public class songResult
    {
        public Beatmap beatmap = ScriptableObject.CreateInstance<Beatmap>();
        public AudioClip audio;
    }

    public void SaveSong(Beatmap map, AudioClip song)
    {
        InitPath();
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        if (Directory.Exists(path))
        {
            string songPath = path + "/" + map.songArtist + " - " + map.songName;
            if (File.Exists(songPath + "/song.wav"))
                File.Delete(songPath + "/song.wav");
            SavWav.Save(songPath + "/song", song, true);
            string mapJson = JsonUtility.ToJson(map);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream mapFile = File.Create(songPath + "/beat.map");
            bf.Serialize(mapFile, mapJson);
            mapFile.Close();
        }
    }
    public songResult LoadSong(string songname)
    {
        InitPath();
        songResult result = new songResult();
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        if (Directory.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            if (File.Exists(songname + "/beat.map"))
            {
                FileStream songFile = File.Open(songname + "/beat.map", FileMode.Open);
                JsonUtility.FromJsonOverwrite((string)bf.Deserialize(songFile), result.beatmap);
                Debug.Log(result.beatmap);
                if (File.Exists(songname + "/song.wav"))
                    result.audio = WavUtility.ToAudioClip(songname + "/song.wav");
                Debug.Log(result.audio);
                songFile.Close();
            }
        }
        return (result);
    }
    public Beatmap GetBeatmapFromFolder(string folder)
    {
        InitPath();
        BinaryFormatter bf = new BinaryFormatter();
        Beatmap beatmap = ScriptableObject.CreateInstance<Beatmap>();
        if (File.Exists(folder + "/beat.map"))
        {
            FileStream songFile = File.Open(folder + "/beat.map", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(songFile), beatmap);
        }
        return beatmap;
    }
    public Beatmap[] GetAllBeatmaps()
    {
        InitPath();
        string[] folders = ListSongs();
        int len = folders.Length;
        Beatmap[] beatmap;
        if (len > 0)
            beatmap = new Beatmap[len];
        else
            return null;
        Debug.Log(beatmap.Length);
        BinaryFormatter bf = new BinaryFormatter();


        for (int i = 0; i < folders.Length; i++)
        {
            if (File.Exists(folders[i] + "/beat.map"))
            {
                Beatmap intBeat = ScriptableObject.CreateInstance<Beatmap>();
                FileStream songFile = File.Open(folders[i] + "/beat.map", FileMode.Open);
                JsonUtility.FromJsonOverwrite((string)bf.Deserialize(songFile), intBeat);
                Debug.Log(intBeat.songName);
                beatmap[i] = intBeat;
            }
        }
        return beatmap;
    }
    public string[] ListSongs()
    {
        InitPath();
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        //Debug.Log(Directory.GetDirectories(path)[0]);
        return (Directory.GetDirectories(path));
    }
}
