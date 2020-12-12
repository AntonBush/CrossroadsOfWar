using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class WeatherControl : MonoBehaviour
{
    ColorGradingModel.Settings color_settings = new ColorGradingModel.Settings();
    PostProcessingProfile myProfile;

    public GameManager gameManager;

    public bool changeWeather;

    public int weatherNumber;

    public TimeCount timeCount; //штуки, которые нужны скрипту, чтоб управлять погодой

    public GameObject ThunderImage;
    public AudioSource ThunderSource;
    public AudioClip ThunderSound;
    float thunderTimer;
    float thunder1;
    float thunder2;
    float thunderSoundTimer;
    bool thunderOnetime1, thunderOnetime2;
    bool thunderSoundTimerOneTime;

    public ParticlesMove particlesMove;
    public ParticlesMove cloudsMove;

    public SpriteRenderer Sun;
    public SpriteRenderer SadSky;
    public ParticleSystem Rainy;
    public ParticleSystem Snowy;

    public AudioSource RainSound;

    public List<WeatherTiming> weatherTimings = new List<WeatherTiming>();

    float speed = 8f;

    bool staticClouds, deleteClouds, enableClouds;
    bool sadSky, rainOn;

    int tempHours;
    bool onetime;

    int tempWeatherID;

    bool dontCheckWeather;

    void UpdateClouds()
    {
        if (deleteClouds)
        {
            cloudsMove.enabled = false;
            if (cloudsMove.particles.Count > 0)
            {
                for (int i = 0; i < cloudsMove.particles.Count; i++)
                    cloudsMove.particles[i].needToBeDestroyed = true;
            }
            else deleteClouds = false;
        }
        else
        {
            if (enableClouds) cloudsMove.enabled = true;
        }
    }

    void MakeCloudsStatic(bool _enableClouds, bool _static)
    {
        if (staticClouds != _static) deleteClouds = true;
        staticClouds = _static;

        if (_static)
        {
            cloudsMove.minSpeed = 0f;
            cloudsMove.maxSpeed = 0f;
            cloudsMove.minX = -5f;
            cloudsMove.maxX = 5f;
            cloudsMove.random_side = true;
        }
        else
        {
            cloudsMove.minSpeed = 0.05f;
            cloudsMove.maxSpeed = 0.3f;
            cloudsMove.minX = -15f;
            cloudsMove.maxX = 15f;
            cloudsMove.random_side = false;
        }

        if (_enableClouds)
        {
            cloudsMove.enabled = true;
            cloudsMove.maxParticlesCount = Random.Range(1, 7);
        }
        else
        {
            deleteClouds = true;
            cloudsMove.enabled = false;
        }
    }

    void UpdateParticles()
    {
        if (!particlesMove.enabled)
        {
            for (int i = 0; i < particlesMove.particles.Count; i++)
                particlesMove.particles[i].needToBeDestroyed = true;
        }
    }

    void SetSadSky()
    {
        if (sadSky)
        {
            if (SadSky.color.a < 1) SadSky.color = new Color(1f, 1f, 1f, SadSky.color.a + Time.deltaTime / speed);
            if (Sun.color.a > 0) Sun.color = new Color(1f, 1f, 1f, Sun.color.a - Time.deltaTime / speed);
            if (color_settings.basic.contrast > 0.7f)
            {
                color_settings.basic.contrast -= Time.deltaTime / speed;
            }
            if (color_settings.basic.saturation > 0.8f)
            {
                color_settings.basic.saturation -= Time.deltaTime / speed;
            }
        }
        else
        {
            if (SadSky.color.a > 0) SadSky.color = new Color(1f, 1f, 1f, SadSky.color.a - Time.deltaTime / speed);
            if (Sun.color.a < 1) Sun.color = new Color(1f, 1f, 1f, Sun.color.a + Time.deltaTime / speed);
            if (color_settings.basic.contrast < 1f)
            {
                color_settings.basic.contrast += Time.deltaTime / speed;
            }
            if (color_settings.basic.saturation < 1f)
            {
                color_settings.basic.saturation += Time.deltaTime / speed;
            }
        }
        myProfile.colorGrading.settings = color_settings;
    }

    void SetRainOn()
    {
        var emit = Rainy.emission;
        var emit1 = Snowy.emission;
        if (rainOn)
        {
            if (emit.rateOverTimeMultiplier < 1500)
                emit.rateOverTimeMultiplier += 5;
            if(emit1.rateOverTimeMultiplier < 200)
                emit1.rateOverTimeMultiplier += 5;
            if (RainSound.volume < gameManager.soundVolume * 0.3f) RainSound.volume += Time.deltaTime / 9f;
            else RainSound.volume -= Time.deltaTime / 4f;
        }
        else
        {
            if (emit.rateOverTimeMultiplier > 0)
                emit.rateOverTimeMultiplier -= 5;
            if(emit1.rateOverTimeMultiplier > 0)
                emit1.rateOverTimeMultiplier -= 5;
            if (RainSound.volume > 0) RainSound.volume -= Time.deltaTime / 9f;
        }
       
    }

    void UpdateThunder()
    {
        if(rainOn && weatherNumber > 10)
        {
            if(!thunderOnetime1)
            {
                thunderTimer = Random.Range(10f, 40f);
                thunderOnetime1 = true;
                thunder1 = thunder2 = 0.2f;
            }
            else
            {
                if (thunderTimer > 0)
                {
                    thunderTimer -= Time.deltaTime;
                }
                else
                {

                    if (thunder1 > 0)
                    {
                        ThunderImage.SetActive(true);
                        thunder1 -= Time.deltaTime;
                    }
                    else
                    {
                        if (thunder2 > 0)
                        {
                            ThunderImage.SetActive(false);
                            thunder2 -= Time.deltaTime;
                            if (thunderOnetime2)
                            {
                                thunderOnetime1 = false;
                                thunderSoundTimer = Random.Range(1f, 4f);
                                thunderSoundTimerOneTime = true;
                            }
                        }
                        else
                        {
                            thunderOnetime2 = true;
                            thunder1 = thunder2 = 0.2f;
                        }
                    }
                }
            }
            if(thunderSoundTimerOneTime)
            {
                if(thunderSoundTimer > 0)
                {
                    thunderSoundTimer -= Time.deltaTime;
                }
                else
                {
                    ThunderSource.volume = gameManager.soundVolume;
                    ThunderSource.PlayOneShot(ThunderSound);
                    thunderSoundTimerOneTime = false;
                }
            }
        }
        else
        {
            thunderSoundTimerOneTime = thunderOnetime1 = false;
        }
    }

    void SetParameters(bool _enableClouds, bool _staticClouds, bool enabledParticles, int FogColor, bool _sadSky, bool _rainOn)
    {
        MakeCloudsStatic(enableClouds, _staticClouds);
        particlesMove.enabled = enabledParticles;
        enableClouds = _enableClouds;

        sadSky = _sadSky;
        rainOn = _rainOn;
    }

    void ChangeWeather()
    {
        if (weatherNumber == 0) //0 - ясная погода с частицами
        {
            SetParameters(false, true, true, 0, false, false);
        }
        if (weatherNumber == 1)  //1 - ясная погода без частиц
        {
            SetParameters(false, true, false, 0, false, false);
        }
        if (weatherNumber == 2) //2 - облачная погода с движущимися облаками с частицами
        {
            SetParameters(true, false, true, 0, false, false);
        }
        if (weatherNumber == 3)  //3 - облачная погода с движущимися облаками без частиц
        {
            SetParameters(true, false, false, 0, false, false);
        }
        if (weatherNumber == 4)  //4 - облачная погода со статичными облаками с частицами
        {
            SetParameters(true, true, true, 0, false, false);
        }
        if (weatherNumber == 5)  //5 - облачная погода со статичными облаками без частиц
        {
            SetParameters(true, true, false, 0, false, false);
        }
        if (weatherNumber == 6) //6 - туман (без облаков и частиц)
        {
            SetParameters(true, true, true, 0, false, false);
        }
        if (weatherNumber == 7)  //7 - тучи (без частиц)
        {
            SetParameters(false, false, false, 0, true, false);
        }
        if (weatherNumber == 8)  //8 - дождик со статичными облаками с частицами
        {
            SetParameters(true, true, true, 0, false, true);
        }
        if (weatherNumber == 9) //9 - дождик со статичными облаками без частиц
        {
            SetParameters(true, true, false, 0, false, true);
        }
        if (weatherNumber == 10)   //10 - дождик с динамичными облаками с частицами
        {
            SetParameters(true, false, true, 0, false, true);
        }
        if (weatherNumber == 11)   //11 - дождик с динамичными облаками без частиц
        {
            SetParameters(true, false, false, 0, false, true);
        }
        if (weatherNumber == 12)   //12 - дождь с тучами
        {
            SetParameters(false, false, false, 0, true, true);
        }
    }

    void UpdateWeather()
    {
        UpdateClouds();
        UpdateParticles();
        SetSadSky();
        SetRainOn();
        UpdateThunder();
    }

    int randomweather
    {
        get
        {
            if(timeCount.days == 0)
            return Random.Range(0, 6);
            else 
            return Random.Range(0, 13);
        }
    }

    // void CreateWeather()
    // {
    //     if (timeCount.hours == 4)
    //     {
    //         tempHours = 4;
    //         weatherTimings.Clear();
    //         while (tempHours < 21)
    //         {
    //             int randomWeather = randomweather;
    //             int randomTime = Random.Range(2, 21 - tempHours);
    //             if (tempHours > 4)
    //             {
    //                 if (tempHours + randomTime < (21 - 2))
    //                 {
    //                     randomTime += Random.Range(0, 1);
    //                 }
    //             }
    //             weatherTimings.Add(new WeatherTiming(tempHours + randomTime, randomWeather));
    //             tempHours += randomTime;
    //         }
    //     }
    //     if (timeCount.hours == 21)
    //     {
    //         tempHours = 21;
    //         weatherTimings.Clear();
    //         while (tempHours < 24)
    //         {
    //             int randomWeather = randomweather;
    //             while (randomWeather == 6)
    //             {
    //                 randomWeather = randomweather;
    //             }
    //             int randomTime = Random.Range(2, 24 - tempHours);
    //             weatherTimings.Add(new WeatherTiming(tempHours + randomTime, randomWeather));
    //             tempHours += randomTime;
    //         }
    //         tempHours = 0;
    //         while (tempHours < 4)
    //         {
    //             int randomWeather = randomweather;
    //             while (randomWeather == 6)
    //             {
    //                 randomWeather = randomweather;
    //             }
    //             int randomTime = Random.Range(2, 4 - tempHours);
    //             weatherTimings.Add(new WeatherTiming(tempHours + randomTime, randomWeather));
    //             tempHours += randomTime;
    //         }
    //     }
    //     onetime = true;
    // }

    // void CheckTime()
    // {
    //     if (timeCount.hours == 4)
    //     {
    //         if (!onetime)
    //             CreateWeather();
    //     }
    //     if (timeCount.hours == 5)
    //     {
    //         onetime = false;
    //     }
    //     if (timeCount.hours == 21)
    //     {
    //         if (!onetime)
    //             CreateWeather();
    //     }
    //     if (timeCount.hours == 22)
    //     {
    //         onetime = false;
    //     }
    // }

    // void ManageWeather()
    // {
    //     if (tempWeatherID < weatherTimings.Count - 1) tempWeatherID++;
    //     else tempWeatherID = 0;

    //     if (timeCount.hours >= weatherTimings[tempWeatherID].beginHour)
    //     {
    //         if (!weatherTimings[tempWeatherID].over)
    //         {
    //             weatherNumber = weatherTimings[tempWeatherID].myWeatherNumber;
    //             changeWeather = true;
    //             weatherTimings[tempWeatherID].over = true;
    //         }
    //     }
    // }

    void ManageWeather()
    {
        if (timeCount.days == weatherTimings[tempWeatherID].beginDay) //если нужный день наступил
        {
            if (timeCount.hours == weatherTimings[tempWeatherID].beginHour) //если нужный час наступил
            {
                weatherNumber = weatherTimings[tempWeatherID].myWeatherNumber;
                changeWeather = true;
                if (tempWeatherID < weatherTimings.Count - 1)
                    tempWeatherID++;
                else dontCheckWeather = true;
            }
            else if (timeCount.hours > weatherTimings[tempWeatherID].beginHour) tempWeatherID++;
        }
        else if (timeCount.days > weatherTimings[tempWeatherID].beginDay) tempWeatherID++;
    }

    private void Start()
    {
        myProfile = GetComponent<PostProcessingBehaviour>().profile;
        color_settings = myProfile.colorGrading.settings;
    }

    private void Update()
    {
        //изменение параметров под соответствующую погоду
        UpdateWeather();
        if (changeWeather)
        {
            ChangeWeather();
            changeWeather = false;
        }

        //создание и управление прогнозом погоды за день
        // CheckTime();
        if(!dontCheckWeather)
        ManageWeather();
    }
}

[System.Serializable]
public class WeatherTiming
{
    public int beginDay;
    public int beginHour;
    public int myWeatherNumber;

    // public bool over;
    public WeatherTiming(int _day, int _hour, int _number)
    {
        beginDay = _day;
        beginHour = _hour;
        myWeatherNumber = _number;
    }
}