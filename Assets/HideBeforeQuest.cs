using BWolf.Utilities.PlayerProgression.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Monobehaviour object that helps to hide a GameObject based on progress of a Quest.</summary>
/*                                  -=-=-=-=-=-=- WARNING! -=-=-=-=-=-=-
    This Script is NOT generic, for now it can only be used for the one button that is used for now!*/
public class HideBeforeQuest : MonoBehaviour
{
    [SerializeField]
    private Quest quest = null;

    [SerializeField]
    private GameObject toHideGameObject = null;

    private void Update()
    {
        string timeStr = TimeSpan.FromSeconds(TimeController.Instance.elapsedTime).ToString();
        int index = timeStr.IndexOf(':');
        int hours = Int32.Parse(timeStr.Substring(0, index));


        if (quest.IsActive || quest.IsCompleted)
        {
            toHideGameObject.SetActive(true);
            toHideGameObject.GetComponent<Button>().interactable = true;
        }
        if (!quest.IsActive && !quest.IsCompleted) toHideGameObject.SetActive(false);

        if (5 < hours && hours < 11 && toHideGameObject.activeInHierarchy) toHideGameObject.GetComponent<Button>().interactable = true;
        else toHideGameObject.GetComponent<Button>().interactable = false;
    }
}
