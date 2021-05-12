using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolunteerWorkProgressBar : MonoBehaviour
{
    [SerializeField]
    public static VolunteerWorkProgressBar Instance;

    [SerializeField]
    private GameObject arrow1 = null;
    [SerializeField]
    private GameObject arrow2 = null;
    [SerializeField]
    private GameObject house = null;
    [SerializeField]
    private GameObject bicycle = null;
    [SerializeField]
    private GameObject sport = null;
    [SerializeField]
    private GameObject coffee = null;
    [SerializeField]
    private GameObject nature = null;


    public void StartAnimation(string animationName)
    {
        bicycle.SetActive(true);
        StartCoroutine(followUpAnimation(animationName));
    }

    private IEnumerator followUpAnimation(string animationName)
    {
        yield return new WaitForSeconds(3.5f);
        arrow1.SetActive(true);
        switch (animationName)
        {
            case "sport":
                {
                    sport.SetActive(true);
                }
                break;
            case "coffee":
                {
                    coffee.SetActive(true);
                }
                break;
            case "nature":
                {
                    nature.SetActive(true);
                }
                break;
        }
        yield return new WaitForSeconds(3.5f);
        arrow2.SetActive(true);
        house.SetActive(true);
    }
}
