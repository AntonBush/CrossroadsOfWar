using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmBuild : Building
{
    public Transform[] smokePositions;
    public GameObject Smoke;

    public Sprite coltForm;
    public Sprite mareForm;
    public Transform workerPosition;
    public Earthpony worker;

    public Sprite[] levelSprites;
    public Flag flag;
    public MainFire mainFire;
    public int foodPerTime;
    public float timePerFood;

    SpriteRenderer SR;

    IEnumerator newFood;


    public Sprite form
    {
        get
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
    }

    IEnumerator NewFood()
    {
        for (; ; )
        {
            if (resourses.Food + foodPerTime < resourses.FoodMax && worker != null)
            {
                resourses.AddResourses(0, foodPerTime);
            }
            yield return new WaitForSeconds(timePerFood);
        }
    }

    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        timePerFood = 15;
        if (buildingLevel >= 0)
        {
            transform.localEulerAngles = Vector3.zero;
        }
        if(buildingLevel == 1)
        {
            SR.sprite = levelSprites[1];
            if (newFood == null)
            {
                newFood = NewFood();
                StartCoroutine(newFood);
            }
        }
        if(buildingLevel == 2)
        {
            SR.sprite = levelSprites[2];
            if (newFood == null)
            {
                newFood = NewFood();
                StartCoroutine(newFood);
            }
            foodPerTime += 1;
            timePerFood = 8;
        }
        if (buildingLevel == 3)
        {
            SR.sprite = levelSprites[3];
            if (newFood == null)
            {
                newFood = NewFood();
                StartCoroutine(newFood);
            }
            foodPerTime += 2;
            timePerFood = 4;
        }
    }

    void CheckWorker()
    {
        if (worker != null && worker.sit)
        {
            if (!Smoke.activeSelf)
            {
                Smoke.transform.position = smokePositions[buildingLevel - 1].position;
                Smoke.SetActive(true);
            }
        }
        else
        {
            if (Smoke.activeSelf)
            {
                Smoke.SetActive(false);
            }
        }
    }


    private void Update()
    {
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
                        if (newFood == null)
                        {
                            newFood = NewFood();
                            StartCoroutine(newFood);
                        }
                        resourses.AddResourses(-needWood, 0);
                        buildingLevel++;
                        hintOff = false;
                        onetime = false;
                    }
                }
            }
        }
        if (buildingLevel == 1)
        {
            CheckWorker();

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
                        foodPerTime += 1;
                        timePerFood = 8;
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
            CheckWorker();

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
                        foodPerTime += 1;
                        timePerFood = 4;
                        resourses.AddResourses(-needWoodUpdateThree, 0);
                        buildingLevel++;
                        onetime = false;
                    }
                }
            }
        }
    }
}
