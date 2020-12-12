using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornMovingController : MonoBehaviour
{
    Unicorn unicorn;
    SpriteRenderer SR;
    public SpriteRenderer[] partsOfBody;
    public SpriteRenderer HornMagicSR;
    public SpriteRenderer HornMagicItemSR;
    public Animator clothAnimator;
    public Animator BeardAnimator;
    public Animator EyesBlackAnimator;
    public Animator EyesGreyAnimator;
    public Animator HornAnimator;
    public Animator HornMagicAnimator;

    public List<Color> myOwnColors = new List<Color>();

    public float timerCheckColor;
    bool gotRed, onetime;

    public void ForgetColors()
    {
        timerCheckColor = 0.2f;
        myOwnColors.Clear();
        onetime = false;
    }

    void SetAllAnims(float _speed)
    {
        if (clothAnimator.enabled)
            clothAnimator.SetFloat("speed", _speed);
        if (BeardAnimator.gameObject.activeSelf)
        {
            BeardAnimator.SetFloat("speed", _speed);
        }
        EyesBlackAnimator.SetFloat("speed", _speed);
        EyesGreyAnimator.SetFloat("speed", _speed);
        HornAnimator.SetFloat("speed", _speed);
        if(HornMagicAnimator.gameObject.activeSelf)
        HornMagicAnimator.SetFloat("speed", _speed);
    }

    public void SetAllAnims(string trigger)
    {
        if (clothAnimator.enabled)
            clothAnimator.SetTrigger(trigger);
        if (BeardAnimator.gameObject.activeSelf)
        {
            BeardAnimator.SetTrigger(trigger);
        }
        EyesBlackAnimator.SetTrigger(trigger);
        EyesGreyAnimator.SetTrigger(trigger);
        HornAnimator.SetTrigger(trigger);
        if(HornMagicAnimator.gameObject.activeSelf)
        HornMagicAnimator.SetTrigger(trigger);
    }

    public void SetAllAnims(string name, bool boolean)
    {
        if (clothAnimator.enabled)
            clothAnimator.SetBool(name, boolean);
        if (BeardAnimator.gameObject.activeSelf)
        {
            BeardAnimator.SetBool(name, boolean);
        }
        EyesBlackAnimator.SetBool(name, boolean);
        EyesGreyAnimator.SetBool(name, boolean);
        HornAnimator.SetBool(name, boolean);
        if(HornMagicAnimator.gameObject.activeSelf)
        HornMagicAnimator.SetBool(name, boolean);
    }

    public void SetAllSRs(Material material, bool red)
    {
        SR.material = material;
        if (red)
        {
            SR.color = Color.red;
            gotRed = true;
        }
        else
        {
            if (gotRed && myOwnColors.Count > 0)
            {
                SR.color = myOwnColors[0];
            }
        }
        if (gotRed)
        {
            for (int i = 0; i < partsOfBody.Length; i++)
            {
                partsOfBody[i].material = material;
                if (red)
                {
                    partsOfBody[i].color = Color.red;
                }
                else
                {
                    if (gotRed && myOwnColors.Count > 0)
                    {
                        partsOfBody[i].color = myOwnColors[i + 1];
                    }
                }
            }
        }
        if (!red) gotRed = false;
    }

    public void SetAllSRs(int order)
    {
        if (partsOfBody[0].sortingOrder != order)
        {
            SR.sortingOrder = order;
            HornMagicSR.sortingOrder = order;
            HornMagicItemSR.sortingOrder = order;
            for (int i = 0; i < partsOfBody.Length; i++)
            {
                partsOfBody[i].sortingOrder = order;
            }
        }
    }


    void CheckColors()
    {
        if (!onetime)
        {
            if (timerCheckColor > 0)
            {
                timerCheckColor -= Time.deltaTime;
            }
            else
            {
                myOwnColors.Add(SR.color);
                for (int i = 0; i < partsOfBody.Length; i++)
                {
                    myOwnColors.Add(partsOfBody[i].color);
                }
                onetime = true;
            }
        }
    }

    void CheckSRs()
    {
        if (SR.flipX && !partsOfBody[0].flipX)
        {
            HornMagicSR.flipX = true;
            for (int i = 0; i < partsOfBody.Length; i++)
            {
                if (partsOfBody[i].gameObject.activeSelf)
                    partsOfBody[i].flipX = true;
            }
        }
        if (!SR.flipX && partsOfBody[0].flipX)
        {
            HornMagicSR.flipX = false;
            for (int i = 0; i < partsOfBody.Length; i++)
            {
                if (partsOfBody[i].gameObject.activeSelf)
                    partsOfBody[i].flipX = false;
            }
        }
        CheckInside();
    }

    void CheckInside()
    {
        if (unicorn.insideWarehouse)
        {
            SetAllSRs(0);
        }
        else
        {
            SetAllSRs(6);
        }
    }

    private void Start()
    {
        unicorn = GetComponent<Unicorn>();
        SR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!unicorn.insideWarehouse)
            SetAllAnims(Mathf.Abs(unicorn.speedX));


        HornMagicSR.enabled = unicorn.uniItem.gameObject.activeSelf;

        CheckColors();
        CheckSRs();


        if (unicorn.redTime > 0)
        {
            SetAllSRs(unicorn.GlowMat, true);
        }
        else
        {
            SetAllSRs(unicorn.NormMat, false);
        }

        if (unicorn.health <= 0) SetAllAnims("dead", true);
            
    }
}
