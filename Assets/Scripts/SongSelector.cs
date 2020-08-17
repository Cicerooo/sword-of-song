using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class SongSelector : MonoBehaviour
{
    public TextMeshProUGUI songTitle;
    public Beatmap beatmap;
    public string folderName;
    public AudioClip premadeSong;
    public bool isPremade;

    public void StartMap()
    {
        StartCoroutine(PlayerCoroutine());
    }
    public IEnumerator PlayerCoroutine()
    {
        FindObjectOfType<MenuSelect>().StartLoadingScreen();
        yield return new WaitForSeconds(2);
        GameObject obj = Instantiate(new GameObject());
        obj.AddComponent<SongData>();
        SongData songData = obj.GetComponent<SongData>();
        songData.beatmap = beatmap;
        if (isPremade)
            songData.audioClip = premadeSong;
        else
            songData.audioClip = WavUtility.ToAudioClip(folderName + "/song.wav");
        DontDestroyOnLoad(obj);
        SceneManager.LoadScene("L1");
        yield return 0;
    }
    public void StartEditor()
    {
        StartCoroutine(EditorCoroutine());
    }
    public IEnumerator EditorCoroutine()
    {
        FindObjectOfType<MenuSelect>().StartLoadingScreen();
        yield return new WaitForSeconds(2);
        GameObject obj = Instantiate(new GameObject());
        obj.AddComponent<SongData>();
        SongData songData = obj.GetComponent<SongData>();
        songData.beatmap = beatmap;
        songData.audioClip = WavUtility.ToAudioClip(folderName + "/song.wav");
        DontDestroyOnLoad(obj);
        SceneManager.LoadScene("Editor");
        yield return 0;
    }
    public void NewEditor()
    {
        SceneManager.LoadScene("Editor");
    }
}
