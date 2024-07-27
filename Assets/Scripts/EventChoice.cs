using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Choice", menuName = "Events/New Choice")]
public class EventChoice : ScriptableObject
{
    public string text;
    public ChoiceInfluence influence;

    public void MakeChoice()
    {
        GameManager.Instance.ChoiceWasMade(this);
    }
}
