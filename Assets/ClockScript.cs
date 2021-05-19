using Assets.Scripts.Player.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockScript : MonoBehaviour
{
    [SerializeField]
    private GameObject secondHand;
    [SerializeField]
    private GameObject minuteHand;
    [SerializeField]
    private GameObject hourHand;
    [SerializeField]
    private bool timelapseMode = false;
    [SerializeField]
    private GameObject progressBar = null;
    [SerializeField]
    private PlayerVolunteerProperty volunteerProperty = null;

    private bool timelapseGoing;

    private bool animationInitiate = true;

    private string oldSeconds = null;
    

    private TimeSpan inGameTime = new TimeSpan();

    void Update()
    {
        inGameTime = TimeController.Instance.dayNightCycleTime;
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
        int time = 1;

        if (timelapseMode)
        {
            if (timelapseGoing == false && animationInitiate == true)
            {
                volunteerProperty.SetVolunteerName(VolunteerType.Nature);
                string volunteerWork = volunteerProperty.GetVolunteerName().ToString().ToLower();

                if (progressBar && volunteerProperty) progressBar.GetComponent<VolunteerWorkProgressBar>().StartAnimation(volunteerWork);

                timelapseGoing = true;
                StartCoroutine(Timelapse());
            }
            if (animationInitiate == true)
            {
                iTween.RotateTo(minuteHand, iTween.Hash("z", -3600, "time", 10, "easetype", "linear"));
                iTween.RotateTo(hourHand, iTween.Hash("z", -3600, "time", 100, "easetype", "linear"));
            }
        } else
        {
            iTween.RotateTo(secondHand, iTween.Hash("z", secondsInt * 6 * -1, "time", time, "easetype", "easeOutQuint"));
            iTween.RotateTo(minuteHand, iTween.Hash("z", minutesInt * 6 * -1, "time", time, "easetype", "easeOutElastic"));

            if (hoursInt > 12) hoursInt = hoursInt - 12;

            if (minutesInt == 0 || minutesInt == 12 || minutesInt == 24 || minutesInt == 36 || minutesInt == 48)
            {
                iTween.RotateTo(hourHand, iTween.Hash("z", (hoursInt + hourDistance) * 30 * -1, "time", 1, "easetype", "easeOutQuint"));
            }
        }
    }

    private IEnumerator Timelapse()
    {
        float timer = 0.0f;

        while (timelapseGoing)
        {
            timer += Time.deltaTime;
            int seconds = (int)(timer % 60);

            Debug.Log(seconds);

            if (seconds == 10)
            {
                iTween.Stop(minuteHand);
                iTween.Stop(hourHand);
                StopTimer();
            }

            yield return null;
        }
    }

    private void StopTimer()
    {
        timelapseGoing = false;
        animationInitiate = false;
    }
}
