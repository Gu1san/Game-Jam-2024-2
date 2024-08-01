using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("Texts")]
    [SerializeField] TMP_Text speakerName;
    [SerializeField] TMP_Text eventText;
    [SerializeField] TMP_Text dayText;
    [SerializeField] TMP_Text dayMomentText;
    [SerializeField] TMP_Text reportText;
    [SerializeField] TMP_Text endGameTitle;
    [SerializeField] TMP_Text endGameText;

    [Header("Panels")]
    [SerializeField] GameObject reportPanel;
    [SerializeField] GameObject endGamePanel;
    [SerializeField] GameObject pausePanel;

    [Header("Sliders")]
    [SerializeField] Image foodSlider;
    [SerializeField] Image waterSlider;
    [SerializeField] Image moneySlider;
    [SerializeField] Image satisfactionSlider;

    [Header("Images/Materials")]
    [SerializeField] Image dayMomentImage;
    [SerializeField] Sprite[] dayMomentSprites;
    [SerializeField] Material[] skyboxes;

    [SerializeField] ChoicePrefab[] displayChoices;
    [SerializeField] Color highlightColor;

    [Header("Sound")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip openReport;
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) &&
           !reportPanel.activeInHierarchy && 
           !endGamePanel.activeInHierarchy)
        {
            pausePanel.SetActive(true);
        }
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
        audioSource.PlayOneShot(openReport);
        fader.FadeOut();
        float foodDiference = GameManager.Instance.CurrentFood - GameManager.Instance.LastStatus.food;
        float waterDiference = GameManager.Instance.CurrentWater - GameManager.Instance.LastStatus.water;
        float remedyDiference = GameManager.Instance.CurrentMoney - GameManager.Instance.LastStatus.money;
        float satisfactionDiference = GameManager.Instance.CurrentSatisfaction - GameManager.Instance.LastStatus.satisfaction;
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
        dayText.text = GameManager.Instance.CurrentDay.ToString();
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
        int momentIndex = (int)GameManager.Instance.DayMoment;
        RenderSettings.skybox = skyboxes[momentIndex];
        dayMomentImage.sprite = dayMomentSprites[momentIndex];
        fader.FadeInScene();
    }

    public void UpdateSliders()
    {
        float[] newValues = {
            GameManager.Instance.CurrentFood / GameManager.Instance.MaxStatusValue,
            GameManager.Instance.CurrentWater / GameManager.Instance.MaxStatusValue,
            GameManager.Instance.CurrentMoney / GameManager.Instance.MaxStatusValue,
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
        else if(GameManager.Instance.CurrentMoney <= 0)
        {
            endGameText.text = "Depois de sua má gestão com os medicamentos, a ONU decidiu que você não está apto a continuar gerenciando o campo de refugiados";
        }else if (GameManager.Instance.CurrentSatisfaction <= 0)
        {
            endGameText.text = "Com a sua falta de tratamento humanitário, a ONU decidiu que você não está apto a continuar gerenciando o campo de refugiados";
        }
        endGamePanel.SetActive(true);
    }

    public void HighlightSliders(ChoiceInfluence influences)
    {
        if(influences.food != 0)
        {
            foodSlider.color = highlightColor;
        }
        if(influences.water != 0)
        {
            waterSlider.color = highlightColor;
        }
        if(influences.money != 0)
        {
            moneySlider.color = highlightColor;
        }
        if(influences.satisfaction != 0)
        {
            satisfactionSlider.color = highlightColor;
        }
    }

    public void UnhighlightSliders()
    {
        foodSlider.color = Color.white;
        waterSlider.color = Color.white;
        moneySlider.color = Color.white;
        satisfactionSlider.color = Color.white;
    }

    IEnumerator UpdateSlidersSmoothly(float[] newValues)
    {
        float time = 0;
        float startFood = foodSlider.fillAmount;
        float startWater = waterSlider.fillAmount;
        float startRemedy = moneySlider.fillAmount;
        float startSatisfaction = satisfactionSlider.fillAmount;
        while (time < .5f)
        {
            time += Time.deltaTime;
            float t = time / .5f;
            foodSlider.fillAmount = Mathf.Lerp(startFood, newValues[0], t);
            waterSlider.fillAmount = Mathf.Lerp(startWater, newValues[1], t);
            moneySlider.fillAmount = Mathf.Lerp(startRemedy, newValues[2], t);
            satisfactionSlider.fillAmount = Mathf.Lerp(startSatisfaction, newValues[3], t);
            yield return null;
        }
    }
}
