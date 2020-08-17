using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public int score;
    public TextMeshProUGUI scoreText;
    public int combo;
    public TextMeshProUGUI comboText;
    public int count300=0;
    public int count100=0;
    public int countMiss=0;
    public string songName;
    public string songArtist;

    public void AddScore(int addScore)
    {
        combo++;
        score += addScore*combo;
        if (addScore == 300)
        {
            count300++;
        }
        if (addScore == 100)
        {
            count100++;
        }
    }
    public void Miss()
    {
        combo = 0;
        countMiss++;
    }
    private void Update()
    {
        if (score > 0)
        {
            scoreText.text = score.ToString();
        }
        comboText.text = "x"+combo.ToString();
    }
}
