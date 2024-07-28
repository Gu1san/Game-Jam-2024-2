using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] TMP_Text speakerName;
    [SerializeField] TMP_Text eventText;
    [SerializeField] TMP_Text dayText;
    [SerializeField] TMP_Text dayMomentText;
    [SerializeField] TMP_Text reportText;
    [SerializeField] TMP_Text endGameTitle;
    [SerializeField] TMP_Text endGameText;
    [SerializeField] ChoicePrefab[] displayChoices;
    [SerializeField] GameObject reportPanel;
    [SerializeField] GameObject endGamePanel;
    public Slider foodSlider;
    public Slider waterSlider;
    public Slider remedySlider;
    public Slider satisfactionSlider;
    SceneFader fader;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        fader = GetComponent<SceneFader>();
        UpdateDayInfo();
    }

    public void ShowEvent(Event eventSO)
    {
        speakerName.text = eventSO.speakerName;
        eventText.text = eventSO.text;
        for(int i = 0; i < displayChoices.Length; i++)
        {
            displayChoices[i].choice = eventSO.choices[i];
            displayChoices[i].gameObject.SetActive(true);
        }
    }

    public void HideEvent()
    {
        speakerName.text = "";
        eventText.text = "";
        foreach(ChoicePrefab choice in displayChoices)
        {
            choice.gameObject.SetActive(false);
        }
    }

    public void ShowReport()
    {
        fader.FadeOut();
        float foodDiference = GameManager.Instance.CurrentFood - GameManager.Instance.lastStatus.food;
        float waterDiference = GameManager.Instance.CurrentWater - GameManager.Instance.lastStatus.water;
        float remedyDiference = GameManager.Instance.CurrentRemedy - GameManager.Instance.lastStatus.remedy;
        float satisfactionDiference = GameManager.Instance.CurrentSatisfaction - GameManager.Instance.lastStatus.satisfaction;
        reportText.text = $"{foodDiference}\n{waterDiference}\n{remedyDiference}\n{satisfactionDiference}";
        reportPanel.SetActive(true);
    }

    public void CloseReport()
    {
        reportPanel.SetActive(false);
        reportText.text = "";
        UpdateDayInfo();
        GameManager.Instance.GetNewEvent();
    }

    public void UpdateDayInfo()
    {
        if(GameManager.Instance.CurrentDay != 1 || GameManager.Instance.DayMoment != 0)
        {
            fader.FadeOut();
        }
        dayText.text = "Dia " + GameManager.Instance.CurrentDay.ToString();
        switch ((int)GameManager.Instance.DayMoment)
        {
            case 0:
                dayMomentText.text = "Manhã";
                break;
            case 1:
                dayMomentText.text = "Tarde";
                break;
            case 2:
                dayMomentText.text = "Noite";
                break;
        }
        fader.FadeInScene();
    }

    public void UpdateSliders()
    {
        float[] newValues = {
            GameManager.Instance.CurrentFood / GameManager.Instance.MaxStatusValue,
            GameManager.Instance.CurrentWater / GameManager.Instance.MaxStatusValue,
            GameManager.Instance.CurrentRemedy / GameManager.Instance.MaxStatusValue,
            GameManager.Instance.CurrentSatisfaction / GameManager.Instance.MaxStatusValue
        };
        StartCoroutine(UpdateSlidersSmoothly(newValues));
    }

    public void WinGame()
    {
        fader.FadeOut();
        endGameTitle.text = "Missão concluída!";
        if(GameManager.Instance.MaximizedStatus is 0 or 1)
        {
            endGameText.text = "Você cumpriu seus deveres dentro do que era esperado. Parabéns!";
        }
        else if(GameManager.Instance.MaximizedStatus is 2 or 3)
        {
            endGameText.text = "Você se saiu muito bem superando as expectativas. Parabéns!";
        }
        else if(GameManager.Instance.MaximizedStatus >= 4)
        {
            endGameText.text = "Você realizou um trabalho muito acima de tudo que era experado e garantiu a qualidade de vida de milhares de pessoas. Parabéns!";
        }
        endGamePanel.SetActive(true);
    }

    public void GameOver()
    {
        fader.FadeOut();
        endGameTitle.text = "Missão fracassada";
        if(GameManager.Instance.CurrentFood <= 0)
        {
            endGameText.text = "Depois de sua má gestão com os alimentos, a ONU decidiu que você não está apto a continuar gerenciando o campo de refugiados";
        }
        else if (GameManager.Instance.CurrentWater <= 0)
        {
            endGameText.text = "Depois de sua má gestão com a água do local, a ONU decidiu que você não está apto a continuar gerenciando o campo de refugiados";
        }
        else if(GameManager.Instance.CurrentRemedy <= 0)
        {
            endGameText.text = "Depois de sua má gestão com os medicamentos, a ONU decidiu que você não está apto a continuar gerenciando o campo de refugiados";
        }else if (GameManager.Instance.CurrentSatisfaction <= 0)
        {
            endGameText.text = "Com a sua falta de tratamento humanitário, a ONU decidiu que você não está apto a continuar gerenciando o campo de refugiados";
        }
        endGamePanel.SetActive(true);
    }

    IEnumerator UpdateSlidersSmoothly(float[] newValues)
    {
        float time = 0;
        float startFood = foodSlider.value;
        float startWater = waterSlider.value;
        float startRemedy = remedySlider.value;
        float startSatisfaction = satisfactionSlider.value;
        while (time < .5f)
        {
            time += Time.deltaTime;
            float t = time / .5f;
            foodSlider.value = Mathf.Lerp(startFood, newValues[0], t);
            waterSlider.value = Mathf.Lerp(startWater, newValues[1], t);
            remedySlider.value = Mathf.Lerp(startRemedy, newValues[2], t);
            satisfactionSlider.value = Mathf.Lerp(startSatisfaction, newValues[3], t);
            yield return null;
        }
    }
}
