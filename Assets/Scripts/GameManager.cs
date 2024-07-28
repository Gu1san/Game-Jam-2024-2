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
    public float MaxStatusValue { get; private set; } = 100;
    public float CurrentFood { get; private set; }
    public float CurrentWater { get; private set; }
    public float CurrentSatisfaction { get; private set; }
    public float CurrentMoney { get; private set; }
    public int CurrentDay { get; private set; } = 1;
    public int MaximizedStatus { get; private set; } = 0;

    [SerializeField] Event currentEvent;
    [SerializeField] Transform npcSpawn;
    [SerializeField] int daysDuration = 10;

    private bool isFoodMaximized = false;
    private bool isSatisfactionMaximized = false;
    private bool isMoneyMaximized = false;
    private bool isWaterMaximized = false;

    readonly Object[][] events = new Object[3][];

    public DayMoments DayMoment { get; private set; }

    public ChoiceInfluence LastStatus { get; private set; } = new() {
        food = 50,
        water = 50,
        money = 50,
        satisfaction = 50
    };

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GetEvents();
        CurrentFood = 50;
        CurrentWater = 50;
        CurrentMoney = 50;
        CurrentSatisfaction = 50;
        StartNewDay();
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
        GameObject npc = Instantiate(currentEvent.npcModel, npcSpawn.position, npcSpawn.rotation, npcSpawn);
        npc.GetComponent<Animator>().CrossFade(currentEvent.npcPose, 0);
        UIManager.Instance.ShowEvent(currentEvent);
    }

    public void ChoiceWasMade(EventChoice choice)
    {
        UIManager.Instance.UnhighlightSliders();
        ChoiceInfluence influence = choice.influence;
        CurrentFood = Mathf.Clamp(CurrentFood + influence.food, 0, MaxStatusValue);
        CurrentSatisfaction = Mathf.Clamp(CurrentSatisfaction + influence.satisfaction, 0, MaxStatusValue);
        CurrentMoney = Mathf.Clamp(CurrentMoney + influence.money, 0, MaxStatusValue);
        CurrentWater = Mathf.Clamp(CurrentWater + influence.water, 0, MaxStatusValue);
        UIManager.Instance.UpdateSliders();
        if (CurrentFood <= 0 || CurrentWater <= 0 || CurrentMoney <= 0 || CurrentSatisfaction <= 0)
        {
            UIManager.Instance.GameOver();
            return;
        }
        if (CurrentFood >= MaxStatusValue && !isFoodMaximized)
        {
            MaximizedStatus++;
            isFoodMaximized = true;
        }
        if (CurrentWater >= MaxStatusValue && !isWaterMaximized)
        {
            MaximizedStatus++;
            isWaterMaximized = true;
        }
        if (CurrentSatisfaction >= MaxStatusValue && !isSatisfactionMaximized)
        {
            MaximizedStatus++;
            isSatisfactionMaximized = true;
        }
        if (CurrentMoney >= MaxStatusValue && !isMoneyMaximized)
        {
            MaximizedStatus++;
            isMoneyMaximized = true;
        }
        UIManager.Instance.HideEvent();
        Destroy(npcSpawn.GetChild(0).gameObject);
        NextEvent();
    }

    private void NextEvent()
    {
        if(DayMoment == DayMoments.Night)
        {
            if(CurrentDay >= daysDuration)
            {
                UIManager.Instance.WinGame();
                return;
            }
            CurrentDay++;
            StartNewDay();
        }
        else
        {
            DayMoment++;
            UIManager.Instance.UpdateDayInfo();
            GetNewEvent();
        }
    }

    void StartNewDay()
    {
        if (CurrentDay > 1)
        {
            UIManager.Instance.ShowReport();
        }
        DayMoment = DayMoments.Morning;
        LastStatus = new() { 
            food = CurrentFood,
            water = CurrentWater,
            money = CurrentMoney,
            satisfaction = CurrentSatisfaction
        };
    }

    public void GetNewEvent()
    {
        int index = Random.Range(0, events[(int)DayMoment].Length);
        currentEvent = (Event)events[(int)DayMoment][index];
        StartEvent();
    }
}
