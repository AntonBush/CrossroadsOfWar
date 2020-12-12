using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampAlertSystem : MonoBehaviour {

    public Transform Player;
    public GameObject AlertLeft, AlertRight;
    float cooldownAlertLeft, cooldownAlertRight;
    float timerAlertLeft, timerAlertRight;

    public void SeeEnemy(Vector2 myPosition)
    {
        if(myPosition.x - Player.position.x > 15f)
        {
            if(cooldownAlertRight <= 0)
            {
                cooldownAlertRight = 20f;
                timerAlertRight = 2f;
            }
        }
        else if(myPosition.x - Player.position.x < -15f)
        {
            if (cooldownAlertLeft <= 0)
            {
                cooldownAlertLeft = 20f;
                timerAlertLeft = 2f;
            }
        }
    }

    private void Update()
    {
        if (cooldownAlertLeft > 0) cooldownAlertLeft -= Time.deltaTime;
        if (cooldownAlertRight > 0) cooldownAlertRight -= Time.deltaTime;

        if(timerAlertLeft > 0)
        {
            timerAlertLeft -= Time.deltaTime;
            AlertLeft.SetActive(true);
        }
        else
            AlertLeft.SetActive(false);

        if (timerAlertRight > 0)
        {
            timerAlertRight -= Time.deltaTime;
            AlertRight.SetActive(true);
        }
        else
            AlertRight.SetActive(false);
    }

}
