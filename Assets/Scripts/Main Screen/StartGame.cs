using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour
{
    public void OpenGame()
    {
        SceneManager.LoadScene("SongSelect");
    }
    public void OpenEditor()
    {
        SceneManager.LoadScene("EditorSelect");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
