using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegasusSpawn : MonoBehaviour
{
    [Header("Заодно спавнит урсу")]

    public List<Pegasus> pegasus = new List<Pegasus>();

    [SerializeField]
    GameManager gameManager;

    public Crown crown;
    public TowerBuild LeftTower;
    public TowerBuild RightTower;
    public WarehouseBuild warehouseBuild;
    public EarthponiesCamp leftCamp;
    public EarthponiesCamp mainCamp;
    public UrsaMinor Ursa;
    public UnicornsSpawn UniSpawn;
    public Creature Player;
    public GameObject PegasusPrefab;
    public TimeCount timeCount;
    public int ursaDay;
    public int ursaHour;
    public List<PegasusPerHour> pegasusPerDay = new List<PegasusPerHour>();
    int pegPerDayI;

    void SpawnPegasus()
    {
        GameObject newpony = PoolManager.getGameObjectFromPool(PegasusPrefab);
        Pegasus newPony = newpony.GetComponent<Pegasus>();
        newPony.gameManager = gameManager;
        newPony.transform.parent = null;
        newPony.transform.position = new Vector3(transform.position.x + Random.Range(-3, 3), 5f + +Random.Range(-2, 2), -pegasus.Count);
        newPony.myHome = this;
        newPony.foodCount = 0;
        newPony.tempVictim = null;
        newPony.haveFood = false;
        newPony.deadTimer = 2f;
        newPony.timerGettingFood = 1.5f;
        newPony.leftTower = LeftTower;
        newPony.rightTower = RightTower;
        newPony.warehouse = warehouseBuild;
        newPony.leftCamp = leftCamp;
        newPony.mainCamp = mainCamp;
        newPony.Player = Player;
        newPony.ursa = Ursa;
        newPony.UniSpawn = UniSpawn;
        newPony.health = 50;
        newPony.GetComponent<RandomSex>().newPony = true;
        PegasusMovingController PegController = newPony.GetComponent<PegasusMovingController>();
        PegController.WingsAnimator.GetComponent<CopyColor>().enabled = true;
        PegController.ForgetColors();
        newPony.GetComponent<RandomColor>().newPony = true;
        RandomColor[] otherColors = newPony.GetComponentsInChildren<RandomColor>();
        for (int i = 0; i < otherColors.Length; i++)
        {
            otherColors[i].newPony = true;
        }
        pegasus.Add(newPony);
    }

    void CheckPegSpawn()
    {
        if (pegasusPerDay.Count > 0 && pegPerDayI < pegasusPerDay.Count - 1)
        {
            if (timeCount.days == pegasusPerDay[pegPerDayI].day) //если нужный день наступил
            {
                if (timeCount.hours >= pegasusPerDay[pegPerDayI].hour - 3 && timeCount.hours < pegasusPerDay[pegPerDayI].hour)
                {
                    crown.gameObject.SetActive(true);
                    crown.mustBeOff = false;
                }
                if (timeCount.hours == pegasusPerDay[pegPerDayI].hour) //если нужный час наступил
                {
                    if (pegasusPerDay[pegPerDayI].pegasusPerHour > 0) //спавним всех пегасов
                    {
                        SpawnPegasus();
                        pegasusPerDay[pegPerDayI].pegasusPerHour--;
                    }
                    else //когда пегасы кончаются, переходим к следующим
                    {
                        crown.mustBeOff = true;
                        pegPerDayI++;
                    }
                }
                else if (timeCount.hours > pegasusPerDay[pegPerDayI].hour) pegPerDayI++;
            }
            else if (timeCount.days > pegasusPerDay[pegPerDayI].day) pegPerDayI++;
        }
    }

    void CheckUrsaSpawn()
    {
        if(!Ursa.gameObject.activeSelf && Ursa.health > 0)
        {
            if(timeCount.days >= ursaDay)
            {
                if(timeCount.hours >= ursaHour)
                {
                    Ursa.gameObject.SetActive(true);
                }
            }
        }
    }

    private void Update()
    {
        CheckPegSpawn();
        CheckUrsaSpawn();
    }
}

[System.Serializable]
public class PegasusPerHour
{
    public int day;
    public int hour;
    public int pegasusPerHour;

    public PegasusPerHour(int _day, int _hour, int _pegasusPerHour)
    {
        day = _day;
        hour = _hour;
        pegasusPerHour = _pegasusPerHour;
    }
}
