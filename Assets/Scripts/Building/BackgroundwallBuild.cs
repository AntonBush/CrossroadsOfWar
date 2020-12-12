using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundwallBuild : Building
{

    public MainFire fire;

    public SpriteRenderer[] SRs;

    public Sprite Level3Sprite;

    private void Start()
    {
        if(fire.buildingLevel == 2)
        {
            transform.localEulerAngles = Vector3.zero;
        }
        if (fire.buildingLevel == 3)
        {
            transform.localEulerAngles = Vector3.zero;
            for (int i = 0; i < SRs.Length; i++)
            {
                SRs[i].sprite = Level3Sprite;
            }
        }
    }

    private void Update()
    {
        if (buildingLevel == 0)
        {
            if (fire.buildingLevel == 2)
            {
                if (Up()) buildingLevel++;
            }
        }
        if (buildingLevel == 1)
        {
            if (fire.startBuilding)
            {
                startBuilding = true;
            }
            if (startBuilding)
            {
                if (Down())
                {
                    for (int i = 0; i < SRs.Length; i++)
                    {
                        SRs[i].sprite = Level3Sprite;
                    }
                    buildingLevel++;
					startBuilding = false;
                }
            }
        }
        if (buildingLevel == 2)
        {
            if (Up()) enabled = false;
        }
    }
}
