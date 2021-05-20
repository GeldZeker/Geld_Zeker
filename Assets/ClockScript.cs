using Assets.Scripts.Player.Properties;
using GameStudio.GeldZeker.SceneTransitioning;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>A MonoBehaviour object that represents the script for the clock.</summary>
public class ClockScript : MonoBehaviour
{
    [SerializeField]
    private GameObject secondHand = null;
    [SerializeField]
    private GameObject minuteHand = null;
    [SerializeField]
    private GameObject hourHand = null;
    [SerializeField]
    private bool timelapseMode = false;
    [SerializeField]
    private string timelapseForwardScene = "";
    [SerializeField]
    private bool volunteerWork = false;
    [SerializeField]
    private GameObject progressBar = null;
    [SerializeField]
    private PlayerVolunteerProperty volunteerProperty = null;

    private bool timelapseGoing;

    private bool animationInitiate = true;

    private string oldSeconds = null;
    

    private TimeSpan inGameTime = new TimeSpan();

    /// <summary>Update Clock functionality.</summary>
    void Update()
    {
        inGameTime = TimeController.instance.dayNightCycleTime;
        string seconds = inGameTime.ToString("ss");

        if (seconds != oldSeconds) UpdateTimer();

        oldSeconds = seconds;
    }

    /// <summary>Update state of the Clock UI.</summary>
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

    /// <summary>Coroutine for Timelapse. Execute certain functions when timelapse is over.</summary>
    private IEnumerator Timelapse()
    {
        float timer = 0.0f;

        while (timelapseGoing)
        {
            timer += Time.deltaTime;
            int seconds = (int)(timer % 60);

            if (seconds == 10)
            {
                iTween.Stop(minuteHand);
                iTween.Stop(hourHand);
                StopTimer();
            }

            yield return null;
        }
    }

    /// <summary>Stop Timelapse UI. Reroutes if timelapse is used for volunteerWork.</summary>
    private void StopTimer()
    {
        timelapseGoing = false;
        animationInitiate = false;

        if (volunteerWork) OnVolunteerWorkEnd();
        // This can be expanded in the future for more use-cases outside Volunteer Work with a string(instead of a boolean) and a switch-case.
    }

    /// <summary>Method is executed after Timelapse if Volunteer Work.</summary>
    private void OnVolunteerWorkEnd()
    {
        if (!timelapseForwardScene.Equals(""))
        {
            SceneTransitionSystem.Instance.Transition(SceneTransitionSystem.DefaultTransition, timelapseForwardScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
        // Add additional payout route to function.
    }
}
