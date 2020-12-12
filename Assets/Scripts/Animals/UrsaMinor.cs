using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrsaMinor : Animal
{
    public bool destroyWall;
    public WallBuild RightWall;
    public Creature Player;
    public EarthponiesCamp earthponiesSpawnRight;
    public EarthponiesCamp earthponiesSpawnLeft;
    public EarthponiesCamp mainSpawn;
    public UnicornsSpawn UniSpawn;
    public PegasusSpawn pegaSpawn;

    public float placeToGO;
    public float placeToGoBack;
    bool goBack;

    public int damage;

    public Creature tempVictim;

    CameraFollow cameraFollow;

    [SerializeField]
    AudioClip scream;
    [SerializeField]
    AudioClip hitSound;

    float cooldown;
    float timerHit = 0.3f;
    bool hit;

    int ponyIRight, ponyILeft, ponyIMain, hunterI, uniI, pegI;

    float cooldownScream;
    float cooldownNotScream;
    bool screaming;

    bool onetimeHitSound;

    [SerializeField]
    AudioClip[] Walk;
    float stepTimer;
    int stepI;


    private void Start()
    {
        InitiateAnimal();
        cooldownScream = 1.6f;
        cameraFollow = Camera.GetComponent<CameraFollow>();
        deadTimer = 2f;
    }

    void Hit()
    {
        if (tempVictim != null)
        {
            if (transform.position.x < tempVictim.transform.position.x - 0.2f)
            {
                if (SR.flipX) SR.flipX = false;
            }
            else if (transform.position.x > tempVictim.transform.position.x + 0.2f)
            {
                if (!SR.flipX) SR.flipX = true;
            }

            cooldown = 1.5f;
            tempVictim.Hitted(damage, !SR.flipX);
        }
    }

    void CheckVictim()
    {
        if (Vector2.Distance(Player.transform.position, transform.position) < 15f) //проверяем игрока
            if (Player.health > 0)
            {
                tempVictim = Player;
            }
        if (earthponiesSpawnRight.Ponies.Count > 0) //проверяем лагерь справа
        {
            if (ponyIRight < earthponiesSpawnRight.Ponies.Count - 1) ponyIRight++;
            else ponyIRight = 0;
            if (Vector2.Distance(earthponiesSpawnRight.Ponies[ponyIRight].transform.position, transform.position) < 15f)
                if (tempVictim == null)
                    if (earthponiesSpawnRight.Ponies[ponyIRight].health > 0 && earthponiesSpawnRight.PoniesWalk[ponyIRight])
                        tempVictim = earthponiesSpawnRight.Ponies[ponyIRight];
        }
        if (earthponiesSpawnLeft.Ponies.Count > 0) //проверяем лагерь слева
        {
            if (ponyILeft < earthponiesSpawnLeft.Ponies.Count - 1) ponyILeft++;
            else ponyILeft = 0;
            if (Vector2.Distance(earthponiesSpawnLeft.Ponies[ponyILeft].transform.position, transform.position) < 15f)
                if (tempVictim == null)
                    if (earthponiesSpawnLeft.Ponies[ponyILeft].health > 0 && earthponiesSpawnLeft.PoniesWalk[ponyILeft])
                        tempVictim = earthponiesSpawnLeft.Ponies[ponyILeft];
        }
        if (mainSpawn.Ponies.Count > 0) //проверяем лагерь игрока
        {
            if (ponyIMain < mainSpawn.Ponies.Count - 1) ponyIMain++;
            else ponyIMain = 0;
            //проверяем обычных пней
            if (Vector2.Distance(mainSpawn.Ponies[ponyIMain].transform.position, transform.position) < 15f)
                if (tempVictim == null)
                    if (mainSpawn.Ponies[ponyIMain].health > 0 && mainSpawn.PoniesWalk[ponyIMain])
                        tempVictim = mainSpawn.Ponies[ponyIMain];
        }
        if (mainSpawn.Hunters.Count > 0)
        {
            if (hunterI < mainSpawn.Hunters.Count - 1) hunterI++;
            else hunterI = 0;
            //проверяем охотников отдельно от обычных пней
            if (Vector2.Distance(mainSpawn.Hunters[hunterI].transform.position, transform.position) < 15f)
                if (tempVictim == null)
                    if (mainSpawn.Hunters[hunterI].health > 0 && mainSpawn.Hunters[hunterI])
                        tempVictim = mainSpawn.Hunters[hunterI];

        }
        if (pegaSpawn.pegasus.Count > 0)
        {
            if (pegI < pegaSpawn.pegasus.Count - 1) pegI++;
            else pegI = 0;
            //проверяем пегасов
            if (Vector2.Distance(pegaSpawn.pegasus[pegI].transform.position, transform.position) < 15f)
                if (tempVictim == null)
                    if (pegaSpawn.pegasus[pegI].health > 0)
                        if (pegaSpawn.pegasus[pegI].transform.position.x > RightWall.transform.position.x || RightWall.health <= 0)
                            tempVictim = pegaSpawn.pegasus[pegI];
                        else destroyWall = true;
        }
        if (UniSpawn.unicorns.Count > 0)
        {
            if (uniI < UniSpawn.unicorns.Count - 1) uniI++;
            else uniI = 0;
            //проверяем юникорнов
            if (Vector2.Distance(UniSpawn.unicorns[uniI].transform.position, transform.position) < 15f)
                if (tempVictim == null)
                    if (UniSpawn.unicorns[uniI].health > 0 && UniSpawn.unicorns[uniI])
                        tempVictim = UniSpawn.unicorns[uniI];
        }
    }

    public override void Dead()
    {
        if (deadTimer > 0)
        {
            anim.SetBool("dead", true);
            deadTimer -= Time.deltaTime;
        }
        else
        {
            GameObject deadParticles = PoolManager.getGameObjectFromPool(DeadParticles);
            deadParticles.transform.position = transform.position;
            deadParticles.GetComponent<DeadParticles>().gameManager = gameManager;
            deadParticles.GetComponent<DeadParticles>().timerDissapear = 1f;
            gameObject.SetActive(false);
        }
    }

    void ShakeCamera()
    {
        float dist = Vector2.Distance(transform.position, Player.transform.position);
        float dis = 0;
        if (dist > 0)
            dis = 1 / Vector2.Distance(transform.position, Player.transform.position);
        else
            dis = 1;
        if (health > 0)
        {
            if (!screaming)
            {
                if (Mathf.Abs(speedX) > 0)
                {
                    cameraFollow.rangeShake = 0.6f * dis;
                    cameraFollow.speedShake = 5f * dis;
                }
                else cameraFollow.rangeShake = cameraFollow.speedShake = 0;
            }
            else
            {
                cameraFollow.rangeShake = 4.5f * dis;
                cameraFollow.speedShake = 66f * dis;
            }
        }
        else
        {
            cameraFollow.rangeShake = cameraFollow.speedShake = 0;
        }
    }

    void SoundSteps()
    {
        _audi.volume = gameManager.soundVolume;

        float spd = Mathf.Abs(speedX);
        if (spd > 0)
        {
            //играем звуки шагов
            if (stepTimer > 0)
            {
                stepTimer -= Time.deltaTime;
            }
            else
            {
                _audi.PlayOneShot(Walk[stepI]);
                stepTimer = 0.5f;
                if (stepI < Walk.Length - 1) stepI++;
                else stepI = 0;
            }
        }
    }

    private void Update()
    {
        CheckVictim();
        ShakeCamera();
        rigbody.velocity = new Vector2(speedX, speedY);
        if (Mathf.Abs(speedX) > 0) anim.SetBool("walk", true);
        else anim.SetBool("walk", false);
        SoundSteps();

        if (cooldown > 0) //высчитываем отдельно кулдаут
        {
            cooldown -= Time.deltaTime;
        }

        if (hit) //задержка для укусов
        {
            if (timerHit > 0)
            {
                if(!onetimeHitSound)
                {
                    _audi.PlayOneShot(hitSound);
                    onetimeHitSound = true;
                }

                timerHit -= Time.deltaTime;
            }
            else
            {
                onetimeHitSound = false;
                Hit();
                timerHit = 0.3f;
                hit = false;
            }
        }

        if (health > 0)
        {
            if (!isHitted)
            {
                if (tempVictim == null) //если жертв нет, он идет до левой стены и разворачивается
                {
                    if (!goBack)
                    {
                        if (transform.position.x > placeToGO)
                        {
                            maxSpeed = 3.5f;
                            Running(-maxSpeed);
                        }
                        else goBack = true;
                    }
                    else
                    {
                        if (transform.position.x < placeToGoBack)
                        {
                            maxSpeed = 3.5f;
                            Running(maxSpeed);
                        }
                    }
                }
                else
                {
                    if (cooldownNotScream > 0)
                    {
                        cooldownNotScream -= Time.deltaTime;
                        cooldownScream = 1.6f;

                        if (destroyWall) //если медведь бежал за игроком и встретил стену
                        {
                            if (RightWall.health > 0)
                            {
                                float dist = Vector2.Distance(transform.position, RightWall.transform.position);
                                if (dist < 10f && dist > 3f && cooldown <= 0)
                                {
                                    SR.flipX = false;
                                    cooldown = 1.5f;
                                    anim.SetTrigger("hit");
                                    RightWall.health -= damage;
                                }
                                else if (dist < 3f)
                                {
                                    Running(4);
                                }
                                else speedX = speedY = 0;
                            }
                            else destroyWall = false;
                            if (tempVictim != Player || Player.transform.position.x > RightWall.transform.position.x) destroyWall = false;
                        }
                        else
                        {
                            float dist = Vector2.Distance(transform.position, tempVictim.transform.position);

                            if (RightWall.health > 0 && Vector2.Distance(transform.position, RightWall.transform.position) < 5f)
                                destroyWall = true;

                            if (dist >= 9.5f && cooldown < 0.6f) //бежим за жервой, если она далеко
                            {
                                maxSpeed = 3.5f;
                                RunTo(tempVictim.transform.position);
                            }
                            else if (dist < 9.5f && cooldown < 0.1f) //бьем, если она стоит вплотную
                            {
                                speedX = 0;
                                if (!hit)
                                {
                                    anim.SetTrigger("hit");
                                    hit = true;
                                }
                            }
                            else if (dist < 9f && cooldown < 0.6f) //идем, если она близко
                            {
                                if (Mathf.Abs(maxSpeed) != 3f)
                                {
                                    if (tempVictim.transform.position.x > transform.position.x)
                                        maxSpeed = 3.5f;
                                    else
                                        maxSpeed = -3.5f;
                                }
                                Running(maxSpeed);
                            }
                        }
                        if (Vector2.Distance(transform.position, tempVictim.transform.position) > 19f ||
                            !tempVictim.gameObject.activeSelf || tempVictim.health <= 0)
                        {
                            tempVictim = null;
                            return; //нужно, чтоб он дальше не проверял у пустой жертвы уровень здоровья
                        }
                    }
                    else
                    {
                        if (cooldownScream > 0)
                        {
                            maxSpeed = 0;
                            cooldownScream -= Time.deltaTime;
                            if (!screaming)
                            {
                                _audi.PlayOneShot(scream);
                                anim.SetTrigger("scream");
                                screaming = true;
                            }
                        }
                        else
                        {
                            cooldownNotScream = Random.Range(10f, 42f);
                            screaming = false;
                        }
                    }
                }
            }
        }
        else
        {
            rigbody.velocity = Vector2.zero;
            Dead();
        }
    }
}