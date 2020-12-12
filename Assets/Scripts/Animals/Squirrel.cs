using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squirrel : Animal
{
    public string Status;
    public SquirrelSpawn myParent;
    public Transform player;
    public Transform tree; //белка бежит к середине дерева - примерно - затем поднимается на рандомное расстояние наверх и исчезает там
    public Transform newtree;
    public AudioClip stepSound;

    float walkingTimer;
    bool landed;

    bool onetime;

    int runRandomSide;

    float stepTimer;

    void SoundStep()
    {
        if (_audi == null) _audi = GetComponent<AudioSource>();
        if (gameManager == null) gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _audi.volume = 0.35f * gameManager.soundVolume;
        if (Mathf.Abs(speedX) > 0)
        {
            if (stepTimer > 0)
            {
                stepTimer -= Time.deltaTime;
            }
            else
            {
                _audi.PlayOneShot(stepSound);
                stepTimer = 0.5f;
            }
        }
    }


    private void Start()
    {
        landed = false;
        InitiateAnimal();
        minX = -170f;
        maxX = 19f;
        walkingTimer = Random.Range(0, 24f);
    }

    void GoToTheLand()
    {
        if (transform.position.y > -2.25f)
        {
            transform.localEulerAngles = new Vector3(0, 0, 90f);
            speedY = -3f;
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
            speedY = 0;
            landed = true;
        }
    }

    void GoToTheTree()
    {
        if (RunTo(new Vector2(newtree.position.x, newtree.position.y), 1f))
        {
            speedX = 0;
            if (transform.localPosition.y < -0.45f)
            {
                if (SR.flipX)
                    transform.localEulerAngles = new Vector3(0, 0, -90f);
                else
                    transform.localEulerAngles = new Vector3(0, 0, 90f);
                speedY = +3f;
            }
            else
            {
                myParent.Squirrels.Remove(this);
                PoolManager.putGameObjectToPool(gameObject);
            }
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
            speedY = 0;
        }
    }

    void GoOut()
    {
        speedY = 0;
        transform.localEulerAngles = new Vector3(0, 0, 0);
        if (runRandomSide == 0)
            if (Random.value > 0.5f) runRandomSide = -1;
            else runRandomSide = 1;

        if (runRandomSide == -1)
        {
            if (transform.position.x > minX)
            {
                Running(-maxSpeed);
            }
            else
            {
                runRandomSide = 1;
            }
        }
        if (runRandomSide == 1)
        {
            if (transform.position.x < maxX)
            {
                Running(maxSpeed);
            }
            else
            {
                runRandomSide = -1;
            }
        }

        if (Vector2.Distance(transform.position, player.position) > 10f)
        {
            myParent.Squirrels.Remove(this);
            PoolManager.putGameObjectToPool(gameObject);
        }
    }

    private void Update()
    {
        rigbody.velocity = new Vector2(speedX, speedY);
        anim.SetFloat("speed", Mathf.Abs(speedX + speedY));
        SoundStep();

        if (!landed)
        {
            GoToTheLand();
            Status = "landing to land";
        }
        else
        {
            if (walkingTimer > 0)
            {
                walkingTimer -= Time.deltaTime;
                GoSomewhere();
                Status = "walking";
            }
            else
            {
                if (newtree != null && newtree.gameObject.activeSelf)
                {
                    GoToTheTree();
                    Status = "landing to tree";
                }
                else
                {
                    if (!onetime)
                    {
                        landed = false;
                        onetime = true;
                    }
                    GoOut();
                    Status = "going out";
                }
            }
        }
    }
}
