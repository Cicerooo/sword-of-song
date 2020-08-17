using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class AndroidPermissionSetup : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isUpdating = false;
    // Update is called once per frame
    void Update()
    {
        if (!isUpdating)
        {
            StartCoroutine(RequestPermission());
            isUpdating = !isUpdating;
        }

    }
    private IEnumerator RequestPermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
        yield return 0;
    }
}
