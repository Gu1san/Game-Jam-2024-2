using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum DayMoments
    {
        Morning,
        Evening,
        Night
    }
    public float maxFood { get; private set; } = 100;
    public float currentFood { get; private set; }
    public float maxWater { get; private set; } = 100;
    public float currentWater { get; private set; }
    public float maxSatisfaction { get; private set; } = 100;
    public float currentSatisfaction { get; private set; }
    public float maxRemedy { get; private set; } = 100;
    public float currentRemedy { get; private set; }
    public int currentDay { get; private set; } = 1;

    [SerializeField] Event currentEvent;

    readonly Object[][] events = new Object[3][];

    public DayMoments DayMoment { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DayMoment = DayMoments.Morning;
        GetEvents();
        currentFood = 50;
        currentWater = 50;
        currentRemedy = 50;
        currentSatisfaction = 50;
        UIManager.Instance.UpdateSliders();
        GetNewEvent();
    }

    private void GetEvents()
    {
        events[0] = Resources.LoadAll("Events/Morning");
        events[1] = Resources.LoadAll("Events/Evening");
        events[2] = Resources.LoadAll("Events/Night");
    }

    public void StartEvent()
    {
        UIManager.Instance.ShowEvent(currentEvent);
    }

    public void ChoiceWasMade(EventChoice choice)
    {
        ChoiceInfluence influence = choice.influence;
        currentFood = Mathf.Clamp(currentFood + influence.food, 0, maxFood);
        currentSatisfaction = Mathf.Clamp(currentSatisfaction + influence.satisfaction, 0, maxSatisfaction);
        currentRemedy = Mathf.Clamp(currentRemedy + influence.remedy, 0, maxRemedy);
        currentWater = Mathf.Clamp(currentWater + influence.water, 0, maxWater);
        UIManager.Instance.UpdateSliders();
        UIManager.Instance.HideEvent();
        NextEvent();
    }

    private void NextEvent()
    {
        if(DayMoment == DayMoments.Night)
        {
            DayMoment = DayMoments.Morning;
            currentDay++;
        }
        else
        {
            DayMoment++;
        }
        GetNewEvent();
    }

    private void GetNewEvent()
    {
        int index = Random.Range(0, events[(int)DayMoment].Length);
        currentEvent = (Event)events[(int)DayMoment][index];
        StartEvent();
    }
}
