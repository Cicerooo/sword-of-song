using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Summary : MonoBehaviour
{
    public Scoreboard scoreboard;
    public TextMeshProUGUI score;
    public TextMeshProUGUI combo;
    public TextMeshProUGUI miss;
    public TextMeshProUGUI great;
    public TextMeshProUGUI perfect;
    public TextMeshProUGUI songName;
    public TextMeshProUGUI songArtist;
    // Start is called before the first frame update
    void Start()
    {
        scoreboard = FindObjectOfType<Scoreboard>();
        if (scoreboard)
        {
            score.text = "Score: " + scoreboard.score.ToString();
            combo.text = "Combo: " + scoreboard.combo.ToString();
            miss.text = "Miss: " + scoreboard.countMiss.ToString();
            great.text = "Great: " + scoreboard.count100.ToString();
            perfect.text = "Perfect: " + scoreboard.count300.ToString();
            songName.text = scoreboard.songName;
            songArtist.text = scoreboard.songArtist;
            Destroy(scoreboard.gameObject);
        }
    }
}
