using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class StartTimer : MonoBehaviour
{
    public TextMeshProUGUI MainText;
    public GameObject background;
    private Conductor conductor;
    void Start()
    {
        conductor = FindObjectOfType<Conductor>();
        StartCoroutine(startTimer());   
    }
    public IEnumerator startTimer()
    {
        conductor.gameObject.GetComponent<AudioSource>().enabled = false;
        conductor.enabled = false;
        background.SetActive(true);
        MainText.gameObject.SetActive(true);
        MainText.text = "READY?";
        yield return new WaitForSeconds(0.5f);
        MainText.text = "SET";
        yield return new WaitForSeconds(0.5f);
        MainText.text = "SLASH";
        yield return new WaitForSeconds(0.5f);
        background.SetActive(false);
        MainText.gameObject.SetActive(false);
        conductor.gameObject.GetComponent<AudioSource>().enabled = true;
        conductor.enabled = true;
        yield return 0;
    }

}
