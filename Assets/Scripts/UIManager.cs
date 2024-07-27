using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] TMP_Text speakerName;
    [SerializeField] TMP_Text eventText;
    [SerializeField] ChoicePrefab[] displayChoices;
    public Slider foodSlider;
    public Slider waterSlider;
    public Slider remedySlider;
    public Slider satisfactionSlider;

    private void Awake()
    {
        Instance = this;
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

    public void UpdateSliders()
    {
        Debug.Log(GameManager.Instance.currentFood/ GameManager.Instance.maxFood);
        foodSlider.value = GameManager.Instance.currentFood / GameManager.Instance.maxFood;
        waterSlider.value = GameManager.Instance.currentWater / GameManager.Instance.maxWater;
        remedySlider.value = GameManager.Instance.currentRemedy / GameManager.Instance.maxRemedy;
        satisfactionSlider.value = GameManager.Instance.currentSatisfaction / GameManager.Instance.maxSatisfaction;
    }
}
