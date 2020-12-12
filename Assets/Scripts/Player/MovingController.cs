using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingController : Creature
{
    //определяет, как персонаж будет двигаться + его атрибуты типа здоровья + звуки
    public bool hasBow;

    [SerializeField]
    ArrowTest arrowPlayerScript;
    public SpriteRenderer[] partsOfBody;
    public Animator clothAnimator;
    public Animator BeardAnimator;
    public Animator EyesBlackAnimator;
    public Animator EyesGreyAnimator;
    public Animator HairAnimator;
    public Animator LampAnimator;
    public Animator BowAnimator;

    public AudioClip[] Walk;
    public AudioClip[] Run;

    public AudioClip[] snowWalk;
    public AudioClip[] snowRun;

    public float minX, maxX;

    int i1;
    float timerSit = 6f;
    bool gotRed, onetime;

    public List<Color> myOwnColors = new List<Color>();
    float timerCheckColor = 0.2f;

    float stepTimer;
    int stepI;

    int uniI, pegI;

    void Start()
    {
        InitiateCreature();
        positionY = -0.98f;
        _audi = GetComponent<AudioSource>();
    }

    public bool SomeoneIsTryingToKillMe
    {
        get
        {
            if (arrowPlayerScript.Timberwolf1.health > 0 && Vector2.Distance(transform.position, arrowPlayerScript.Timberwolf1.transform.position) < 13f)
            {
                return true;
            }
            if (arrowPlayerScript.Timberwolf2.health > 0 && Vector2.Distance(transform.position, arrowPlayerScript.Timberwolf2.transform.position) < 13f)
            {
                return true;
            }
            if (arrowPlayerScript.UniSpawn.unicorns.Count > 0)
            {
                if (uniI < arrowPlayerScript.UniSpawn.unicorns.Count - 1) uniI++;
                else uniI = 0;

                if (arrowPlayerScript.UniSpawn.unicorns[uniI].health == 0) uniI = 0;
                else if (arrowPlayerScript.UniSpawn.unicorns[uniI].health > 0 && Vector2.Distance(transform.position, arrowPlayerScript.UniSpawn.unicorns[uniI].transform.position) < 13f)
                {
                    return true;
                }
            }
            if (arrowPlayerScript.PegaSpawn.pegasus.Count > 0)
            {
                if (pegI < arrowPlayerScript.PegaSpawn.pegasus.Count - 1) pegI++;
                else pegI = 0;

                if (arrowPlayerScript.PegaSpawn.pegasus[pegI].health == 0) pegI = 0;
                else if (arrowPlayerScript.PegaSpawn.pegasus[pegI].health > 0 && Vector2.Distance(transform.position, arrowPlayerScript.PegaSpawn.pegasus[pegI].transform.position) < 16f)
                {
                    return true;
                }
            }
            return false;
        }
    }

    void SetAllAnims(float _speed)
    {
        anim.SetFloat("speed", _speed);
        clothAnimator.SetFloat("speed", _speed);
        if (BeardAnimator.gameObject.activeSelf)
        {
            BeardAnimator.SetFloat("speed", _speed);
        }
        if (BowAnimator.gameObject.activeSelf)
        {
            BowAnimator.SetFloat("speed", _speed);
        }
        EyesBlackAnimator.SetFloat("speed", _speed);
        EyesGreyAnimator.SetFloat("speed", _speed);
        HairAnimator.SetFloat("speed", _speed);
        LampAnimator.SetFloat("speed", _speed);
    }

    public void SetAllAnims(string trigger)
    {
        anim.SetTrigger(trigger);
        clothAnimator.SetTrigger(trigger);
        if (BeardAnimator.gameObject.activeSelf)
        {
            BeardAnimator.SetTrigger(trigger);
        }
        if (BowAnimator.gameObject.activeSelf)
        {
            BowAnimator.SetTrigger(trigger);
        }
        EyesBlackAnimator.SetTrigger(trigger);
        EyesGreyAnimator.SetTrigger(trigger);
        HairAnimator.SetTrigger(trigger);
        LampAnimator.SetTrigger(trigger);
    }

    public void SetAllAnims(string name, bool boolean)
    {
        anim.SetBool(name, boolean);
        clothAnimator.SetBool(name, boolean);
        if (BeardAnimator.gameObject.activeSelf)
        {
            BeardAnimator.SetBool(name, boolean);
        }
        if (BowAnimator.gameObject.activeSelf)
        {
            BowAnimator.SetBool(name, boolean);
        }
        EyesBlackAnimator.SetBool(name, boolean);
        EyesGreyAnimator.SetBool(name, boolean);
        HairAnimator.SetBool(name, boolean);
        LampAnimator.SetBool(name, boolean);
    }

    public void SetAllSRs(Material material, bool red)
    {
        SR.material = material;
        if (red)
        {
            SR.color = Color.red;
            gotRed = true;
        }
        else
        {
            if (gotRed)
            {
                SR.color = myOwnColors[0];
            }
        }
        if (gotRed)
        {
            for (int i = 0; i < partsOfBody.Length; i++)
            {
                partsOfBody[i].material = material;
                if (red)
                {
                    partsOfBody[i].color = Color.red;
                }
                else
                {
                    if (gotRed)
                    {
                        partsOfBody[i].color = myOwnColors[i + 1];
                    }
                }
            }
        }
        if (!red) gotRed = false;
    }

    void CheckColors()
    {
        if (!onetime)
        {
            if (timerCheckColor > 0)
            {
                timerCheckColor -= Time.deltaTime;
            }
            else
            {
                myOwnColors.Add(SR.color);
                for (int i = 0; i < partsOfBody.Length; i++)
                {
                    myOwnColors.Add(partsOfBody[i].color);
                }
                onetime = true;
            }
        }
    }

    void CheckBow()
    {
        if (hasBow)
        {
            BowAnimator.gameObject.SetActive(true);
            BowAnimator.GetComponent<SpriteRenderer>().flipX = SR.flipX;
        }
        else BowAnimator.gameObject.SetActive(false);
    }

    void SoundSteps()
    {
        _audi.volume = 0.7f * gameManager.soundVolume;
        float spd = Mathf.Abs(speedX);
        bool snow = transform.position.x > 20;
        if (spd > 0 && spd < 9)
        {
            //играем звуки шагов
            if (stepTimer > 0)
            {
                stepTimer -= Time.deltaTime;
            }
            else
            {
                if (snow)
                {
                    _audi.PlayOneShot(snowWalk[stepI]);
                }
                else
                {
                    _audi.PlayOneShot(Walk[stepI]);
                    
                }
                stepTimer = 0.35f;
                if (stepI < Walk.Length - 1) stepI++;
                else stepI = 0;
                
            }
        }
        else if(spd > 9)
        {
            //играем звуки бега
            if (stepTimer > 0)
            {
                stepTimer -= Time.deltaTime;
            }
            else
            {
                if (snow)
                {
                    _audi.PlayOneShot(snowRun[stepI]);
                }
                else
                {
                    _audi.PlayOneShot(Run[stepI]);
                }
                stepTimer = 0.58f;
                if (stepI < Run.Length - 1) stepI++;
                else stepI = 0;
               
            }
        }
    }

    void Update()
    {
        float newSpeed = Mathf.Abs(speedX);
        SetAllAnims(newSpeed);
        rigbody.velocity = new Vector2(speedX, speedY);

        SoundSteps();

        CheckColors(); //с задержкой запоминаем цвета т.к. гребаный рандом слишком медленный
        CheckBow();

        if(!gameManager.GamePaused)
        {
            if (!isHitted)
            {
                if (health > 0)
                {
                    if (arrowPlayerScript.waiting <= 0)
                    {
                        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && transform.position.x < maxX)
                        {
                            timerSit = 6f;
                            SetAllAnims("sit", false);
                            if (Input.GetKey(KeyCode.LeftShift))
                            {
                                speedX = 10f;
                            }
                            else
                            {
                                speedX = 4f;
                            }

                            if (SR.flipX)
                            {
                                SR.flipX = false;
                                for (int i = 0; i < partsOfBody.Length; i++)
                                {
                                    if (partsOfBody[i].gameObject.activeSelf)
                                        partsOfBody[i].flipX = false;
                                }
                            }
                        }
                        else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && transform.position.x > minX)
                        {
                            timerSit = 6f;
                            SetAllAnims("sit", false);
                            if (Input.GetKey(KeyCode.LeftShift))
                            {
                                speedX = -10f;
                            }
                            else
                            {
                                speedX = -4f;
                            }
                            if (!SR.flipX)
                            {
                                SR.flipX = true;
                                for (int i = 0; i < partsOfBody.Length; i++)
                                {
                                    if (partsOfBody[i].gameObject.activeSelf)
                                        partsOfBody[i].flipX = true;
                                }
                            }
                        }
                        else
                        {
                            speedX = 0;
                            if (timerSit > 0)
                            {
                                timerSit -= Time.deltaTime;
                            }
                            else
                            {
                                SetAllAnims("sit", true);
                            }
                        }
                    }
                    else
                    {
                        timerSit = 6f;
                        SetAllAnims("sit", false);
                    }
                }
                else
                {
                    PlayerPrefs.SetInt("DealthCount", PlayerPrefs.GetInt("DealthCount") + 1);
                    rigbody.velocity = Vector2.zero;
                    if (deadTimer > 0)
                    {
                        SetAllAnims("dead", true);
                        deadTimer -= Time.deltaTime;
                    }
                    else
                    {
                        GameObject deadParticles = PoolManager.getGameObjectFromPool(DeadParticles);
                        deadParticles.transform.position = transform.position;
                        deadParticles.GetComponent<DeadParticles>().timerDissapear = 1f;
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
