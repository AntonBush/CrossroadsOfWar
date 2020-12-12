using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Animal
{
    public Manticore Timberwolf1;
    public Manticore Timberwolf2;
    public RabbitSpawn myParent;
    public float timerRunningForLife;
    public AudioClip stepSound;

    Transform tempHunter;

    float stepTimer;

    void SoundStep()
    {
        _audi.volume = 0.45f * gameManager.soundVolume;
        if (Mathf.Abs(speedX) > 0)
        {
            if (stepTimer > 0)
            {
                stepTimer -= Time.deltaTime;
            }
            else
            {
                _audi.PlayOneShot(stepSound);
                stepTimer = 0.3f;
            }
        }
    }


    private void Start()
    {
        InitiateAnimal();
        minX = -178f;
        maxX = 19f;
    }

    void RunForYourLife()
    {
        if (transform.position.x > -145f && transform.position.x < 114f)
        {
            if (transform.position.x < tempHunter.position.x)
            {
                Running(-maxSpeed);
            }
            else
            {
                Running(maxSpeed);
            }
        }
        else speedX = 0;
    }

    bool SomeoneIsTryingToKillMe
    {
        get
        {
            if (Timberwolf1.health > 0 && Vector2.Distance(transform.position, Timberwolf1.transform.position) < 11f)
            {
                tempHunter = Timberwolf1.transform;
                return true;
            }
            if (Timberwolf2.health > 0 && Vector2.Distance(transform.position, Timberwolf2.transform.position) < 11f)
            {
                tempHunter = Timberwolf2.transform;
                return true;
            }
            return false;
        }
    }

    private void Update()
    {
        rigbody.velocity = new Vector2(speedX, speedY);
        anim.SetFloat("speed", Mathf.Abs(speedX));
        SoundStep();

        if (stanned > 0)
        {
            stanned -= Time.deltaTime;
        }

        if (!isHitted)
        {
            if (health > 0)
            {
                if (stanned < 0.1f)
                {
                    if (timerRunningForLife > 0)
                    {
                        RunForYourLife();
                        timerRunningForLife -= Time.deltaTime;
                    }
                    else
                    {
                        if (!SomeoneIsTryingToKillMe)
                        {
                            GoSomewhere();
                            tempHunter = null;
                        }
                        else
                        {
                            timerRunningForLife = 2.5f;
                        }
                    }
                }
                else
                {
                    speedX = 0;
                }

            }
            else
            {
                rigbody.velocity = Vector2.zero;
                Dead();
            }
        }

    }
}
