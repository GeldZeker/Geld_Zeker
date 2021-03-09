using GameStudio.GeldZeker.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
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

    public TimeSpan dayNightCycleTime;

    private double elapsedSecondsRL;

    private double elapsedHoursIG;

    private string prevFrameTime;

    private double latestDecimalTime;

    private const string FILE_DateTime = "Time/DateTimeClosed";

    private const string FILE_InGameTime = "Time/TimeInGameClosed";

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        LoadDateTimeClosed();
        timeCounter = "00:00";
        timerGoing = false;
    }

    /// <summary>Starts the timer.</summary>
    public void BeginTimer()
    {
        timerGoing = true;
        elapsedTime = (float)((3600 * elapsedHoursIG) + 0f);

        StartCoroutine(UpdateTimer());
    }

    /// <summary>Ends the timer.</summary>
    public void EndTimer()
    {
        timerGoing = false;
        SaveDateTimeClosed();
    }

    /// <summary>Updates the timer.</summary>
    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            elapsedTime += Time.deltaTime * 60;
            dayNightCycleTime = TimeSpan.FromSeconds(elapsedTime);
            string dayNightCycleStr = "Time: " + dayNightCycleTime.ToString("hh':'mm");
            timeCounter = dayNightCycleStr;

            if(dayNightCycleStr != prevFrameTime)
            {
                double decimalTime = TimeStringToDouble(dayNightCycleStr, 1, 2);
                //Debug.Log(timeCounter);
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

    /// <summary>Converts a time string to double in decimal numbers.</summary>
    private double TimeStringToDouble(string timeString, int cutOff1, int cutOff2)
    {
        double decimalHours = Convert.ToDouble(timeString.Split(':')[cutOff1]);
        double decimalMinutes = Convert.ToDouble(timeString.Split(':')[cutOff2]) / 6 * 0.1;
        double decimalTime = decimalHours + decimalMinutes;

        return decimalTime;
    }

    /// <summary>Loads the latest datatime from file on previous app close.</summary>
    private void LoadDateTimeClosed()
    {
        if (GameFileSystem.LoadFromFile(FILE_DateTime, out long outValueDateTime))
        {
            DateTime lastDateTimeClosed = DateTime.FromFileTime(outValueDateTime);
            elapsedSecondsRL = (DateTime.Now - lastDateTimeClosed).TotalSeconds;
        }

        if (GameFileSystem.LoadFromFile(FILE_InGameTime, out double outValueInGameTime))
        {
            TimeSpan convertedTime = TimeSpan.FromSeconds(elapsedSecondsRL * 60);
            string convertedTimeStr = convertedTime.ToString("hh':'mm");
            double decimalTime = TimeStringToDouble(convertedTimeStr, 0, 1);

            elapsedHoursIG = outValueInGameTime + decimalTime;
        } 
        else elapsedHoursIG = 7;
    }

    /// <summary>Saves the current datatime as file time</summary>
    private void SaveDateTimeClosed()
    {
        GameFileSystem.SaveToFile(FILE_DateTime, DateTime.Now.ToFileTime());
        GameFileSystem.SaveToFile(FILE_InGameTime, latestDecimalTime);
    }
}
