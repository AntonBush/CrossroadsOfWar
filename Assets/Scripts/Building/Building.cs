using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public GameManager gameManager;
    public WeatherControl weather;
    public Resourses resourses;
    public int needWood;
    public int needWoodUpdateTwo;
    public int needWoodUpdateThree;
    public Transform Player;
    public string BuildingName;
    public string BuildText;
    public string UpdateTwoText;
    public string UpdateThreeText;
    public Text HintText;
    public int buildingLevel;
    public bool startBuilding;

    [HideInInspector]
    public AudioSource _audi;
    public AudioClip DownClip;
    public AudioClip UpClip;

    public bool onetime;

    bool oneside;
    public bool hintOff;
    public bool onetimeHint;
    public float timerWood;

    bool onetimeSound;

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
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + 5f, 0, 0);
            return false;
        }
        else
        {
            onetimeSound = false;
            return true;
        }

    }

    public bool Down(Transform _object)
    {
        CheckAudio();
        if (_object.localEulerAngles.x < 90)
        {
            if (!onetimeSound)
            {
                _audi.PlayOneShot(DownClip);
                onetimeSound = true;
            }
            _object.localEulerAngles = new Vector3(_object.localEulerAngles.x + 5f, 0, 0);
            return false;
        }
        else
        {
             onetimeSound = false;
            return true;
        }
    }

    public bool Up()
    {
        CheckAudio();
        if (transform.localEulerAngles.x > 1)
        {
            if (!_audi.isPlaying) _audi.PlayOneShot(UpClip);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x - 5f, 0, 0);
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool Up(Transform _object)
    {
        CheckAudio();
        if (_object.localEulerAngles.x > 1)
        {
            if (!_audi.isPlaying) _audi.PlayOneShot(UpClip);
            _object.localEulerAngles = new Vector3(_object.localEulerAngles.x - 5f, 0, 0);
            return false;
        }
        else
        {
            return true;
        }
    }

    public void CheckBuild()
    {
        MovingController controller = Player.GetComponent<MovingController>();
        if (!hintOff && Vector2.Distance(Player.transform.position, transform.position) < 3.5f && Mathf.Abs(controller.speedX) < 8 &&
        controller.health > 0)
        {
            if (timerWood > 0)
            {
                if (weather.weatherNumber >= 8)
                    HintText.text = "Нельзя строить во время дождя";
                else if (!weather.timeCount.isDay)
                    HintText.text = "Нельзя строить ночью";
                else HintText.text = "Не хватает древесины";
                timerWood -= Time.deltaTime;
            }
            else
            {
                if (needWood > 0)
                    HintText.text = BuildText + ", " + needWood + " древесины";
                else
                    HintText.text = BuildText;
            }

            HintText.color = new Color(1, 1, 1, 1);
            onetimeHint = true;

            if (Input.GetKeyDown(KeyCode.E) && !gameManager.GamePaused)
            {
                if (resourses.Wood >= needWood && weather.weatherNumber < 8 && weather.timeCount.isDay)
                {
                    startBuilding = true;
                    hintOff = true;
                }
                else
                    timerWood = 0.7f;
            }
        }
        else
        {
            if (onetimeHint)
            {
                timerWood = 0;
                HintText.color = new Color(1, 1, 1, 0);
                onetimeHint = false;
            }
        }
    }

    public void CheckUpdateTwo()
    {
        MovingController controller = Player.GetComponent<MovingController>();
        if (!hintOff && Vector2.Distance(Player.transform.position, transform.position) < 2f && Mathf.Abs(controller.speedX) < 8 &&
        controller.health > 0)
        {
            if (timerWood > 0)
            {
                if (weather.weatherNumber >= 8)
                    HintText.text = "Нельзя строить во время дождя";
                else if (!weather.timeCount.isDay)
                    HintText.text = "Нельзя строить ночью";
                else HintText.text = "Не хватает древесины";
                timerWood -= Time.deltaTime;
            }
            else
            {
                if (needWoodUpdateTwo > 0)
                    HintText.text = UpdateTwoText + ", " + needWoodUpdateTwo + " древесины";
                else
                    HintText.text = UpdateTwoText;
            }

            HintText.color = new Color(1, 1, 1, 1);
            onetimeHint = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (resourses.Wood >= needWoodUpdateTwo && weather.weatherNumber < 8 && weather.timeCount.isDay)
                {
                    onetime = true;
                    startBuilding = true;
                    hintOff = true;
                }
                else
                    timerWood = 0.7f;
            }
        }
        else
        {
            if (onetimeHint)
            {
                timerWood = 0;
                HintText.color = new Color(1, 1, 1, 0);
                onetimeHint = false;
            }
        }
    }

     public void CheckUpdateThree()
    {
        MovingController controller = Player.GetComponent<MovingController>();
        if (!hintOff && Vector2.Distance(Player.transform.position, transform.position) < 2f &&
        controller.health > 0 && Mathf.Abs(controller.speedX) < 8)
        {
            if (timerWood > 0)
            {
                if (weather.weatherNumber >= 8)
                    HintText.text = "Нельзя строить во время дождя";
                else if (!weather.timeCount.isDay)
                    HintText.text = "Нельзя строить ночью";
                else HintText.text = "Не хватает древесины";
                timerWood -= Time.deltaTime;
            }
            else
                if (needWoodUpdateThree > 0)
                HintText.text = UpdateThreeText + ", " + needWoodUpdateThree + " древесины";
            else
                HintText.text = UpdateThreeText;

            HintText.color = new Color(1, 1, 1, 1);
            onetimeHint = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (resourses.Wood >= needWoodUpdateThree && weather.weatherNumber < 8 && weather.timeCount.isDay)
                {
                    onetime = true;
                    startBuilding = true;
                    hintOff = true;
                }
                else
                    timerWood = 0.7f;
            }
        }
        else
        {
            if (onetimeHint)
            {
                timerWood = 0;
                HintText.color = new Color(1, 1, 1, 0);
                onetimeHint = false;
            }
        }
    }
}
