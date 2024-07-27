using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoicePrefab : MonoBehaviour
{
    public EventChoice choice;
    [SerializeField] TMP_Text text;

    private void OnEnable()
    {
        text.text = choice.text;
    }

    private void OnDisable()
    {
        text.text = "";
    }

    public void OnClick()
    {
        choice.MakeChoice();
    }
}
