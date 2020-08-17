using dynamicscroll;
using noteInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace noteInfo
{
    public class NoteInfoScrollObject : DynamicScrollObject<NoteInfoData>
    {
        public override float CurrentHeight { get; set; }
        public override float CurrentWidth { get; set; }
        public override string objectName => "NoteInfo";

        private TextMeshProUGUI title;
        private TMP_InputField beatPosition;
        private TMP_Dropdown directionSelect;
        private NoteInfo note;

        public void Awake() {
            /*
            CurrentHeight = GetComponent<RectTransform>().rect.height;
            CurrentWidth = GetComponent<RectTransform>().rect.width;*/
            CurrentHeight = 300;
            CurrentWidth = 450;
            note = GetComponent<NoteInfo>();
            title = transform.Find("NoteID").GetComponent<TextMeshProUGUI>();
            beatPosition = transform.Find("NotePosition").GetComponent<TMP_InputField>();
            directionSelect = transform.Find("Direction").GetComponent<TMP_Dropdown>();
        }

        public override void UpdateScrollObject(NoteInfoData item, int index)
        {
            base.UpdateScrollObject(item, index);

            title.gameObject.SetActive(true);
            beatPosition.gameObject.SetActive(true);
            directionSelect.gameObject.SetActive(true);

            title.text = "Note object "+index;
            beatPosition.text = item.notePoint.ToString();
            directionSelect.value = (int)item.direction;
            note.notePoint = item.notePoint;
            note.direction = item.direction;
        }
    }
}
