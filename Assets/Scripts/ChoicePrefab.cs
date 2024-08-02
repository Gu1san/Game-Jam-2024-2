using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoicePrefab : MonoBehaviour
{
    public EventChoice choice;
    [SerializeField] TMP_Text text;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip mouseEnterClip;
    [SerializeField] AudioClip mouseClickClip;

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
        audioSource.PlayOneShot(mouseClickClip);
        choice.MakeChoice();
    }

    public void MouseEnter()
    {
        audioSource.PlayOneShot(mouseEnterClip);
        UIManager.Instance.HighlightSliders(choice.influence);
    }

    public void MouseExit()
    {
        UIManager.Instance.UnhighlightSliders();
    }
}
