using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using NAudio;
using NAudio.Wave;
using UnityEngine.Networking;
using SimpleFileBrowser;
using TMPro;
public class AudioFileSelect : MonoBehaviour
{
    private Editor editor;
    private AudioSource audioSource;
    public TextMeshProUGUI pathText;
    public GameObject loadingScreen;
    private Animator loadinAnimator;
    private void Start()
    {
        editor = GetComponent<Editor>();
        audioSource = GetComponent<AudioSource>();
        loadinAnimator = loadingScreen.GetComponent<Animator>();
    }
    public void ReadMp3Sounds()
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Sounds", ".mp3"));
        FileBrowser.SetDefaultFilter(".mp3");
        StartCoroutine(ShowLoadDialogCoroutine());
    }
    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(false, null, "Select Sound", "Select");
        if (FileBrowser.Success)
        {
            loadingScreen.SetActive(true);
            loadinAnimator.SetTrigger("fadeIn");
            yield return new WaitForSeconds(2f);
            byte[] SoundFile = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result);
            yield return SoundFile;
            audioSource.clip = NAudioPlayer.FromMp3Data(SoundFile);
            editor.dsptimesong = (float)AudioSettings.dspTime;
            editor.addTimeSkimDelay = 0;
            loadinAnimator.SetTrigger("fadeOut");
            yield return new WaitForSeconds(2f);
            loadingScreen.SetActive(false);
            pathText.text = Path.GetFileNameWithoutExtension(FileBrowser.Result);
            audioSource.Play();
        }
    }
}
