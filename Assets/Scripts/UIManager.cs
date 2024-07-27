using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] TMP_Text speakerName;
    [SerializeField] TMP_Text eventText;
    [SerializeField] TMP_Text dayText;
    [SerializeField] TMP_Text dayMomentText;
    [SerializeField] ChoicePrefab[] displayChoices;
    public Slider foodSlider;
    public Slider waterSlider;
    public Slider remedySlider;
    public Slider satisfactionSlider;

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

    public void UpdateDayInfo()
    {
        dayText.text = "Dia " + GameManager.Instance.currentDay.ToString();
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
    }

    public void UpdateSliders()
    {
        float[] newValues = {
            GameManager.Instance.currentFood / GameManager.Instance.maxFood,
            GameManager.Instance.currentWater / GameManager.Instance.maxWater,
            GameManager.Instance.currentRemedy / GameManager.Instance.maxRemedy,
            GameManager.Instance.currentSatisfaction / GameManager.Instance.maxSatisfaction
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
}
