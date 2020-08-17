using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerKill : MonoBehaviour
{
    public IEnumerator destroyTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        delet();
        yield return 0;
    }
    public IEnumerator destroyTimerAnimTrigger(float timer, string trigger)
    {
        yield return new WaitForSeconds(timer);
        GetComponent<Animator>().SetTrigger(trigger);
        yield return 0;
    }
    public void delet()
    {
        Destroy(gameObject);
    }
}
