using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unicorn : Creature
{
    [Header("UnicornItems")]
    public GameObject FoodPrefab;
    bool destroyWall;
    public WallBuild RightWall;
    public WarehouseBuild warehouse;
    public EarthponiesCamp rightCamp;
    public EarthponiesCamp mainCamp;
    public Creature Player;
    public Creature ursa;

    public PegasusSpawn pegaSpawn;
    public float distanceSeeing = 13f;
    public float maxSpeed;
    public int damage;

    public Creature tempVictim;

    public Sprite Weapon;
    public Sprite Food;

    public UnicornItem uniItem;

    public UnicornsSpawn myHome;

    [Header("Sound")]
    public AudioClip[] WalkSlow;
    public AudioClip[] WalkFast;
    public AudioClip[] Run;
    public AudioClip[] snowWalkSlow;
    public AudioClip[] snowWalkFast;
    public AudioClip[] snowRun;

    public AudioClip SwordTryHit;
    public AudioClip SwordHit;

    float cooldown;

    [HideInInspector]
    public bool hasFood;

    public bool insideWarehouse { get; private set; }

    [HideInInspector]
    public float timerGettingFood = 1.5f;
    float timerHit;
    bool makeHit;
    [HideInInspector]
    public int foodIHave;

    int ponyI, hunterI, pegI;

    float stepTimer;
    int stepI;

    bool tryHitSound;

    public override void Dead()
    {
        insideWarehouse = false;
        uniItem.gameObject.SetActive(false);
        if (foodIHave > 0)
        {
            Item newWood = PoolManager.getGameObjectFromPool(FoodPrefab).GetComponent<Item>();
            newWood.GetComponent<SpriteRenderer>().sprite = newWood.foodSprite;
            newWood.player = Player.transform;
            newWood.transform.position = new Vector2(transform.position.x, transform.position.y - 0.2f);
            newWood.foodCount = foodIHave;
            warehouse.resourses.SaveItems.Add(newWood);
            foodIHave = 0;
        }

        if (deadTimer > 0)
        {
            anim.SetBool("dead", true);
            deadTimer -= Time.deltaTime;
        }
        else
        {
            myHome.unicorns.Remove(this);
            GameObject deadParticles = PoolManager.getGameObjectFromPool(DeadParticles);
            deadParticles.transform.position = transform.position;
            deadParticles.GetComponent<DeadParticles>().gameManager = gameManager;
            deadParticles.GetComponent<DeadParticles>().timerDissapear = 1f;
            PoolManager.putGameObjectToPool(gameObject);
        }
    }

    public override void Hitted(int damage, bool leftSide)
    {
        health -= damage;
        flyTime = 0.4f;
        if (leftSide) speedX = 3f;
        else speedX = -3f;
        redTime = 0.2f;
        stanned = 0.6f;
    }

    void CheckVictim()
    {
        if (Vector2.Distance(Player.transform.position, transform.position) < distanceSeeing) //проверяем игрока
            if (Player.health > 0)
            {
                if (Player.transform.position.x > RightWall.transform.position.x || RightWall.health <= 0)
                    tempVictim = Player;
                else destroyWall = true;
            }

        if (rightCamp.Ponies.Count > 0) //проверяем лагерь рядом
        {
            if (ponyI < rightCamp.Ponies.Count - 1) ponyI++;
            else ponyI = 0;
            if (Vector2.Distance(rightCamp.Ponies[ponyI].transform.position, transform.position) < distanceSeeing)
                    if (rightCamp.Ponies[ponyI].health > 0 && rightCamp.PoniesWalk[ponyI])
                    {
                        if (rightCamp.Ponies[ponyI].transform.position.x > RightWall.transform.position.x || RightWall.health <= 0)
                            tempVictim = rightCamp.Ponies[ponyI];
                        else destroyWall = true;
                    }
        }
        if (mainCamp.Ponies.Count > 0) //проверяем лагерь игрока
        {
            if (ponyI < mainCamp.Ponies.Count - 1) ponyI++;
            else ponyI = 0;
            //проверяем обычных пней
            if (Vector2.Distance(mainCamp.Ponies[ponyI].transform.position, transform.position) < distanceSeeing)
                    if (mainCamp.Ponies[ponyI].health > 0 && mainCamp.PoniesWalk[ponyI])
                        if (mainCamp.Ponies[ponyI].transform.position.x > RightWall.transform.position.x || RightWall.health <= 0)
                            tempVictim = mainCamp.Ponies[ponyI];
                        else destroyWall = true;
        }
        if (mainCamp.Hunters.Count > 0)
        {
            if (hunterI < mainCamp.Hunters.Count - 1) hunterI++;
            else hunterI = 0;
            //проверяем охотников отдельно от обычных пней
            if (Vector2.Distance(mainCamp.Hunters[hunterI].transform.position, transform.position) < distanceSeeing)
                if (tempVictim == null)
                    if (mainCamp.Hunters[hunterI].health > 0 && mainCamp.Hunters[hunterI])
                        if (mainCamp.Hunters[hunterI].transform.position.x > RightWall.transform.position.x || RightWall.health <= 0)
                            tempVictim = mainCamp.Hunters[hunterI];
                        else destroyWall = true;

        }
        if (pegaSpawn.pegasus.Count > 0)
        {
            if (pegI < pegaSpawn.pegasus.Count - 1) pegI++;
            else pegI = 0;
            //проверяем пегасов
            if (Vector2.Distance(pegaSpawn.pegasus[pegI].transform.position, transform.position) < distanceSeeing)
                    if (pegaSpawn.pegasus[pegI].health > 0)
                        if (pegaSpawn.pegasus[pegI].transform.position.x > RightWall.transform.position.x || RightWall.health <= 0)
                            tempVictim = pegaSpawn.pegasus[pegI];
                        else destroyWall = true;
        }
        if (ursa.gameObject.activeSelf && ursa.health > 0 && Vector2.Distance(ursa.transform.position, transform.position) < distanceSeeing)
        { //проверяем урсу
            tempVictim = ursa;
        }
    }

    public bool RunTo(Vector2 destination, float distance)
    {
        if (transform.position.x < destination.x - distance)
        {
            speedX = maxSpeed;
            if (SR.flipX) SR.flipX = false;
        }
        else if (transform.position.x > destination.x + distance)
        {
            speedX = -maxSpeed;
            if (!SR.flipX) SR.flipX = true;
        }
        else
        {
            speedX = 0;
            return true;
        }
        return false;
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

            uniItem.useSword = true;

            cooldown = 1f;
            timerHit = 0.2f;
            makeHit = true;
        }
    }

    public override bool isHitted
    {
        get
        {
            if (flyTime > 0.25f)
            {
                speedY = 6f;
            }
            else
            {
                speedY = -6f;
            }

            if (redTime > 0)
            {
                redTime -= Time.deltaTime;
            }

            if (flyTime > 0)
            {
                flyTime -= 0.05f;
                return true;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, positionY, transform.position.z);
                flyTime = 0;
                speedX = speedY = 0f;
                return false;
            }
        }
    }

    void SoundSteps()
    {
        _audi.volume = gameManager.soundVolume;
        float spd = Mathf.Abs(speedX);
        bool snow = transform.position.x > 20;
        if (spd > 0 && spd < 4)
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
                    _audi.PlayOneShot(snowWalkSlow[stepI]);
                }
                else
                {
                    _audi.PlayOneShot(WalkSlow[stepI]);

                }
                stepTimer = 0.42f;
                if (stepI < WalkSlow.Length - 1) stepI++;
                else stepI = 0;

            }
        }
        if (spd > 3 && spd < 8)
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
                    _audi.PlayOneShot(snowWalkFast[stepI]);
                }
                else
                {
                    _audi.PlayOneShot(WalkFast[stepI]);

                }
                stepTimer = 0.35f;
                if (stepI < WalkSlow.Length - 1) stepI++;
                else stepI = 0;

            }
        }
        else if (spd >= 8)
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

    private void Start()
    {
        InitiateCreature();
    }

    private void Update()
    {
        rigbody.velocity = new Vector2(speedX, speedY);
        anim.SetFloat("speed", Mathf.Abs(speedX));
        SoundSteps();

        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        if (tempVictim != null)
        {
            if (makeHit)
            {
                if (timerHit > 0)
                {
                    if (!tryHitSound)
                    {
                        _audi.PlayOneShot(SwordTryHit);
                        
                        tryHitSound = true;
                    }
                    timerHit -= Time.deltaTime;
                }
                else
                {  
                    if (Vector2.Distance(transform.position, tempVictim.transform.position) < 4.5f)
                    {
                        _audi.PlayOneShot(SwordHit);
                        tempVictim.Hitted(damage, !SR.flipX);
                    }
                    tryHitSound = false;
                    makeHit = false;
                }
            }
        }

        if (health > 0)
        {
            if (!isHitted)
            {
                if (!hasFood)
                {
                    CheckVictim();

                    if (tempVictim == null || insideWarehouse)
                    {
                        if (destroyWall)
                        {
                            if (RightWall.health > 0)
                            {
                                if (RunTo(RightWall.transform.position, 4.5f))
                                {
                                    if (cooldown <= 0) //разрушаем стену
                                    {
                                        _audi.PlayOneShot(SwordTryHit);
                                        SR.flipX = true;
                                        uniItem.useSword = true;
                                        cooldown = 1f;
                                        timerHit = 0.2f;
                                        makeHit = true;
                                        RightWall.health -= damage;
                                    }
                                }
                            }
                            else //если стена разрушается
                            {
                                destroyWall = false;
                            }
                        }
                        else //если юникорн не разрушает стену
                        {
                            if (RightWall.health > 0 && Vector2.Distance(transform.position, RightWall.transform.position) < 5f)
                                destroyWall = true;

                            maxSpeed = 4f;
                            if (RunTo(warehouse.transform.position, 0.2f))
                            {
                                if (timerGettingFood > 0)
                                {
                                    insideWarehouse = true;
                                    uniItem.gameObject.SetActive(false);
                                    timerGettingFood -= Time.deltaTime;
                                }
                                else
                                {
                                    if (warehouse.resourses.Food > 10)
                                    {
                                        foodIHave = 10;
                                        warehouse.resourses.AddResourses(0, -10);
                                        uniItem.SR.sprite = Food;
                                    }
                                    else
                                    {
                                        foodIHave = warehouse.resourses.Food;
                                        warehouse.resourses.Food = 0;
                                        warehouse.resourses.UpdateResourses();
                                    }
                                    if (foodIHave > 0)
                                    {
                                        uniItem.gameObject.SetActive(true);
                                        uniItem.SR.sprite = Food;
                                    }
                                    insideWarehouse = false;
                                    hasFood = true;
                                }
                            }
                        }
                    }
                    else //если юникорн обнаружил врага
                    {

                        float dis = Vector2.Distance(tempVictim.transform.position, transform.position);

                        if (RightWall.health > 0 && tempVictim.transform.position.x < RightWall.transform.position.x)
                        {
                            destroyWall = true;
                            tempVictim = null;
                            return;
                        }

                        if (dis < 4.5f && cooldown <= 0)
                        {
                            Hit();
                        }
                        if (tempVictim.health > 0 && tempVictim.gameObject.activeSelf && dis < (distanceSeeing + 2f))
                        {
                            maxSpeed = 7.5f;
                            RunTo(tempVictim.transform.position, 4.5f);
                        }
                        else
                        {
                            tempVictim = null;
                        }

                    }
                }
                else
                {
                    maxSpeed = 8f;
                    if (RunTo(myHome.transform.position, 1f))
                    {
                        myHome.unicorns.Remove(this);
                        PoolManager.putGameObjectToPool(gameObject);
                    }
                }
            }
        }
        else
        {
            redTime = 0;
            speedY = 0;
            speedX = 0;
            Dead();
        }
    }
}
