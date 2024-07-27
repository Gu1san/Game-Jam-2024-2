using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float maxFood { get; private set; } = 100;
    public float currentFood { get; private set; }
    public float maxWater { get; private set; } = 100;
    public float currentWater { get; private set; }
    public float maxSatisfaction { get; private set; } = 100;
    public float currentSatisfaction { get; private set; }
    public float maxRemedy { get; private set; } = 100;
    public float currentRemedy { get; private set; }
    public int currentDay { get; private set; }
    [SerializeField] Event currentEvent;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentFood = 50;
        currentWater = 50;
        currentRemedy = 50;
        currentSatisfaction = 50;
        UIManager.Instance.UpdateSliders();
        StartEvent();
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
    }
}
