using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFire : Building
{
    public SaveLoadGame savingGame;

    [SerializeField]
    Flag flag;

    [SerializeField]
    GameObject SaveText;
    float saveTextTimer;

    public WorkingManager workManager;
    public WallBuild LeftWall;
    public WallBuild RightWall;
    public GameObject PointLight;

    public GameObject Tower;
    public Sprite TowerLevel3;
    public Sprite WallLevel3;

    public AudioClip FireAudio;
    public AudioSource fireAudio;

    [HideInInspector]
    public int huntersFollow;
    Animator anim;


    float timerOff = 3f; //таймеры для потухания костра во время дождя
    float timerOn = 3f;

    float timerFirstSave;
    bool firstSave;

    private void Start()
    {
        anim = GetComponent<Animator>();
        _audi = GetComponent<AudioSource>();

        if(buildingLevel > 0)
        {
            PointLight.SetActive(true);
            anim.SetBool("burn", true);
        }
        if(buildingLevel == 2)
        {
            Tower.transform.localEulerAngles = new Vector3(0, 0, 0);
            LeftWall.health = LeftWall.healthMax = 150;
            RightWall.health = RightWall.healthMax = 150;
        }
        if (buildingLevel == 3)
        {
            Tower.transform.localEulerAngles = new Vector3(0, 0, 0);
            Tower.GetComponent<SpriteRenderer>().sprite = TowerLevel3;
            LeftWall.health = LeftWall.healthMax = 250;
            RightWall.health = RightWall.healthMax = 250;
        }
    }

    private void Update()
    {
        if(saveTextTimer > 0)
        {
            SaveText.SetActive(true);
            saveTextTimer -= Time.deltaTime;
        }
        else
        {
            SaveText.SetActive(false);
        }

        if (buildingLevel == 0)
        {
            CheckBuild();
            if (startBuilding)
            {
                if (Down())
                {
                    PointLight.SetActive(true);
                    anim.SetBool("burn", true);
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
                        timerFirstSave = 1f;
                        hintOff = false;
                        resourses.AddResourses(-needWood, 0);
                        buildingLevel++;
                        onetime = false;
                    }
                }
            }

        }
        if (buildingLevel > 0)
        {
            if(fireAudio.clip != FireAudio)
            {
                fireAudio.clip = FireAudio;
                fireAudio.Play();
                DownClip = LeftWall.DownClip;
            }

            if (weather.weatherNumber > 7)
            {
                if (timerOff > 0)
                {
                    timerOff -= Time.deltaTime;
                }
                else
                {
                    fireAudio.volume = 0;
                    PointLight.SetActive(false);
                    anim.SetBool("burn", false);
                    timerOn = 3f;
                }
            }
            else
            {
                if (timerOn > 0)
                {
                    timerOn -= Time.deltaTime;
                }
                else
                {
                    fireAudio.volume = 0.45f;
                    PointLight.SetActive(true);
                    timerOff = 3f;
                    anim.SetBool("burn", true);
                }
            }
        }
        if (buildingLevel == 1)
        {
            if (!flag.enabled) //вот тут игра сохраняется при зажигании костра 
            {
                if (timerFirstSave > 0)
                {
                    timerFirstSave -= Time.deltaTime;
                }
                else
                {
                    if (!firstSave)
                    {
                        saveTextTimer = 1.5f;
                        savingGame.SaveGame();
                        firstSave = true;
                    }
                }

            }

            CheckUpdateTwo();

            if (startBuilding)
            {
                if (Up(Tower.transform))
                {
                    if (onetime)
                    {
                        hintOff = false;
                        LeftWall.health = LeftWall.healthMax = 150;
                        RightWall.health = RightWall.healthMax = 150;
                        resourses.AddResourses(-needWoodUpdateTwo, 0);
                        buildingLevel++;
                        saveTextTimer = 1.5f;
                        savingGame.SaveGame();
                        startBuilding = false;
                        onetime = false;
                    }
                }
            }
        }
        if (buildingLevel == 2)
        {
            CheckUpdateThree();

            if (startBuilding)
            {
                if (Down(Tower.transform))
                {
                    Tower.GetComponent<SpriteRenderer>().sprite = TowerLevel3;
                    onetime = true;
                    startBuilding = false;
                }
            }
            else
            {
                if (Up(Tower.transform))
                {
                    if (onetime)
                    {
                        LeftWall.health = LeftWall.healthMax = 250;
                        RightWall.health = RightWall.healthMax = 250;
                        resourses.AddResourses(-needWoodUpdateThree, 0);
                        buildingLevel++;
                        saveTextTimer = 1.5f;
                        savingGame.SaveGame();
                        startBuilding = false;
                        onetime = false;
                    }
                }
            }
        }
    }

}
