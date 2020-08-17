using UnityEngine;
using System.Collections;
using dynamicscroll;
using UnityEngine.UI;
using System.Collections.Generic;

namespace noteInfo
{
    public class NoteInfoScroll : MonoBehaviour
    {

        public DynamicScrollRect verticalScroll;
        public GameObject referenceObject;

        [HideInInspector]
        public DynamicScroll<NoteInfoData, NoteInfoScrollObject> mVerticalDynamicScroll = new DynamicScroll<NoteInfoData, NoteInfoScrollObject>();

        public IEnumerator Start()
        {
            var data = new List<NoteInfoData>();

            mVerticalDynamicScroll.spacing = 5f;
            mVerticalDynamicScroll.Initiate(verticalScroll, data, 0, referenceObject);

            FindObjectOfType<Editor>().Load();
            yield return 0;
        }
    }
}
