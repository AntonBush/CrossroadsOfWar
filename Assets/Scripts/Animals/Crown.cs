using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crown : MonoBehaviour
{
    public bool mustBeOff;

    public Vector2 farAway;
    public Transform Camera;
    public List<TreeBuild> LandingPositions = new List<TreeBuild>();
    public int tempTreeNum { get; private set; }
    Vector2 landingPlace;
    bool flying = true;

    float speed, speedY;

    public float timerFlying, timerLanding;

    int randomDistance;

    float timerHeadRotate;
    bool headTwo;


    Rigidbody2D rigbody;
    Animator anim;
    SpriteRenderer SR;

    bool oneside;

    public float ShakeNumber(float number)
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

    void Flying()
    {
        if (SR.flipX)
        {
            if (transform.position.x > Camera.position.x - randomDistance)
            {
                speed = -4.5f;
            }
            else
            {
                SR.flipX = false;
                randomDistance = Random.Range(12, 50);
            }
        }
        else
        {
            if (transform.position.x < Camera.position.x + randomDistance)
            {
                speed = 4.5f;
            }
            else
            {
                SR.flipX = true;
                randomDistance = Random.Range(12, 50);
            }
        }

        if (transform.position.y < 4.5f) speedY = 2f;
        else
        {
            speedY = ShakeNumber(speedY);
        }
    }

    Vector2 notNormDir;
    Vector2 normDir;

    public bool FlyTo(Vector2 destination, float maxSpeedX, float maxSpeedY, float distance)
    {
        if (maxSpeedY == 0)
        {
            if (transform.position.x > destination.x + distance)
            {
                speed = -maxSpeedX;
                if (!SR.flipX) SR.flipX = true;
            }
            else if (transform.position.x < destination.x - distance)
            {
                speed = maxSpeedX;
                if (SR.flipX) SR.flipX = false;
            }
            else
            {
                speed = 0;
                return true;
            }
            if (transform.position.y < destination.y - 1f)
            {
                speedY = maxSpeedY;
            }
            else if (transform.position.y > destination.y + 1f)
            {
                speedY = -maxSpeedY;
            }
            else
            {
                speedY = ShakeNumber(speedY);
            }
        }
        else
        {
            if (normDir == Vector2.zero)
            {
                notNormDir = destination - (Vector2)transform.position;
                notNormDir = new Vector2(Mathf.Abs(notNormDir.x), Mathf.Abs(notNormDir.y));
                normDir = (Vector2)Vector3.Normalize(notNormDir);
            }
            if (transform.position.x > destination.x + distance)
            {
                speed = -maxSpeedX * normDir.x;
                if (!SR.flipX) SR.flipX = true;
            }
            else if (transform.position.x < destination.x - distance)
            {
                speed = maxSpeedX * normDir.x;
                if (SR.flipX) SR.flipX = false;
            }
            else
            {
                speed = 0;
                return true;
            }
            if (destination.y > transform.position.y + 0.15f)
            {
                speedY = maxSpeedY * normDir.y;
            }
            else if (destination.y < transform.position.y - 0.15f)
            {
                speedY = -maxSpeedY * normDir.y;
            }
            else
            {
                speedY = 0;
            }
        }
        return false;
    }

    void ChooseTree()
    {
        if (landingPlace == Vector2.zero && LandingPositions.Count > 0)
        {
            int tempTreeNum = 0;
            float minDistance = Vector2.Distance(transform.position, LandingPositions[0].transform.position);
            for (int i = 1; i < LandingPositions.Count; i++)
            {
                float tempDitance = Vector2.Distance(transform.position, LandingPositions[i].transform.position);
                if (tempDitance < minDistance)
                {
                    minDistance = tempDitance;
                    tempTreeNum = i;
                }
            }

            int tempNum = Random.Range(0, LandingPositions[tempTreeNum].myLandingPositions.Length);
            landingPlace = LandingPositions[tempTreeNum].myLandingPositions[tempNum].transform.position;
        }
    }

    void Landing()
    {
        if (timerLanding > 0)
        {
            ChooseTree();

            if(FlyTo(landingPlace, 4f, 0f, 9f))
            if (FlyTo(landingPlace, 2.5f, 2.5f, 0.15f))
            {
                if (timerLanding > 0)
                {
                    timerLanding -= Time.deltaTime;
                    speed = speedY = 0;
                    if (timerHeadRotate > 0)
                    {
                        timerHeadRotate -= Time.deltaTime;
                    }
                    else
                    {
                        headTwo = !headTwo;
                        timerHeadRotate = Random.Range(2f, 15f);
                    }
                }
            }
        }
        else
        {
            normDir = Vector2.zero;
            landingPlace = Vector2.zero;
            flying = true;
            timerFlying = Random.Range(15f, 25f);
        }
    }

    private void Start()
    {
        rigbody = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        timerFlying = Random.Range(0, 25f);
        randomDistance = Random.Range(12, 50);

        if (Random.value > 0.5f)
        {
            SR.flipX = true;
        }
        else
        {
            SR.flipX = false;
        }
    }

    private void Update()
    {
        anim.SetFloat("spd", System.Math.Abs(speed));
        anim.SetBool("HeadTwo", headTwo);
        rigbody.velocity = new Vector2(speed, speedY);
        

        if (!mustBeOff)
        {
            if (flying)
            {
                if (timerFlying > 0)
                { //птица летает определенное время
                    Flying();
                    timerFlying -= Time.deltaTime;
                }
                else
                { //и начинает приземляться
                    timerLanding = Random.Range(15f, 60f);
                    flying = false;
                }
            }
            else
            { //тут она будет приземляться на дерево
                Landing();
            }
        }
        else
        {

            if (FlyTo(farAway, 4f, 3f, 1f))
            {
                mustBeOff = false;
                gameObject.SetActive(false);
            }
        }
    }
}
