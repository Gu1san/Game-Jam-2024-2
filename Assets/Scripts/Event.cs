using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Events/Event")]
public class Event : ScriptableObject
{
    public string speakerName;
    public string text;
    public EventChoice[] choices = new EventChoice[2];
}

