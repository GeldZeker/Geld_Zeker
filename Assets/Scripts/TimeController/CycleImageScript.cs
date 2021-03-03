using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleImageScript : MonoBehaviour
{
    [SerializeField]
    private GameObject dayImage;

    [SerializeField]
    private GameObject nightImage;

    // Update is called once per frame
    void Update()
    {
        if (TimeController.instance.latestDayNightCyclePart == "d")
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
