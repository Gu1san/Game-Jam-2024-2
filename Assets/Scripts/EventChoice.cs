using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventChoice
{
    [TextArea(1, 2)]
    public string text;
    public ChoiceInfluence influence;

    public void MakeChoice()
    {
        GameManager.Instance.ChoiceWasMade(this);
    }
}
