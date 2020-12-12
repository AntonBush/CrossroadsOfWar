using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepScript : MonoBehaviour
{
    public Image BlackScreen;
    public Creature Player;
    public GameObject sleepHint;

    bool sleep;
    MainFire mainFire;
    [SerializeField]
    TimeCount timeCount;

    float screenAlpha;

    bool onetime, onetime4;

    bool plusDay;

    void UpdateSleep()
    {
        BlackScreen.color = new Color(0, 0, 0, screenAlpha);
        if (sleep)
        {
            if (screenAlpha < 1)
            {
                Player.gameObject.SetActive(false);
                screenAlpha += Time.deltaTime;
                if(timeCount.hours > 20) plusDay = true;
            }
            else
            {
                if (!onetime4)
                {
                    timeCount.hours = 4;
                    onetime4 = true;
                }
                else
                {
                    if (timeCount.hours < 5 || timeCount.hours >= 21)
                    {
                        Time.timeScale = 20f;
                    }
                    else
                    {
                        if(plusDay) 
                        {
                            timeCount.days++;
                            timeCount.daysCount.text = timeCount.days.ToString();
                            plusDay = false;
                        }
                        Time.timeScale = 1;
                        sleep = false;
                    }
                }
            }
        }
        else
        {
            if (screenAlpha > 0)
            {
                screenAlpha -= Time.deltaTime;
            }
            else
            {
                if (!onetime)
                {
                    Player.gameObject.SetActive(true);
                    onetime = true;
                }
            }
        }
    }

    void CheckSleep()
    {
        if (mainFire.buildingLevel > 1)
        {
            if (timeCount.hours >= 21 || timeCount.hours < 5)
            {
                if (Vector2.Distance(Player.transform.position, transform.position) < 3.5f &&
        Player.GetComponent<MovingController>().health > 0)
                {
                    sleepHint.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        onetime = onetime4 = false;
                        sleep = true;
                    }
                }
                else
                {
                    sleepHint.SetActive(false);
                }
            }
            else
            {
                sleepHint.SetActive(false);
            }
        }
    }

    private void Start()
    {
        mainFire = GetComponent<MainFire>();
    }
    private void Update()
    {
        CheckSleep();
        UpdateSleep();
    }
}
