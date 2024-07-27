using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Events/New Event")]
public class Event : ScriptableObject
{
    public string speakerName;
    public string text;
    public EventChoice[] choices;
}

