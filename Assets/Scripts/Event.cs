using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Events/Event")]
public class Event : ScriptableObject
{
    public string speakerName;
    [TextArea(1, 5)]
    public string text;
    public EventChoice[] choices = new EventChoice[2];
    public GameObject npcModel;
    public string npcPose = "Complimenting";
}

