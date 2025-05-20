using System;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private int minutes;
    public int Minutes { get { return minutes; } set { minutes = value; OnMinutesChange(value); } }
    private int hours;
    public int Hours { get { return hours; } set { hours = value; OnHoursChange(value); } }
    private int days;
    public int Days { get { return days; } set { days = value; OnDaysChange(value); } }

    private float tempSeconds;

    [SerializeField] private TMP_Text minute;
    [SerializeField] private TMP_Text hour;

    public DayEnum day;
    public StationEnum yearStation;

    public float roamerCalculator;
    public void Update()
    {
        tempSeconds += Time.deltaTime;
        if(tempSeconds >= 1)
        {
            Minutes += 1;
            tempSeconds = 0;
        }
    }

    private void OnMinutesChange(int value)
    {
        Debug.Log("Minutes Update");
        if(value >= 60)
        {
            Hours++;            
            minutes = 0;
        }
        if(Hours >= 24)
        {
            Hours = 0;
            Days++;
        }
        UpdateTheRoamer();
        minute.text = minutes.ToString();
    }
    private void OnHoursChange(int value)
    {
        Debug.Log("Hour change called");
        hour.text = hours.ToString();
        foreach (FarmingLand land in gameManager.instance.farmingCubes)
        {
            land.EndOfDayUpdate();
        }
    }
    private void OnDaysChange(int value)
    {
        throw new NotImplementedException();
    }
    public void UpdateTheRoamer()
    {
        roamerCalculator++;
        if(roamerCalculator == 1) 
        {
            gameManager.instance.test[0].GetPath();
        }
        if (roamerCalculator == 21)
        {
            gameManager.instance.test[0].GetPath();
        }
        if (roamerCalculator == 41)
        {
            gameManager.instance.test[0].GetPath();
        }
        if (roamerCalculator >= 60) 
        {
            roamerCalculator = 0;
        }
    }
}
