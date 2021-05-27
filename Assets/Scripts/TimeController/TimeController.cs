using BWolf.Behaviours.SingletonBehaviours;
using GameStudio.GeldZeker.Player;
using GameStudio.GeldZeker.SceneTransitioning;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeController : SingletonBehaviour<TimeController>
{
    [SerializeField]
    public static TimeController instance;

    [Header("In-game Time")]
    [SerializeField]
    private string timeCounter;

    [SerializeField]
    public float elapsedTime;

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

    private const string FILE_InGameTime = "ProgressSaves/Time/TimeInGameClosed";

    private int daySeconds = 86399;

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

    private void OnApplicationQuit()
    {
        base.OnDestroy();
        EndTimer();
    }

    /// <summary>Starts the timer.</summary>
    public void BeginTimer()
    {
        if (timerGoing)
        {
            return;
        }

        LoadDateTimeClosed();
        timerGoing = true;
        elapsedTime = (float)((3600 * elapsedHoursIG) + 0f);

        if (elapsedTime > daySeconds)
        {
            elapsedTime = elapsedTime - (elapsedTime / daySeconds) * daySeconds;
        }

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
            if (elapsedTime > daySeconds) elapsedTime = 0;

            elapsedTime += Time.deltaTime * 60;
            dayNightCycleTime = TimeSpan.FromSeconds(elapsedTime);
            string dayNightCycleStr = "Time: " + dayNightCycleTime.ToString("hh':'mm");
            timeCounter = dayNightCycleStr;

            if (dayNightCycleStr != prevFrameTime)
            {
                //Debug.Log(timeCounter);

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
        if (GameFileSystem.LoadFromFile(FILE_InGameTime, out long[] outValue))
        {
            long outValueDateTime = outValue[0];
            long outValueInGameTime = outValue[1];

            elapsedSecondsRL = (DateTime.Now - DateTime.FromFileTime(outValueDateTime)).TotalSeconds;

            if (!resetActive)
            {
                DateTime lastInGameTimeClosed = DateTime.FromFileTime(outValueInGameTime).AddMinutes(elapsedSecondsRL);
                double decimalIG = TimeStringToDouble(lastInGameTimeClosed.TimeOfDay.ToString(), 0, 1);

                elapsedHoursIG = decimalIG;

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
        DateTime latestDateTime = DateTime.Now;
        long[] timeStamps = new long[] { latestDateTime.ToFileTime(), inGameTime.ToFileTime()};

        GameFileSystem.SaveToFile(FILE_InGameTime, timeStamps);
    }

    /// <summary>Resets the time.</summary>
    public void ResetDateTime()
    {
        resetActive = true;
        EndTimer();
    }
}