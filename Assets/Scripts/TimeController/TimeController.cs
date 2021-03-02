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

    [SerializeField]
    private string timeCounter;

    [SerializeField]
    private TimeSpan dayNightCycleTime;

    [SerializeField]
    private bool timerGoing;

    [SerializeField]
    private float elapsedTime;

    [SerializeField]
    private string prevFrameTime;

    [SerializeField]
    private double latestDecimalTime;

    [SerializeField]
    public string latestDayNightCyclePart;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timeCounter = "00:00";
        timerGoing = false;
    }   

    public void BeginTimer()
    {
        timerGoing = true;
        elapsedTime = (360 * 58) + 0f;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerGoing = false;
    }

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
                double decimalHours = Convert.ToDouble(timeCounter.Split(':')[1]);
                double decimalMinutes = Convert.ToDouble(timeCounter.Split(':')[2]) / 6 * 0.1;
                double decimalTime = decimalHours + decimalMinutes;

                Debug.Log(timeCounter);
                latestDecimalTime = decimalTime;

                string currentDayNightCyclePart;
                if (6 < decimalTime && decimalTime < 20) currentDayNightCyclePart = "d";
                else currentDayNightCyclePart = "n";

                if (latestDayNightCyclePart != currentDayNightCyclePart)
                {
                    Debug.Log("Canvas CHANGE");
                    latestDayNightCyclePart = currentDayNightCyclePart;
                }
            }

            prevFrameTime = dayNightCycleStr;
            yield return null;
        }
    }

    // Function to save time - Future
    public void DisplaySessionTime()
    {
        //DateTime time = DateTime.Now;
        //Debug.Log(time.TimeOfDay.ToString("hh':'mm"));
        //Debug.Log(latestDecimalTime);
    }
}
