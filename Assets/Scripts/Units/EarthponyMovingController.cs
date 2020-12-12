using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthponyMovingController : MonoBehaviour
{
    Earthpony earthpony;
    SpriteRenderer SR;
    public SpriteRenderer[] partsOfBody;
    public Animator clothAnimator;
    public Animator BeardAnimator;
    public Animator BowAnimator;
    public Animator EyesBlackAnimator;
    public Animator EyesGreyAnimator;
    public Animator HairAnimator;
    public Animator HoodAnimator;

    public List<Color> myOwnColors = new List<Color>();

    public bool shoot;
    float timerCheckColor;
    bool gotRed, onetime;

    public void MakeColorsNotHired()
    {
        partsOfBody[0].color = partsOfBody[5].color = new Color(0.5f,0.5f,0.5f);
        ForgetColors();
    }

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
        if (BowAnimator.gameObject.activeSelf)
        {
            BowAnimator.SetFloat("speed", _speed);
        }
        EyesBlackAnimator.SetFloat("speed", _speed);
        EyesGreyAnimator.SetFloat("speed", _speed);
        HairAnimator.SetFloat("speed", _speed);
        if (HoodAnimator.gameObject.activeSelf)
            HoodAnimator.SetFloat("speed", _speed);
    }

    public void SetAllAnims(string trigger)
    {
        if (clothAnimator.enabled)
            clothAnimator.SetTrigger(trigger);
        if (BeardAnimator.gameObject.activeSelf)
        {
            BeardAnimator.SetTrigger(trigger);
        }
        if (BowAnimator.gameObject.activeSelf)
        {
            BowAnimator.SetTrigger(trigger);
        }
        EyesBlackAnimator.SetTrigger(trigger);
        EyesGreyAnimator.SetTrigger(trigger);
        HairAnimator.SetTrigger(trigger);
        if (HoodAnimator.gameObject.activeSelf)
            HoodAnimator.SetTrigger(trigger);
    }

    public void SetAllAnims(string name, bool boolean)
    {
        if (clothAnimator.enabled)
            clothAnimator.SetBool(name, boolean);
        if (BeardAnimator.gameObject.activeSelf)
        {
            BeardAnimator.SetBool(name, boolean);
        }
        if (BowAnimator.gameObject.activeSelf)
        {
            BowAnimator.SetBool(name, boolean);
        }
        EyesBlackAnimator.SetBool(name, boolean);
        EyesGreyAnimator.SetBool(name, boolean);
        HairAnimator.SetBool(name, boolean);
        if (HoodAnimator.gameObject.activeSelf)
            HoodAnimator.SetBool(name, boolean);
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

    void SetAllSRorder(int order)
    {
        SR.sortingOrder = order;
        for (int i = 0; i < partsOfBody.Length; i++)
        {
            partsOfBody[i].sortingOrder = order;
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
                myOwnColors[3] = myOwnColors[7] = myOwnColors[8] = Color.white;
                gotRed = true;
                onetime = true;
            }
        }
    }

    void CheckSRs()
    {
        if (SR.flipX && (!partsOfBody[0].flipX || !partsOfBody[5].flipX))
        {
            for (int i = 0; i < partsOfBody.Length; i++)
            {
                if (partsOfBody[i].gameObject.activeSelf)
                    partsOfBody[i].flipX = true;
            }
        }
        if (!SR.flipX && (partsOfBody[0].flipX || partsOfBody[5].flipX))
        {
            for (int i = 0; i < partsOfBody.Length; i++)
            {
                if (partsOfBody[i].gameObject.activeSelf)
                    partsOfBody[i].flipX = false;
            }
        }
    }

    void CheckShooting()
    {
        if(shoot)
        {
            SetAllAnims("shoot");
            shoot = false;
        }
    }

    bool HasWork() //смена одежды и всего остального для рабочего пня
    {
        if (earthpony.work == null)
        {
            SetAllSRorder(earthpony.myOrder);
            if (!clothAnimator.enabled)
            {
                HoodAnimator.gameObject.SetActive(true);
                clothAnimator.enabled = true;
                gotRed = true;
            }
            return false;
        }
        else if (earthpony.work.gameObject.name == "WeaponBuilding" &&
        Vector2.Distance(earthpony.transform.position, earthpony.work.transform.position) < 2.5f && earthpony.sit && !earthpony.hunter)
        {
            
            SetAllSRorder(4);
            HoodAnimator.gameObject.SetActive(false);
            clothAnimator.enabled = false;
            SpriteRenderer clothSR = clothAnimator.GetComponent<SpriteRenderer>();
            WeaponBuilding weaponWork = earthpony.work.GetComponent<WeaponBuilding>();
            if (weaponWork.worker != earthpony)
            {
                print("эта херня опять произошла - работник вылетел из кузни");
                weaponWork.worker = earthpony;
                return false;
            }
            clothSR.sprite = weaponWork.form;
            clothSR.color = Color.white;
            return true;
        }
        else if (earthpony.work.gameObject.name == "Farm" &&
        Vector2.Distance(earthpony.transform.position, earthpony.work.transform.position) < 3f && earthpony.sit)
        {
            SetAllSRorder(4);
            HoodAnimator.gameObject.SetActive(false);
            clothAnimator.enabled = false;
            SpriteRenderer clothSR = clothAnimator.GetComponent<SpriteRenderer>();
            clothSR.sprite = earthpony.work.GetComponent<FarmBuild>().form;
            clothSR.color = Color.white;
            return true;
        }
        else
        {
            SetAllSRorder(earthpony.myOrder);
            HoodAnimator.gameObject.SetActive(true);
            clothAnimator.enabled = true;
            return false;
        }
    }

    private void Start()
    {
        earthpony = GetComponent<Earthpony>();
        SR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        SetAllAnims(Mathf.Abs(earthpony.speedX));

        CheckColors();
        CheckSRs();
        CheckShooting();

        if (earthpony.health > 0)
        {
            if (earthpony.sit) SetAllAnims("sit", true);
            else SetAllAnims("sit", false);

            if (earthpony.axing) SetAllAnims("axe", true);
            else SetAllAnims("axe", false);

            if (earthpony.guitaring) SetAllAnims("guitar", true);
            else SetAllAnims("guitar", false);

            //if(earthpony.onTower) SetAllSRs(4);

            if (!HasWork())
            {
                if (earthpony.redTime > 0)
                {
                    SetAllSRs(earthpony.GlowMat, true);
                }
                else
                {
                    SetAllSRs(earthpony.NormMat, false);
                }
            }

        }
        else
            SetAllAnims("dead", true);
    }
}
