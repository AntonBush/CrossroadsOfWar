using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBuilding : Building
{
    [Header("Bows making")]

    public float maxTime;
    public int bowsCount;
    public int needWoodForBow;
    int newBow;
    float bowTimer;
    [HideInInspector]
    public int bowsMax;

    public GameObject BowHint;
    public SpriteRenderer[] BowIcons;
    public Sprite Bow;
    public Sprite Waiting;

    [Header("Workers")]
    public Sprite coltForm;
    public Sprite mareForm;
    public Transform workerPosition;
    public Earthpony worker;

    public Sprite[] levelSprites;
    public Flag flag;
    public MainFire mainFire;
    SpriteRenderer SR;

    int bowiconI;

    [HideInInspector]
    public bool playerHasBow;

    public Sprite form
    {
        get
        {
            if (worker != null)
            {
                if (worker.GetComponent<RandomSex>().playerMare)
                {
                    return mareForm;
                }
                else
                {
                    return coltForm;
                }
            }
            else 
            {
                Debug.Log("работника в кузне типа нет, я хз");
                return mareForm;
            }
        }
    }

    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        if (buildingLevel >= 0) transform.localEulerAngles = Vector3.zero;
        if(buildingLevel == 1)
        {
            SR.sprite = levelSprites[1];
            bowsMax = 1;
        }
        if (buildingLevel == 2)
        {
            SR.sprite = levelSprites[2];
            bowsMax = 3;
        }
        if (buildingLevel == 3)
        {
            SR.sprite = levelSprites[3];
            bowsMax = 6;
        }
    }

    void UpdateBowIcons()
    {
        if (bowiconI < BowIcons.Length - 1) bowiconI++;
        else bowiconI = 0;

        if (newBow > bowiconI)
        {
            BowIcons[bowiconI].sprite = Waiting;
            BowIcons[bowiconI].gameObject.SetActive(true);
        }
        else
        {
            if (bowsCount <= bowiconI)
                BowIcons[bowiconI].gameObject.SetActive(false);
        }

        if (bowsCount > bowiconI)
        {
            BowIcons[bowiconI].sprite = Bow;
            BowIcons[bowiconI].gameObject.SetActive(true);
        }
    }

    void CheckBowing()
    {
        UpdateBowIcons();

        if (worker != null && worker.sit)
        {
            if (Vector2.Distance(Player.transform.position, transform.position) < 4f &&
           Player.GetComponent<MovingController>().health > 0 && resourses.Wood > needWoodForBow && newBow + bowsCount < bowsMax)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    resourses.AddResourses(-needWoodForBow, 0);
                    BowHint.SetActive(false);
                    if (bowTimer == 0)
                        bowTimer = (maxTime / buildingLevel);
                    newBow++;
                }
                else
                {
                    BowHint.SetActive(true);
                }
            }
            else BowHint.SetActive(false);


            if (newBow > 0)
            {
                if (bowTimer > 0)
                {
                    bowTimer -= Time.deltaTime;
                }
                else
                {
                    bowsCount++;
                    newBow--;
                    bowTimer = (maxTime / buildingLevel);
                }
            }
        }
        else
        {
            BowHint.SetActive(false);
        }
    }

    void CheckPlayerBowing()
    {
        if (buildingLevel > 0 && !playerHasBow)
        {
            MovingController mov = Player.GetComponent<MovingController>();
            if (!mov.hasBow)
            {
                if (bowsCount > 0)
                {
                    if (Vector2.Distance(transform.position, Player.position) < 2f)
                    {
                        bowsCount--;
                        playerHasBow = true;
                        mov.hasBow = true;
                    }
                }
            }
        }
    }

    private void Update()
    {
        CheckBowing();
        CheckPlayerBowing();

        if (buildingLevel == -1)
        {
            if (flag.enabled == false)
            {
                if (Up())
                {
                    buildingLevel = 0;
                }
            }
        }
        if (buildingLevel == 0)
        {
            if (mainFire.buildingLevel > 0)
            {
                CheckBuild();
            }

            if (startBuilding)
            {
                if (Down())
                {
                    SR.sprite = levelSprites[buildingLevel + 1];
                    onetime = true;
                    startBuilding = false;
                }
            }
            else
            {
                if (Up())
                {
                    if (onetime)
                    {
                        hintOff = false;
                        resourses.AddResourses(-needWood, 0);
                        bowsMax = 1;
                        buildingLevel++;
                        onetime = false;
                    }
                }
            }

        }
        if (buildingLevel == 1)
        {
            if (mainFire.buildingLevel > 1)
                CheckUpdateTwo();

            if (startBuilding)
            {
                if (worker != null)
                {
                    worker.work = null;
                    mainFire.workManager.camp.Ponies.Add(worker);
                    mainFire.workManager.camp.PoniesWalk.Add(true);
                    worker = null;
                }
                if (Down())
                {
                    bowsMax = 3;
                    SR.sprite = levelSprites[buildingLevel + 1];
                    onetime = true;
                    startBuilding = false;
                }
            }
            else
            {
                if (Up())
                {
                    if (onetime)
                    {
                        resourses.AddResourses(-needWoodUpdateTwo, 0);
                        buildingLevel++;
                        hintOff = false;
                        onetime = false;
                    }
                }
            }
        }
        if (buildingLevel == 2)
        {
            if (mainFire.buildingLevel > 2)
                CheckUpdateThree();

            if (startBuilding)
            {
                if (worker != null)
                {
                    worker.work = null;
                    mainFire.workManager.camp.Ponies.Add(worker);
                    mainFire.workManager.camp.PoniesWalk.Add(true);
                    worker = null;
                }
                if (Down())
                {
                    SR.sprite = levelSprites[buildingLevel + 1];
                    onetime = true;
                    startBuilding = false;
                }
            }
            else
            {
                if (Up())
                {
                    if (onetime)
                    {
                        bowsMax = 6;
                        resourses.AddResourses(-needWoodUpdateThree, 0);
                        buildingLevel++;
                        onetime = false;
                    }
                }
            }
        }
    }
}
