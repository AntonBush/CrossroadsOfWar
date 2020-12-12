using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseBuild : Building
{ //склад

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
            resourses.WoodMax += 40;
            resourses.FoodMax += 40;
            resourses.UpdateResourses();
        }
        if(buildingLevel == 2)
        {
            SR.sprite = levelSprites[2];
            myFakel.SetActive(true);
            resourses.WoodMax += 60;
            resourses.FoodMax += 60;
            resourses.UpdateResourses();
        }
        if (buildingLevel == 3)
        {
            SR.sprite = levelSprites[3];
            myFakel.SetActive(true);
            resourses.WoodMax += 75;
            resourses.FoodMax += 75;
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
            if (mainFire.buildingLevel > 0 && !startBuilding)
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
						resourses.WoodMax += 40;
						resourses.FoodMax += 40;
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
                        myFakel.SetActive(true);
                        resourses.WoodMax += 20;
						resourses.FoodMax += 20;
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
                        resourses.WoodMax += 15;
						resourses.FoodMax += 15;
                        resourses.AddResourses(-needWoodUpdateThree, 0);
                        buildingLevel++;
                        onetime = false;
                    }
                }
            }
        }
    }
}
