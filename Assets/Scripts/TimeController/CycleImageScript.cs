using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleImageScript : MonoBehaviour
{
    [SerializeField]
    private GameObject dayImage = null;

    [SerializeField]
    private GameObject nightImage = null;

    // Update is called once per frame
    void Update()
    {
        if (TimeController.Instance.latestDayNightCyclePart == "d")
        {
            dayImage.SetActive(true);
            nightImage.SetActive(false);
        } else
        {
            dayImage.SetActive(false);
            nightImage.SetActive(true);
        }
    }
}
