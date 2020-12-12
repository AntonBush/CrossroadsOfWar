using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuild : Building
{
    public Earthpony myHunter;
    public Transform hunterPosition1;
    public Transform hunterPosition2;
    public float radius;

    public Sprite[] levelSprites;
    public Flag flag;
    public MainFire mainFire;

    public GameObject myFakel;

    SpriteRenderer SR;

    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        if (buildingLevel >= 0) transform.localEulerAngles = Vector3.zero;
        if(buildingLevel == 1)
        {
            SR.sprite = levelSprites[1];
            radius = 25f;
            myFakel.SetActive(true);
        }
        if (buildingLevel == 2)
        {
            SR.sprite = levelSprites[2];
            radius = 36f;
            myFakel.SetActive(true);
        }
        if (buildingLevel == 3)
        {
            SR.sprite = levelSprites[3];
            radius = 50f;
            myFakel.SetActive(true);
        }
    }

    public Transform HunterPosition
    {
        get
        {
            if (buildingLevel == 1) return hunterPosition1;
            if (buildingLevel > 1) return hunterPosition2;
            return transform;
        }
    }

    void CheckHunter()
    {
        if (myHunter != null)
        {
            myHunter.work = null;
            myHunter.hasTower = myHunter.onTower = false;
            mainFire.workManager.camp.Hunters.Add(myHunter);
            myHunter.positionY = -0.83f;
            myHunter = null;
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
                CheckHunter();
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
                        radius = 25f;
                        resourses.AddResourses(-needWood, 0);
                        myFakel.SetActive(true);
                        buildingLevel++;
                        hintOff = false;
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
                CheckHunter();
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
                        resourses.AddResourses(-needWoodUpdateTwo, 0);
                        hintOff = false;
                        radius = 36f;
                        buildingLevel++;
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
                CheckHunter();
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
                        radius = 50f;
                        resourses.AddResourses(-needWoodUpdateThree, 0);
                        buildingLevel++;
                        onetime = false;
                    }
                }
            }
        }
    }
}
