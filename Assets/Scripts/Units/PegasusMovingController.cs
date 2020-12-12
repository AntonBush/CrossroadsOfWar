using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegasusMovingController : MonoBehaviour
{

    Pegasus pegasus;

    SpriteRenderer SR;
    public SpriteRenderer[] partsOfBody;
    public Animator clothAnimator;
    public Animator HairAnimator;
    public Animator BeardAnimator;
    public Animator EyesBlackAnimator;
    public Animator EyesGreyAnimator;
    public Animator WingsAnimator;
    public Animator FoodAnimator;

    public List<Color> myOwnColors = new List<Color>();

    public float timerCheckColor;
    bool gotRed, onetime;

    public void ForgetColors()
    {
        timerCheckColor = 0.6f;
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
		if(FoodAnimator.gameObject.activeSelf)
		{
			FoodAnimator.SetFloat("speed", _speed);
		}
        HairAnimator.SetFloat("speed", _speed);
        EyesBlackAnimator.SetFloat("speed", _speed);
        EyesGreyAnimator.SetFloat("speed", _speed);
        WingsAnimator.SetFloat("speed", _speed);
    }

    public void SetAllAnims(string trigger)
    {
        if (clothAnimator.enabled)
            clothAnimator.SetTrigger(trigger);
        if (BeardAnimator.gameObject.activeSelf)
        {
            BeardAnimator.SetTrigger(trigger);
        }
		if(FoodAnimator.gameObject.activeSelf)
		{
			FoodAnimator.SetTrigger(trigger);
		}
        HairAnimator.SetTrigger(trigger);
        EyesBlackAnimator.SetTrigger(trigger);
        EyesGreyAnimator.SetTrigger(trigger);
        WingsAnimator.SetTrigger(trigger);
    }

    public void SetAllAnims(string name, bool boolean)
    {
        if (clothAnimator.enabled)
            clothAnimator.SetBool(name, boolean);
        if (BeardAnimator.gameObject.activeSelf)
        {
            BeardAnimator.SetBool(name, boolean);
        }
		if(FoodAnimator.gameObject.activeSelf)
		{
			FoodAnimator.SetBool(name, boolean);
		}
        HairAnimator.SetBool(name, boolean);
        EyesBlackAnimator.SetBool(name, boolean);
        EyesGreyAnimator.SetBool(name, boolean);
        WingsAnimator.SetBool(name, boolean);
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
            if (gotRed)
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
                    if (gotRed)
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
            for (int i = 0; i < partsOfBody.Length; i++)
            {
                if (partsOfBody[i].gameObject.activeSelf)
                    partsOfBody[i].flipX = true;
            }
        }
        if (!SR.flipX && partsOfBody[0].flipX)
        {
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
        if (pegasus.insideWarehouse)
        {
            SetAllSRs(0);
        }
        else
        {
            SetAllSRs(6);
        }
    }

    void CheckFood()
    {
        if (pegasus.foodCount > 0)
        {
            FoodAnimator.gameObject.SetActive(true);
        }
        else
        {
			FoodAnimator.gameObject.SetActive(false);
        }
    }


    private void Start()
    {
        pegasus = GetComponent<Pegasus>();
        SR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        SetAllAnims(Mathf.Abs(pegasus.speedX));
		SetAllAnims("pegasus",true);

		CheckFood();
        CheckColors();
        CheckSRs();

        if (pegasus.redTime > 0)
        {
            SetAllSRs(pegasus.GlowMat, true);
        }
        else
        {
            SetAllSRs(pegasus.NormMat, false);
        }

        if (pegasus.health <= 0) SetAllAnims("dead", true);
    }
}
