using Assets.Scripts.Player.Properties;
using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.Player.Properties;
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

    [Header("Player Properties")]
    [SerializeField]
    private PlayerVolunteerProperty volunteerProperty = null;
    [SerializeField]
    private PlayerMoneyProperty playerMoneyProperty = null;
    [SerializeField]
    private double moneyToReceive = 180.00;

    [Header("Quest settings")]
    [SerializeField]
    private Quest volunteerWorkQuest = null;

    [Header("Tamagotchi")]
    [SerializeField]
    private TamagotchiElementProperty happiness = null;

    [SerializeField]
    private int happinessOnCompletion = 10;

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
        if (hoursInt > 12) hoursInt = hoursInt - 12;

        if (timelapseMode)
        {
            if (timelapseGoing == false && animationInitiate == true)
            {
                // Set Clock hands
                hourHand.transform.Rotate(0, 0, (hoursInt + hourDistance) * 30 * -1);
                minuteHand.transform.Rotate(0, 0, minutesInt * 6 * -1);
                // Fetch Volunteer work type
                string volunteerWork = volunteerProperty.GetVolunteerName().ToString().ToLower();
                //Debug.Log(TimeSpan.FromSeconds(TimeController.Instance.elapsedTime));
                //Debug.Log(TimeSpan.FromSeconds(TimeController.Instance.elapsedTime + 36000));

                if (progressBar && volunteerProperty) progressBar.GetComponent<VolunteerWorkProgressBar>().StartAnimation(volunteerWork);

                timelapseGoing = true;
                StartCoroutine(Timelapse());
            }
            if (animationInitiate == true)
            {
                iTween.RotateTo(minuteHand, iTween.Hash("z", -3600, "time", 10, "easetype", "linear"));
                iTween.RotateTo(hourHand, iTween.Hash("z", -3600 - ((hoursInt + hourDistance) * 30 * -1), "time", 100, "easetype", "linear"));
            }
        } else
        {
            iTween.RotateTo(secondHand, iTween.Hash("z", secondsInt * 6 * -1, "time", time, "easetype", "easeOutQuint"));
            iTween.RotateTo(minuteHand, iTween.Hash("z", minutesInt * 6 * -1, "time", time, "easetype", "easeOutElastic"));

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
        AddHoursToTime();
        if (volunteerWork) OnVolunteerWorkEnd();
        // This can be expanded in the future for more use-cases outside Volunteer Work with a string(instead of a boolean) and a switch-case.
    }

    private void AddHoursToTime()
    {
        TimeController.Instance.elapsedTime = TimeController.Instance.elapsedTime + 36000;
    }

    /// <summary>Method is executed after Timelapse if Volunteer Work.</summary>
    private void OnVolunteerWorkEnd()
    {
        if (!timelapseForwardScene.Equals(""))
        {
            SceneTransitionSystem.Instance.Transition(SceneTransitionSystem.DefaultTransition, timelapseForwardScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
        // Add 1 day to timesWorked for every worked shift.
        volunteerProperty.PlusWorkedShift();
        if (volunteerWorkQuest.IsUpdatable)
        {
            // Update volunteer work task.
            IncrementTask volunteerWork = volunteerWorkQuest.GetTask<IncrementTask>("VolunteerWork");
            volunteerWork.Increment();
            // If volunteer work quest is done add happiness.
            if (volunteerWork.IsDone)
            {
                happiness.AddValue(happinessOnCompletion);
            }
        }
        // Check if player has worked more then 8 times, if so pay the player money.
        if (volunteerProperty.CheckHours(volunteerProperty.timesWorked))
        {
            TimeForPayment();
        }
    }

    /// <summary> Method to check if player needs to be payed for working as a volunteer. </summary>
    private void TimeForPayment()
    {
        playerMoneyProperty.AddMoney(moneyToReceive);
        volunteerProperty.Restore();
    }
}
