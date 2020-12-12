using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCount : MonoBehaviour
{
    public int hours;
    public int minutes;
    public int days;

    public Text daysCount;

    public GameManager gameManager;

    public IEnumerator dayTiming;

    public bool isDay
    {
        get
        {
			if(hours > 3 && hours < 22) return true;
			else return false;
        }
    }

    IEnumerator DayTiming()
    {
        for (; ; )
        {
            daysCount.text = days.ToString();

            if (minutes < 59)
            {
                minutes++;
            }
            else
            {
                if (hours < 23)
                {
                    hours++;
                    minutes = 0;
                }
                else
                {
                    days++;
                    hours = minutes = 0;
                }
            }
            yield return new WaitForSeconds(0.2f);
        }

    }
    void Start()
    {
        dayTiming = DayTiming();
        StartCoroutine(dayTiming);
    }
}
