using BWolf.Behaviours.SingletonBehaviours;
using GameStudio.GeldZeker.Player;
using System;
using System.Collections;
using UnityEngine;


public class TimeController : SingletonBehaviour<TimeController>
{
    [SerializeField]
    public static TimeController instance;

    [Header("In-game Time")]
    [SerializeField]
    private string timeCounter;

    [SerializeField]
    private float elapsedTime;

    [SerializeField]
    public string latestDayNightCyclePart;

    [Header("Settings")]
    [SerializeField]
    private bool timerGoing;

    private bool resetActive;

    public TimeSpan dayNightCycleTime;

    private double elapsedSecondsRL;

    private double elapsedHoursIG;

    private string prevFrameTime;

    private double latestDecimalTime;

    private const string FILE_DateTime = "Time/DateTimeClosed";

    private const string FILE_InGameTime = "Time/TimeInGameClosed";

    protected override void Awake()
    {
        base.Awake();

        if (isDuplicate)
        {
            return;
        }

        instance = this;
    }

    private void Start()
    {
        timerGoing = false;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        EndTimer();
    }

    public void BeginTimer()
    {
        LoadDateTimeClosed();
        timerGoing = true;
        elapsedTime = (float)((3600 * elapsedHoursIG) + 0f);

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        Debug.Log("Latest Decimal Time: " + latestDecimalTime);
        timerGoing = false;
        SaveDateTimeClosed();
    }

    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            elapsedTime += Time.deltaTime * 60;
            dayNightCycleTime = TimeSpan.FromSeconds(elapsedTime);
            string dayNightCycleStr = "Time: " + dayNightCycleTime.ToString("hh':'mm");
            timeCounter = dayNightCycleStr;

            if (dayNightCycleStr != prevFrameTime)
            {
                Debug.Log(timeCounter);

                double decimalTime = TimeStringToDouble(dayNightCycleStr, 1, 2);

                latestDecimalTime = decimalTime;

                string currentDayNightCyclePart;

                if (6 < decimalTime && decimalTime < 22) currentDayNightCyclePart = "d";
                else currentDayNightCyclePart = "n";

                if (latestDayNightCyclePart != currentDayNightCyclePart)
                {
                    latestDayNightCyclePart = currentDayNightCyclePart;
                }
            }

            prevFrameTime = dayNightCycleStr;
            yield return null;
        }
    }

    private double TimeStringToDouble(string timeString, int cutOff1, int cutOff2)
    {
        double decimalHours = Convert.ToDouble(timeString.Split(':')[cutOff1]);
        double decimalMinutes = Convert.ToDouble(timeString.Split(':')[cutOff2]) / 6 * 0.1;
        double decimalTime = decimalHours + decimalMinutes;

        return decimalTime;
    }

    private void LoadDateTimeClosed()
    {
        if (GameFileSystem.LoadFromFile(FILE_DateTime, out long outValueDateTime))
        {
            DateTime lastDateTimeClosed = DateTime.FromFileTime(outValueDateTime);
            elapsedSecondsRL = (DateTime.Now - lastDateTimeClosed).TotalSeconds;
        }

        if (GameFileSystem.LoadFromFile(FILE_InGameTime, out long outValueInGameTime))
        {
            if (!resetActive)
            {
                DateTime lastInGameTimeClosed = DateTime.FromFileTime(outValueInGameTime);
                double decimalIG = lastInGameTimeClosed.Hour + (lastInGameTimeClosed.Minute * 0.01);

                TimeSpan convertedTime = TimeSpan.FromSeconds(elapsedSecondsRL * 60);
                string convertedTimeStr = convertedTime.ToString("hh':'mm");
                double decimalTime = TimeStringToDouble(convertedTimeStr, 0, 1);

                elapsedHoursIG = Convert.ToDouble(decimalIG) + decimalTime;

            } else
            {
                elapsedHoursIG = 7;
                resetActive = false;
            }
        }
    }

    /// <summary>Saves the current datatime as file time</summary>
    private void SaveDateTimeClosed()
    {
        DateTime inGameTime = new DateTime(1900, 12, 12).AddHours(latestDecimalTime);
        GameFileSystem.SaveToFile(FILE_InGameTime, inGameTime.ToFileTime());
        GameFileSystem.SaveToFile(FILE_DateTime, DateTime.Now.ToFileTime());
    }

    public void ResetDateTime()
    {
        resetActive = true;
        EndTimer();
    }
}