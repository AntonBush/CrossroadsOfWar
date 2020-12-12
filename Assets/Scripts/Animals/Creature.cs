using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{

    public GameObject DeadParticles;
    public Material GlowMat;
    public Material NormMat;
    public int health;
    [HideInInspector]
    public float speedX;
    [HideInInspector]
    public float speedY;

    public float positionY = -2f;
    [HideInInspector]
    public float redTime;
    public SpriteRenderer SR { get; private set; }
    public Animator anim { get; private set; }
    public Rigidbody2D rigbody { get; private set; }
    [HideInInspector]
    public float flyTime; //время, за которое юнит отлетает от удара
    [HideInInspector]
    public float deadTimer = 2f;
    [HideInInspector]
    public float stanned;

    public GameManager gameManager;

    [HideInInspector]
    public AudioSource _audi;

    public void InitiateCreature()
    {
        anim = GetComponent<Animator>();
        rigbody = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        _audi = GetComponent<AudioSource>();
    }

    public virtual void Dead()
    {
        if (deadTimer > 0)
        {
            anim.SetBool("dead", true);
            deadTimer -= Time.deltaTime;
        }
        else
        {
            GameObject deadParticles = PoolManager.getGameObjectFromPool(DeadParticles);
            deadParticles.transform.position = transform.position;
            deadParticles.GetComponent<DeadParticles>().gameManager = gameManager;
            deadParticles.GetComponent<DeadParticles>().timerDissapear = 1f;
            PoolManager.putGameObjectToPool(gameObject);
        }
    }

    public virtual void Hitted(int damage, bool leftSide)
    {
        health -= damage;
        flyTime = 0.4f;
        if (leftSide) speedX = 3f;
        else speedX = -3f;
        redTime = 0.2f;
        if (GetComponent<MovingController>())
        {
            GetComponent<MovingController>().SetAllAnims("hit");
        }
        else
        {
            if(anim.enabled)
            anim.SetTrigger("hit");
        }
        stanned = 0.6f;
    }

    public virtual bool isHitted
    {
        get
        {
            if (flyTime > 0.25f)
            {
                speedY = 6f;
            }
            else
            {
                speedY = -6f;
            }

            if (redTime > 0)
            {
                redTime -= Time.deltaTime;

                if (GetComponent<MovingController>())
                {
                    GetComponent<MovingController>().SetAllSRs(GlowMat, true);
                }
                else
                {
                    SR.material = GlowMat;
                    SR.color = Color.red;
                }
            }
            else
            {
                if (GetComponent<MovingController>())
                {
                    GetComponent<MovingController>().SetAllSRs(NormMat, false);
                }
                else
                {
                    SR.material = NormMat;
                    SR.color = Color.white;
                }
            }
            if (flyTime > 0)
            {
                flyTime -= 0.05f;
                return true;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, positionY, transform.position.z);
                flyTime = 0;
                speedX = speedY = 0f;
                return false;
            }
        }
    }

}
