using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampDissapear : MonoBehaviour
{
    public MovingController player;
    public TimeCount time;
    public SpriteRenderer SR;
    public GameObject Lighting;

    private void Update()
    {
        if (time.hours == 20)
        {
            SR.enabled = true;
            Lighting.SetActive(true);
        }
        if (time.hours == 5 || player.health <= 0)
        {
			SR.enabled = false;
            Lighting.SetActive(false);
        }
    }

}
