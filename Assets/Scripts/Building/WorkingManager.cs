using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkingManager : MonoBehaviour
{
    public int tempUnitID; //нужен затем, чтобы исправить баг с одновременным наниманием нескольких пней
    public float cooldown;
    public EarthponiesCamp camp;
    public WeaponBuilding weaponBuilding;
    public FarmBuild farmBuilding;
    public TowerBuild LeftTower;
    public TowerBuild RightTower;
    public List<TreeBuild> TreesToAxe = new List<TreeBuild>();

    [HideInInspector]
    public int treeI, ponyI;
    int hunterI;

    private void Start()
    {
        tempUnitID = -1;
    }

    private void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        if (camp.Ponies.Count > 0)
        {
            if (weaponBuilding.buildingLevel > 0 && weaponBuilding.worker == null && !weaponBuilding.startBuilding && !weaponBuilding.onetime)
            {
                if (ponyI < camp.Ponies.Count && camp.Ponies[ponyI].work == null)
                {
                    camp.Ponies[ponyI].work = weaponBuilding;
                    weaponBuilding.worker = camp.Ponies[ponyI];
                    camp.Ponies.RemoveAt(ponyI);
                    camp.PoniesWalk.RemoveAt(ponyI);
                    return;
                }
            }

            if (farmBuilding.buildingLevel > 0 && farmBuilding.worker == null && !farmBuilding.startBuilding && !farmBuilding.onetime)
            { 
                if (ponyI < camp.Ponies.Count && camp.Ponies[ponyI].work == null)
                {
                    camp.Ponies[ponyI].work = farmBuilding;
                    farmBuilding.worker = camp.Ponies[ponyI];
                    camp.Ponies.RemoveAt(ponyI);
                    camp.PoniesWalk.RemoveAt(ponyI);
                    return;
                }
            }

            if (weaponBuilding.buildingLevel > 0 && weaponBuilding.bowsCount > 0 && weaponBuilding.playerHasBow)
            {
                weaponBuilding.bowsCount--;
                camp.Ponies[ponyI].work = weaponBuilding;
                camp.Ponies[ponyI].hunter = true;
                camp.Ponies[ponyI].gameObject.SetActive(true);
                camp.Hunters.Add(camp.Ponies[ponyI]);
                camp.Ponies.RemoveAt(ponyI);
                camp.PoniesWalk.RemoveAt(ponyI);
                ponyI = 0;
                return;
            }

            if (TreesToAxe.Count > 0)
            {
                if (treeI < TreesToAxe.Count - 1) treeI++;
                else
                {
                    treeI = 0;
                    if (ponyI < camp.Ponies.Count - 1) ponyI++; //счетчик пней, который раньше был там
                    else ponyI = 0;
                }

                if (ponyI < camp.Ponies.Count && treeI < TreesToAxe.Count && camp.Ponies[ponyI].work == null && TreesToAxe[treeI].Worker == null)
                {
                    camp.Ponies[ponyI].work = TreesToAxe[treeI];
                    TreesToAxe[treeI].Worker = camp.Ponies[ponyI];
                }
            }
            else ponyI = 0;
        }
        if (camp.Hunters.Count > 0)
        {
            if (LeftTower.buildingLevel > 0 && LeftTower.myHunter == null && !LeftTower.startBuilding && !LeftTower.onetime && hunterI < camp.Hunters.Count)
            {
                if (camp.Hunters[hunterI].hasBow && !camp.Hunters[hunterI].hasTower)
                {
                    camp.Hunters[hunterI].hasTower = true;
                    camp.Hunters[hunterI].work = LeftTower;
                    LeftTower.myHunter = camp.Hunters[hunterI];
                    camp.Hunters.Remove(camp.Hunters[hunterI]);
                    hunterI = 0;
                    return;
                }
            }

            if (RightTower.buildingLevel > 0 && RightTower.myHunter == null && !RightTower.startBuilding && !RightTower.onetime && hunterI < camp.Hunters.Count)
            {
                if (camp.Hunters[hunterI].hasBow && !camp.Hunters[hunterI].hasTower)
                {
                    camp.Hunters[hunterI].hasTower = true;
                    camp.Hunters[hunterI].work = RightTower;
                    RightTower.myHunter = camp.Hunters[hunterI];
                    camp.Hunters.Remove(camp.Hunters[hunterI]);
                }
            }

            if (TreesToAxe.Count > 0)
            {
                if (treeI < TreesToAxe.Count - 1) treeI++;
                else
                {
                    treeI = 0;
                    if (hunterI < camp.Hunters.Count - 1) hunterI++;
                    else hunterI = 0;
                }

                if (hunterI < camp.Hunters.Count && treeI < TreesToAxe.Count)
                {
                    if (camp.Hunters[hunterI].work == null && TreesToAxe[treeI].Worker == null)
                    {
                        camp.Hunters[hunterI].work = TreesToAxe[treeI];
                        TreesToAxe[treeI].Worker = camp.Hunters[hunterI];
                    }
                }
            }
            else hunterI = 0;
        }
    }
}
