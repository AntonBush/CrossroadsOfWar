using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousesBuild : Building
{
    public EarthponiesCamp camp;
    public Sprite[] levelSprites;
    public Flag flag;
    public MainFire mainFire;

    SpriteRenderer SR;

    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        if(buildingLevel >= 0)
        {
            transform.localEulerAngles = Vector3.zero;
        }
        if(buildingLevel == 1)
        {
            SR.sprite = levelSprites[1];
            camp.enabled = true;
            resourses.PoniesMax += 4;
            resourses.UpdateResourses();
        }
        if(buildingLevel == 2)
        {
            SR.sprite = levelSprites[2];
            camp.enabled = true;
            resourses.PoniesMax += 8;
            resourses.UpdateResourses();
        }
        if(buildingLevel == 3)
        {
            SR.sprite = levelSprites[3];
            camp.enabled = true;
            resourses.PoniesMax += 16;
            resourses.UpdateResourses();
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
                        camp.enabled = true;
                        resourses.PoniesMax += 4;
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
            if (mainFire.buildingLevel > 1)
                CheckUpdateTwo();

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
                        resourses.PoniesMax += 4;
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
                        resourses.PoniesMax += 8;
                        resourses.AddResourses(-needWoodUpdateThree, 0);
                        buildingLevel++;
                        onetime = false;
                    }
                }
            }
        }
    }
}
