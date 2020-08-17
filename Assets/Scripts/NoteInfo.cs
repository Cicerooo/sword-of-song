using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NoteInfo : MonoBehaviour
{
    public int index;
    public float notePoint;
    public Notes.Direction direction = new Notes.Direction();
    private bool selected = false;
    public TextMeshProUGUI title;
    public TMP_InputField note;
    public TMP_Dropdown directionSelect;

    // Update is called once per frame
    void Start()
    {
        title.text = "song note " + index.ToString();
        note.text = (Mathf.Round(notePoint * 100) / 100).ToString();
        switch (direction)
        {
            case Notes.Direction.Up:
                directionSelect.value = 0;
                break;
            case Notes.Direction.Down:
                directionSelect.value = 1;
                break;
            case Notes.Direction.Left:
                directionSelect.value = 2;
                break;
            case Notes.Direction.Right:
                directionSelect.value = 3;
                break;
        }
    }
    public void Selected()
    {
        selected = true;
    }
    public void Deselected()
    {
        selected = false;
    }
    public void UpdateNote()
    {
        if (selected)
        {
            switch (directionSelect.value)
            {
                case 0:
                    direction = Notes.Direction.Up;
                    break;
                case 1:
                    direction = Notes.Direction.Down;
                    break;
                case 2:
                    direction = Notes.Direction.Left;
                    break;
                case 3:
                    direction = Notes.Direction.Right;
                    break;

            }
            FindObjectOfType<Editor>().UpdateNote(index,float.Parse(note.text), direction);
        }
    }
    public void NoteUnfocus()
    {
        FindObjectOfType<Editor>().OrderNotes();
    }
    public void RemoveNote()
    {
        FindObjectOfType<Editor>().RemoveNote(index);
    }
}
