using BWolf.Utilities.PlayerProgression.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBeforeQuest : MonoBehaviour
{
    [SerializeField]
    private Quest quest = null;

    [SerializeField]
    private GameObject toHideGameObject = null;

    private void Update()
    {
        if (quest.IsActive) toHideGameObject.SetActive(true);
        if (!quest.IsActive && !quest.IsCompleted) toHideGameObject.SetActive(false);
    }
}
