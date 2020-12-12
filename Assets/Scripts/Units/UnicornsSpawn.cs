using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornsSpawn : MonoBehaviour
{
    public AudioClip DownClip;
    public AudioClip SpawnClip;
    public int health;
    public GameObject SpawnMagic;
    public GameObject Twilight;
    public int twilightDay;
    public int twilightHour;
    public GameManager gameManager;
    public Crown crown;
    public WallBuild rightWall;
    public WarehouseBuild warehouseBuild;
    public EarthponiesCamp rightCamp;
    public EarthponiesCamp mainCamp;
    public UrsaMinor Ursa;
    public PegasusSpawn PegaSpawn;
    public Creature Player;
    public GameObject UnicornPrefab;
    public TimeCount timeCount;
    public List<UnicornsPerHour> unicornsPerDay = new List<UnicornsPerHour>();
    public List<Unicorn> unicorns = new List<Unicorn>();
    int uniPerDayI;
    bool TwySpawned;

    AudioSource _audi;
    bool onetimeSound;

    float magicTimer;

    void SpawnUnicorn()
    {
        GameObject newpony = PoolManager.getGameObjectFromPool(UnicornPrefab);
        Unicorn newPony = newpony.GetComponent<Unicorn>();
        newPony.gameManager = gameManager;
        newPony.transform.parent = null;
        newPony.transform.position = new Vector3(transform.position.x + Random.Range(-3, 3), -1.04f, -unicorns.Count);
        newPony.myHome = this;
        newPony.foodIHave = 0;
        newPony.hasFood = false;
        newPony.uniItem.gameObject.SetActive(true);
        newPony.uniItem.GetComponent<SpriteRenderer>().sprite = newPony.Weapon;
        newPony.tempVictim = null;
        newPony.deadTimer = 2f;
        newPony.timerGettingFood = 1.5f;
        newPony.RightWall = rightWall;
        newPony.warehouse = warehouseBuild;
        newPony.rightCamp = rightCamp;
        newPony.mainCamp = mainCamp;
        newPony.pegaSpawn = PegaSpawn;
        newPony.Player = Player;
        newPony.ursa = Ursa;
        newPony.health = 60;
        newPony.GetComponent<RandomSex>().newPony = true;
        newPony.GetComponent<UnicornMovingController>().ForgetColors();
        newPony.GetComponent<RandomColor>().newPony = true;
        RandomColor[] otherColors = newPony.GetComponentsInChildren<RandomColor>();
        for (int i = 0; i < otherColors.Length; i++)
        {
            otherColors[i].newPony = true;
        }
        unicorns.Add(newPony);
        magicTimer = 0.3f;
        CheckAudio();
        _audi.PlayOneShot(SpawnClip);
    }

    void CheckUniSpawn()
    {
        if (unicornsPerDay.Count > 0 && uniPerDayI < unicornsPerDay.Count - 1)
        {
            if (timeCount.days == unicornsPerDay[uniPerDayI].day) //если нужный день наступил
            {
                if (timeCount.hours >= unicornsPerDay[uniPerDayI].hour - 3 && timeCount.hours < unicornsPerDay[uniPerDayI].hour)
                {
                    crown.gameObject.SetActive(true);
                    crown.mustBeOff = false;
                }
                if (timeCount.hours == unicornsPerDay[uniPerDayI].hour) //если нужный час наступил
                {
                    if (unicornsPerDay[uniPerDayI].unicornsPerHour > 0) //спавним всех единорогов 
                    {
                        SpawnUnicorn();
                        unicornsPerDay[uniPerDayI].unicornsPerHour--;
                    }
                    else //когда единороги кончаются, переходим к следующим
                    {
                        crown.mustBeOff = true;
                        uniPerDayI++;
                    }
                }
                else if (timeCount.hours > unicornsPerDay[uniPerDayI].hour) uniPerDayI++;
            }
            else if (timeCount.days > unicornsPerDay[uniPerDayI].day) uniPerDayI++;
        }
    }

    void CheckTwilight()
    {
        if (PlayerPrefs.GetInt("Twy") != 1 && !TwySpawned)
        {
            if (timeCount.days >= twilightDay)
            {
                if (timeCount.hours >= twilightHour)
                {
                    if (Vector2.Distance(Twilight.transform.position, Player.transform.position) > 11f)
                    {
                        Twilight.gameObject.SetActive(true);
                        TwySpawned = true;
                    }
                }
            }
        }
    }

    void UpdateMagic()
    {
        if(magicTimer > 0)
        {
            magicTimer -= Time.deltaTime;
            SpawnMagic.SetActive(true);
        }
        else
        {
            SpawnMagic.SetActive(false);
        }
    }

    void CheckAudio()
    {
        if (_audi == null) _audi = GetComponent<AudioSource>();
        _audi.volume = gameManager.soundVolume;
    }

    public bool Down()
    {
        CheckAudio();
        if (transform.localEulerAngles.x < 90)
        {
            if (!onetimeSound)
            {
                _audi.PlayOneShot(DownClip);
                onetimeSound = true;
            }
            transform.Translate(0, -1, 0);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + 5f, 0, 0);
            return false;
        }
        else
        {
            onetimeSound = false;
            return true;
        }
    }

    public void HitSpawner(int damage)
    {
        health -= damage;
        if (Random.value > 0.5f) SpawnUnicorn();
        if (Random.value > 0.5f) SpawnUnicorn();
    }

    private void Update()
    {
        if (health > 0)
        {
            UpdateMagic();
            CheckUniSpawn();
            CheckTwilight();
        }
        else
        {
            if(Down())
            {
                if(!_audi.isPlaying)
                gameObject.SetActive(false);
            }
        }
    }
}

[System.Serializable]
public class UnicornsPerHour
{
    public int day;
    public int hour;
    public int unicornsPerHour;

    public UnicornsPerHour(int _day, int _hour, int _unicornsPerHour)
    {
        day = _day;
        hour = _hour;
        unicornsPerHour = _unicornsPerHour;
    }
}