using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockScript : MonoBehaviour
{

    public GameObject secondHand;
    public GameObject minuteHand;
    public GameObject hourHand;
    private string oldSeconds = null;
    

    private TimeSpan inGameTime = new TimeSpan();

    void Update()
    {
        inGameTime = TimeController.instance.dayNightCycleTime;
        string seconds = inGameTime.ToString("ss");

        if (seconds != oldSeconds) UpdateTimer();

        oldSeconds = seconds;
    }

    void UpdateTimer()
    {
        int secondsInt = int.Parse(inGameTime.ToString("ss"));
        int minutesInt = int.Parse(inGameTime.ToString("mm"));
        int hoursInt = int.Parse(inGameTime.ToString("hh"));
        float hourDistance = (float)(minutesInt) / 60f;

        iTween.RotateTo(secondHand, iTween.Hash("z", secondsInt * 6 * -1, "time", 1, "easetype", "easeOutQuint"));
        iTween.RotateTo(minuteHand, iTween.Hash("z", minutesInt * 6 * -1, "time", 1, "easetype", "easeOutElastic"));

        if (hoursInt > 12) hoursInt = hoursInt - 12;

        if (minutesInt == 0 || minutesInt == 12 || minutesInt == 24 || minutesInt == 36 || minutesInt == 48) {
            iTween.RotateTo(hourHand, iTween.Hash("z", (hoursInt + hourDistance) * 30 * -1, "time", 1, "easetype", "easeOutQuint"));
        }
    }
}
