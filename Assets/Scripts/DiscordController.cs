using DiscordPresence;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscordController : MonoBehaviour
{
    /*
    Grab that Client ID from earlier
    Discord.CreateFlags.Default will require Discord to be running for the game to work
    If Discord is not running, it will:
    1. Close your game
    2. Open Discord
    3. Attempt to re-open your game
    Step 3 will fail when running directly from the Unity editor
    Therefore, always keep Discord running during tests, or use Discord.CreateFlags.NoRequireDiscord
*/
/*    public Discord.Discord discord = new Discord.Discord(701443708318515270, (UInt64)Discord.CreateFlags.NoRequireDiscord);
    public Discord.ActivityManager activityManager;*/

    public enum Section
    {
        MainMenu,
        PlayerSelector,
        EditorSelector,
        Player,
        Editor,
        Summary
    }
    public Section section;
    // Start is called before the first frame update
    void Start()
    {
        string state = "";
        string detail = "";
        switch (section)
        {
            case Section.MainMenu:
                state = "In the menu";
                break;
            case Section.PlayerSelector:
                state = "Selecting a map";
                break;
            case Section.EditorSelector:
                state = "Selecting a map to edit";
                break;
            case Section.Player:
                state = "Playing a map";
                Beatmap songData = FindObjectOfType<SongData>().beatmap;
                detail = songData.songArtist + " - " + songData.songName;
                break;
            case Section.Editor:
                state = "Editing a map";
                SongData editData = FindObjectOfType<SongData>();
                if(editData!=null)
                detail = editData.beatmap.songArtist + " - " + editData.beatmap.songName;
                break;
            case Section.Summary:
                state = "Finishing a map";
                break;
        }
        PresenceManager.UpdatePresence(detail: detail, state: state);
        /*var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            State = state,
            Assets =
            {
                LargeImage = "sos_logo_square"
            }
        };
        activityManager = discord.GetActivityManager();
        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Discord.Result.Ok)
            {
                Debug.Log("Discord Rich Presence is up!");
            }
        });*/
    }
    public void UpdateActivity(Discord.Activity activity)
    {

    }
    // Update is called once per frame
    void Update()
    {
        //discord.RunCallbacks();

    }
    private void OnApplicationQuit()
    {
        /*discord.GetActivityManager().ClearActivity((res)=> {
            if (res == Discord.Result.Ok)
            {
                Debug.Log("Everything is fine!");
            }
        });
        discord.Dispose();*/
    }
}
