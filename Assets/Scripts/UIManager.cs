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
    [SerializeField] ChoicePrefab[] displayChoices;
    [SerializeField] GameObject reportPanel;
    public Slider foodSlider;
    public Slider waterSlider;
    public Slider remedySlider;
    public Slider satisfactionSlider;
    public Image img;
    public AnimationCurve curve;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
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
        StartCoroutine(FadeOut());
        float foodDiference = GameManager.Instance.CurrentFood - GameManager.Instance.lastStatus.food;
        float waterDiference = GameManager.Instance.CurrentWater - GameManager.Instance.lastStatus.water;
        float remedyDiference = GameManager.Instance.CurrentRemedy - GameManager.Instance.lastStatus.remedy;
        float satisfactionDiference = GameManager.Instance.CurrentSatisfaction - GameManager.Instance.lastStatus.satisfaction;
        reportText.text = $"{foodDiference}\n{waterDiference}\n{remedyDiference}\n{satisfactionDiference}";
        reportPanel.SetActive(true);
        //StartCoroutine(FadeIn());
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
        StartCoroutine(FadeOut());
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
        StartCoroutine(FadeIn());
    }

    public void UpdateSliders()
    {
        float[] newValues = {
            GameManager.Instance.CurrentFood / GameManager.Instance.MaxFood,
            GameManager.Instance.CurrentWater / GameManager.Instance.MaxWater,
            GameManager.Instance.CurrentRemedy / GameManager.Instance.MaxRemedy,
            GameManager.Instance.CurrentSatisfaction / GameManager.Instance.MaxSatisfaction
        };
        StartCoroutine(UpdateSlidersSmoothly(newValues));
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

    IEnumerator FadeIn()
    {
        float t = 2f;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }

    IEnumerator FadeOut()
    {
        float t = 0f;

        while (t < 2f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }
}
