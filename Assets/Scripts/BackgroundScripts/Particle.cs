using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public GameManager gameManager;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public float movingPosition;
    public float speed;

    public bool needToBeDestroyed;
    public Sprite mySprite;
    public ParticlesMove myParent;

    SpriteRenderer SR;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        SR = GetComponent<SpriteRenderer>();
        if (mySprite != null)
            SR.sprite = mySprite;
    }

    void DestroyParticle()
    {
        if (myParent != null)
            myParent.particles.Remove(this);
        PoolManager.putGameObjectToPool(this.gameObject);
    }

    void Update()
    {
        if (needToBeDestroyed)
        {
            if (SR.color.a > 0)
            {
                SR.color = new Color(1f, 1f, 1f, SR.color.a - Time.deltaTime / 10f);
            }
            else
            {
                DestroyParticle();
            }
        }


        if (transform.localPosition.x != endPosition.x)
        {
            if (!needToBeDestroyed)
            {
                if (Vector2.Distance(transform.localPosition, endPosition) < 5f)
                {
                    SR.color = new Color(1f, 1f, 1f, SR.color.a - Time.deltaTime / 4f);
                }
            }
            transform.localPosition = Vector2.Lerp(startPosition, endPosition, movingPosition);
            if (!gameManager.GamePaused)
            {
                if (movingPosition < 1)
                {
                    movingPosition += speed / 1000;
                }
                else movingPosition = 1;
            }
        }
        else
        {
            DestroyParticle();
        }
    }
}
