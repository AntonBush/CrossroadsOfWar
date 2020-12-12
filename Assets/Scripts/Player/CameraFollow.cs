using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public MovingController controller;
    float timer;
    public float timer2;
    public bool MayGo = true;
    public float timer3;

    Vector3 lastPosition;

    Vector3 tempPosition;

    public float rangeShake;
    public float speedShake;

    bool oneside;

    float cameraY;

    public float ShakeNumber(float number)
    {
        if (oneside)
        {
            if (number < rangeShake + 2)
            {
                number += Time.deltaTime * speedShake;
            }
            else
            {
                oneside = false;
            }
        }
        else
        {
            if (number > -rangeShake + 2)
            {
                number -= Time.deltaTime * speedShake;
            }
            else
            {
                oneside = true;
            }
        }
        return number;
    }

    void Start()
    {
        timer3 = 1f;
        lastPosition = transform.position;
        cameraY = 2;
    }

    void Update()
    {
        cameraY = ShakeNumber(cameraY);

        if (controller != null)
        {
            float X = Mathf.Lerp(controller.transform.position.x - 2f, controller.transform.position.x + 2f, timer3);
            if (controller.speedX > 0) if (timer3 < 1) timer3 += 0.04f;
            if (controller.speedX < 0) if (timer3 > 0) timer3 -= 0.04f;
            if (controller.isActiveAndEnabled)
            {
                if (controller.speedX == 0)
                {
                    if (timer > 0)
                    {
                        timer -= 0.1f;
                    }
                }
                else if (Mathf.Abs(controller.speedX) <= 1)
                {
                    if (timer < 0.5f)
                    {
                        timer += 0.1f;
                    }
                }
                else if (Mathf.Abs(controller.speedX) <= 2)
                {
                    if (timer < 1f)
                    {
                        timer += 0.1f;
                    }
                }
                Vector3 destination = new Vector3(X, 1, -10);
                if (timer2 < 1)
                {
                    timer2 += 0.05f;
                }
                if (MayGo)
                    tempPosition = Vector3.Lerp(lastPosition, destination, timer2);
                transform.position = new Vector3(tempPosition.x,cameraY,-25);
            }
        }

    }
}
