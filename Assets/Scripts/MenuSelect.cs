using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelect : MonoBehaviour
{
    public Transform listContainer;
    public GameObject listItemPrefab;
    public bool listPremadeSongs;
    public Beatmap[] premadeMaps;
    private Beatmap[] songList;
    private string[] folderList;
    private SongManagerComponent songManager;
    public GameObject loadingScreen;
    private Animator loadingAnimator;
    void Start()
    {
        loadingAnimator = loadingScreen.GetComponent<Animator>();

        if (FindObjectOfType<SongData>())
            Destroy(FindObjectOfType<SongData>().gameObject);

        songManager = gameObject.AddComponent<SongManagerComponent>();

        songList = songManager.GetAllBeatmaps();
        folderList = songManager.ListSongs();

        if(listPremadeSongs)
        for (int j = 0; j < premadeMaps.Length; j++)
        {
            Debug.Log(premadeMaps[j].songName + " - " + j.ToString());
            GameObject game = Instantiate(listItemPrefab, listContainer);
            game.GetComponent<SongSelector>().songTitle.text = premadeMaps[j].songName;
            game.GetComponent<SongSelector>().beatmap = premadeMaps[j];
            game.GetComponent<SongSelector>().isPremade = true;
            game.GetComponent<SongSelector>().premadeSong = premadeMaps[j].song;
        }
        if(songList.Length>0)
        for (int i=0; i < songList.Length; i++)
        {
            Debug.Log(songList[i].songName+" - "+i.ToString());
            GameObject game = Instantiate(listItemPrefab, listContainer);
            game.GetComponent<SongSelector>().songTitle.text = songList[i].songName;
            game.GetComponent<SongSelector>().folderName = folderList[i];
            game.GetComponent<SongSelector>().beatmap = songList[i];
        }
    }
    public void StartLoadingScreen()
    {
        loadingScreen.SetActive(true);
        loadingAnimator.SetTrigger("fadeIn");
    }
}
