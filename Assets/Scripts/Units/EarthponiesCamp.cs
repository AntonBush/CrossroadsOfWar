using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthponiesCamp : MonoBehaviour
{
    public MusicScript musicScript;

    [SerializeField]
    GameManager gameManager;

    public Transform Player;
    public EarthponiesCamp mainCamp;
    public Text hintText;
    public Resourses res;
    public Transform fire;
    public bool myCamp;
    public Manticore timberWolf1;
    public Manticore timberWolf2;
    public UrsaMinor Ursa;
    public UnicornsSpawn UniSpawn;
    public PegasusSpawn PegaSpawn;
    public Transform MainCamera;
    public GameObject earthponyPrefab;
    public WeatherControl weather;
    public TimeCount timing;
    public Animator mainFire;
    public GameObject Light;

    [HideInInspector]
    public MainFire fireBuild;

    public bool placeOneBusy;
    public bool placeTwoBusy;

    public bool guitarIsPlaying;

    public float minX, maxX;

    float cooldownSpawn;

    int ponyI;

    public List<Earthpony> Hunters = new List<Earthpony>();
    public List<Earthpony> Ponies = new List<Earthpony>();
    public List<bool> PoniesWalk;

    MovingController controller;

    AudioSource fireAudi;

    public GameObject SpawnNewPony(int health)
    {
        GameObject newpony = PoolManager.getGameObjectFromPool(earthponyPrefab);
        Earthpony newPony = newpony.GetComponent<Earthpony>();
        for (int i = 0; i < newPony.randomColors.Length; i++)
        {
            newPony.randomColors[i].enabled = false;
        }
        newPony.myOrder = gameManager.earthponiesCount + 6;
        newPony.gameManager = gameManager; //для настройки звуков
        newPony.transform.parent = null;
        newPony.transform.position = new Vector3(transform.position.x + Random.Range(-2, 2), -0.83f, -1);
        newPony.unit = true;
        newPony.myHome = this;
        newPony.health = health;
        newPony.maxX = -28.61f;
        newPony.minX = -99.34f;
        newPony.Camera = MainCamera;
        newPony.Timberwolf1 = timberWolf1;
        newPony.Timberwolf2 = timberWolf2;
        newPony.ursa = Ursa;
        newPony.UniSpawn = UniSpawn;
        newPony.PegaSpawn = PegaSpawn;
        newPony.fire = fire;
        HireUnit hireUnit = newpony.GetComponent<HireUnit>();
        if (controller == null) controller = Player.GetComponent<MovingController>();
        hireUnit.Cloth.originSR = controller.clothAnimator.GetComponent<SpriteRenderer>();
        hireUnit.Hood.enabled = hireUnit.Cloth.enabled = true;
        newPony.GetComponent<EarthponyMovingController>().ForgetColors();

        hireUnit.Player = controller;
        hireUnit.gameManager = gameManager; //для обработки паузы - чтоб кнопки не нажимались
        hireUnit.res = res;
        hireUnit.mainCamp = mainCamp;
        hireUnit.HintText = hintText;
        hireUnit.enabled = false;
        Ponies.Add(newPony);
        gameManager.earthponiesCount++;
        PoniesWalk.Add(true);
        res.Ponies++;

        return newpony;
    }

    public GameObject SpawnNewHunter(int health)
    {
        GameObject newpony = PoolManager.getGameObjectFromPool(earthponyPrefab);
        Earthpony newPony = newpony.GetComponent<Earthpony>();
        newPony.gameManager = gameManager; //для настройки звуков
        for (int i = 0; i < newPony.randomColors.Length; i++)
        {
            newPony.randomColors[i].enabled = false;
        }
        newPony.hunter = true;
        newPony.unit = true;
        newPony.Bow.SetActive(true);
        newPony.Bow.GetComponent<SpriteRenderer>().flipX = newPony.GetComponent<SpriteRenderer>().flipX;
        newPony.hasBow = true;
        newPony.transform.parent = null;
        newPony.myOrder = gameManager.earthponiesCount + 6;
        newPony.transform.position = new Vector3(transform.position.x + Random.Range(-10, 42), -0.83f, -1);
        newPony.myHome = this;
        newPony.health = health;
        newPony.maxX = -28.61f;
        newPony.minX = -99.34f;
        newPony.Camera = MainCamera;
        newPony.Timberwolf1 = timberWolf1;
        newPony.Timberwolf2 = timberWolf2;
        newPony.ursa = Ursa;
        newPony.UniSpawn = UniSpawn;
        newPony.PegaSpawn = PegaSpawn;
        newPony.fire = fire;
        HireUnit hireUnit = newpony.GetComponent<HireUnit>();
        if (controller == null) controller = Player.GetComponent<MovingController>();
        hireUnit.Cloth.originSR = controller.clothAnimator.GetComponent<SpriteRenderer>();
        hireUnit.Hood.enabled = hireUnit.Cloth.enabled = true;
        newPony.GetComponent<EarthponyMovingController>().ForgetColors();

        hireUnit.Player = controller;
        hireUnit.gameManager = gameManager; //для обработки паузы - чтоб кнопки не нажимались
        hireUnit.res = res;
        hireUnit.mainCamp = mainCamp;
        hireUnit.HintText = hintText;
        hireUnit.enabled = false;
        gameManager.earthponiesCount++;
        Hunters.Add(newPony);
        res.Ponies++;

        return newpony;
    }

    void SpawnPony()
    {
        GameObject newpony = PoolManager.getGameObjectFromPool(earthponyPrefab);
        Earthpony newPony = newpony.GetComponent<Earthpony>();
        newPony.gameManager = gameManager; //для настройки звуков
        newPony.myOrder = gameManager.earthponiesCount + 6;
        newPony.transform.parent = null;
        newPony.transform.position = new Vector3(transform.position.x + Random.Range(-10, 10), -0.83f, -1);
        newPony.myHome = this;
        newPony.health = 50;
        newPony.maxX = maxX;
        newPony.minX = minX;
        newPony.Camera = MainCamera;
        newPony.Timberwolf1 = timberWolf1;
        newPony.Timberwolf2 = timberWolf2;
        newPony.ursa = Ursa;
        newPony.UniSpawn = UniSpawn;
        newPony.PegaSpawn = PegaSpawn;
        newPony.fire = fire;
        newPony.GetComponent<RandomSex>().newPony = true;
        newPony.GetComponent<RandomColor>().newPony = true;
        newPony.deadTimer = 2f;
        newPony.onetimeDead = false;
        RandomColor[] otherColors = newPony.GetComponentsInChildren<RandomColor>();
        for (int i = 0; i < otherColors.Length; i++)
        {
            otherColors[i].newPony = true;
        }
        newPony.GetComponent<EarthponyMovingController>().ForgetColors();
        HireUnit hireUnit = newpony.GetComponent<HireUnit>();
        hireUnit.gameManager = gameManager; //для обработки паузы - чтоб кнопки не нажимались
        hireUnit.res = res;
        hireUnit.mainCamp = mainCamp;
        hireUnit.HintText = hintText;
        gameManager.earthponiesCount++;
        
        Ponies.Add(newPony);
        PoniesWalk.Add(true);
    }

    private void Start()
    {
        controller = Player.GetComponent<MovingController>();
        if (fire.GetComponent<MainFire>())
        {
            fireBuild = fire.GetComponent<MainFire>();
        }
        fireAudi = fire.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!myCamp)
        {
            if (weather.weatherNumber > 6)
            {
                mainFire.SetBool("burn", false);
                Light.SetActive(false);
                if (fireAudi.isPlaying)
                    fireAudi.Stop();
            }
            else
            {
                mainFire.SetBool("burn", true);
                Light.SetActive(true);
                if (!fireAudi.isPlaying)
                    fireAudi.Play();
            }

            if (Ponies.Count < 3)
            {
                if (cooldownSpawn > 0)
                {
                    cooldownSpawn -= Time.deltaTime;
                }
                else
                {
                    if (timing.hours > 4 && timing.hours < 21 && weather.weatherNumber < 8)
                    {
                        float dis = Vector2.Distance(transform.position, MainCamera.position);
                        if (dis > 20f && dis < 55f && gameManager.earthponiesCount < gameManager.earthponiesMax)
                        {
                            SpawnPony();
                        }
                    }
                }
            }
            else
            {
                cooldownSpawn = 15f;
            }
        }
        if (Ponies.Count > 0)
        {
            if (ponyI < Ponies.Count - 1) ponyI++;
            else ponyI = 0;

            if (!PoniesWalk[ponyI])
            {
                if(Ponies[ponyI].timerRunningForLife > 0)
                {
                    Ponies[ponyI].timerRunningForLife -= Time.deltaTime;
                }
                else if (!Ponies[ponyI].SomeoneIsTryingToKillMe && weather.weatherNumber < 8 && timing.hours > 4 && timing.hours < 21)
                {
                    Ponies[ponyI].health = 50;
                    Ponies[ponyI].gameObject.SetActive(true);
                    PoniesWalk[ponyI] = true;
                }
            }
        }
    }
}