using BWolf.Utilities.PlayerProgression.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        if (quest.IsActive)
        {
            toHideGameObject.SetActive(true);
            toHideGameObject.GetComponent<Button>().interactable = true;
        }
        if (!quest.IsActive && !quest.IsCompleted) toHideGameObject.SetActive(false);

        if (6 < hours && hours < 10 && toHideGameObject.activeInHierarchy) toHideGameObject.GetComponent<Button>().interactable = true;
        else toHideGameObject.GetComponent<Button>().interactable = false;
    }
}
