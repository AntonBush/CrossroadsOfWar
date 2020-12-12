using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    public GameManager gameManager;
    public RuntimeAnimatorController myAnim;
    public DuckSpawn myParent;
    public Vector2 startPosition;
    public Vector2 endPosition;

	public float maxSpeed;

    Animator anim;

    Rigidbody2D rigbody;
    SpriteRenderer SR;

    AudioSource _audi;

    float speed, speedY;

    bool oneside;

    float ShakeNumber(float number)
    {
        if (oneside)
        {
            if (number < 0.6f)
            {
                number += Time.deltaTime * 5f;
            }
            else
            {
                oneside = false;
            }
        }
        else
        {
            if (number > -0.6f)
            {
                number -= Time.deltaTime * 5f;
            }
            else
            {
                oneside = true;
            }
        }
        return number;
    }


    void Fly()
    {
        if (transform.localPosition.x < endPosition.x - 0.05f)
        {
            SR.flipX = true;
            speed = maxSpeed;
        }
        else if (transform.localPosition.x > endPosition.x + 0.05f)
        {
            SR.flipX = false;
            speed = -maxSpeed;
        }
        else
        {
            myParent.tempDuckCount--;
            Destroy(gameObject);
        }
        speedY = ShakeNumber(speedY);

    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigbody = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        _audi = GetComponent<AudioSource>();
        transform.localPosition = startPosition;
        if(myAnim != null) 
        {
            anim.runtimeAnimatorController = myAnim;
        }
    }

    private void Update()
    {
        rigbody.velocity = new Vector2(speed, speedY);
        _audi.volume = gameManager.soundVolume;

        Fly();
    }
}
