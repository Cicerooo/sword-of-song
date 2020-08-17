using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class CustomSongManager
{
    public static string path = Application.persistentDataPath + "/songs/";

    public class songResult
    {
        public Beatmap beatmap = ScriptableObject.CreateInstance<Beatmap>();
        public AudioClip audio;
    }

    public static void SaveSong(Beatmap map, AudioClip song)
    {
        if (!Directory.Exists(path)) 
            Directory.CreateDirectory(path);
        if (Directory.Exists(path))
        {
            string songPath = path + "/" + map.songArtist + " - " + map.songName;
            if (File.Exists(songPath + "/song.wav"))
            File.Delete(songPath+"/song.wav");
            SavWav.Save(songPath + "/song", song,true);
            string mapJson = JsonUtility.ToJson(map);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream mapFile = File.Create(songPath+"/beat.map");
            bf.Serialize(mapFile, mapJson);
            mapFile.Close();
        }
    }
    public static songResult LoadSong(string songname)
    {
        songResult result = new songResult();
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        if (Directory.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            if (File.Exists(songname + "/beat.map"))
            {
                FileStream songFile = File.Open(songname + "/beat.map",FileMode.Open);
                JsonUtility.FromJsonOverwrite((string)bf.Deserialize(songFile), result.beatmap);
                Debug.Log(result.beatmap);
                if(File.Exists(songname+"/song.wav"))
                result.audio = WavUtility.ToAudioClip(songname + "/song.wav");
                Debug.Log(result.audio);
                songFile.Close();
            }
        }
        return(result);
    }
    public static Beatmap GetBeatmapFromFolder(string folder)
    {
        BinaryFormatter bf = new BinaryFormatter();
        Beatmap beatmap = ScriptableObject.CreateInstance<Beatmap>();
        if (File.Exists(folder + "/beat.map"))
        {
            FileStream songFile = File.Open(folder + "/beat.map", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(songFile), beatmap);
        }
        return beatmap;
    }
    public static Beatmap[] GetAllBeatmaps()
    {
        if (ListSongs() == null)
            return null;
        string[] folders = ListSongs();
        int len = folders.Length;
        Debug.Log(len);
        Beatmap[] beatmap = new Beatmap[len];
        Debug.Log(beatmap.Length);
        BinaryFormatter bf = new BinaryFormatter();
        

        for(int i=0; i < folders.Length; i++)
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
    public static string[] ListSongs()
    {
        if (!Directory.Exists(path))
            return null;
        Debug.Log(Directory.GetDirectories(path)[0]);
        return(Directory.GetDirectories(path));
    }
}
