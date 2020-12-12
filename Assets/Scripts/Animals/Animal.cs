using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : Creature
{
    public Transform Camera;
    public float timerStand { get; private set; }

    public Vector2 tempDest { get; private set; }

    public float maxSpeed = 3f;
    public float minX;
    public float maxX;

    public void InitiateAnimal()
    {
        InitiateCreature();
        timerStand = Random.Range(0, 10f);
    }

    public void Running(float speed)
    {
        speedX = speed;
        if (speed > 0 && SR.flipX) SR.flipX = false;
        if (speed < 0 && !SR.flipX) SR.flipX = true;
    }

    public void GoSomewhere()
    {
        if (timerStand > 0)
        {
            timerStand -= Time.deltaTime;
            tempDest = Vector2.zero;
            speedX = 0;
        }
        else
        {
            if (tempDest == Vector2.zero || tempDest.x > maxX || tempDest.x < minX)
                tempDest = new Vector2(Random.Range(minX, maxX), transform.position.y);
            if (RunTo(tempDest))
            {
                timerStand = Random.Range(0f, 10f);
                tempDest = Vector2.zero;
            }
        }
    }

    public bool RunTo(Vector2 destination)
    {
        if (transform.position.x < destination.x - 0.2f)
        {
            speedX = maxSpeed;
            if (SR.flipX) SR.flipX = false;
        }
        else if (transform.position.x > destination.x + 0.2f)
        {
            speedX = -maxSpeed;
            if (!SR.flipX) SR.flipX = true;
        }
        else
        {
            speedX = 0;
            return true;
        }
        return false;
    }

     public bool RunTo(Vector2 destination, float distance)
    {
        if (transform.position.x < destination.x - distance)
        {
            speedX = maxSpeed;
            if (SR.flipX) SR.flipX = false;
        }
        else if (transform.position.x > destination.x + distance)
        {
            speedX = -maxSpeed;
            if (!SR.flipX) SR.flipX = true;
        }
        else
        {
            speedX = 0;
            return true;
        }
        return false;
    }
}
