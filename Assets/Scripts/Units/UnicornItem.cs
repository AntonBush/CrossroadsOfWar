using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornItem : MonoBehaviour
{

    public Unicorn unicorn;
    public Transform placeLeft;
    public Transform placeRight;
    [HideInInspector]
    public SpriteRenderer SR;
    [HideInInspector]
    public bool useSword;

    float timer, posX, posY;
    bool onetimeLeft, onetimeRight, oneside, onesideTurn;

    float tempX;

    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }

    public float ShakeNumber(float number)
    {
        if (oneside)
        {
            if (number < 0.35f)
            {
                number += Time.deltaTime;
            }
            else
            {
                oneside = false;
            }
        }
        else
        {
            if (number > -0.35f)
            {
                number -= Time.deltaTime;
            }
            else
            {
                oneside = true;
            }
        }
        return number;
    }

    void Turn()
    {
        if (useSword)
        {
            if (!onesideTurn)
            {
                if (SR.flipX)
                {
                    if (transform.localEulerAngles.z < 105 || transform.localEulerAngles.z > 145)
                    {
                        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + 42f);
                    }
                    else
                    {
                        onesideTurn = true;
                    }
                }
                else
                {
                    if (transform.localEulerAngles.z > 245 || transform.localEulerAngles.z < 205)
                    {
                        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z - 42f);
                    }
                    else
                    {
                        onesideTurn = true;
                    }
                }
            }
            else
            {
                if (SR.flipX)
                {
                    if (transform.localEulerAngles.z > 20)
                    {
                        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z - 12f);
                    }
                    else
                    {
                        onesideTurn = false;
                        useSword = false;
                    }
                }
                else
                {
                    if (transform.localEulerAngles.z > 20)
                    {
                        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + 12f);
                    }
                    else
                    {
                        onesideTurn = false;
                        useSword = false;
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (timer < 1) timer += Time.deltaTime;
        SR.flipX = unicorn.SR.flipX;

        if (SR.flipX)
        {
            if (!onetimeLeft)
            {
                tempX = transform.position.x;
                timer = 0;
                onetimeLeft = true;
                onetimeRight = false;
            }
            else
            {
                posX = Mathf.Lerp(tempX, placeLeft.position.x, timer);
            }
        }
        else
        {

            if (!onetimeRight)
            {
                tempX = transform.position.x;
                timer = 0;
                onetimeRight = true;
                onetimeLeft = false;
            }
            else
            {
                posX = Mathf.Lerp(tempX, placeRight.position.x, timer);
            }
        }

        posY = ShakeNumber(posY);

        Turn();

        transform.position = new Vector2(posX, posY);
    }
}
