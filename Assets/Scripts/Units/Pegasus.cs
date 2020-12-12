using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pegasus : Creature
{
    public int foodCount;

    public GameObject FoodPrefab;

    public PegasusSpawn myHome;

    public TowerBuild leftTower;
    public TowerBuild rightTower;
    public WarehouseBuild warehouse;
    public EarthponiesCamp leftCamp;
    public EarthponiesCamp mainCamp;
    public UnicornsSpawn UniSpawn;
    public Creature ursa;
    public Creature Player;
    public float distanceSeeing = 13f;
    public float maxSpeed;
    public int damage;

    public Creature tempVictim;

    [SerializeField]
    AudioClip HitSound;

    [SerializeField]
    AudioClip WingSound;
    [SerializeField]
    float WingSoundCooldown;
    float wingSoundTimer;

    float tempPosY;
    bool onetimeHitted;

    [HideInInspector]
    public bool haveFood;
    [HideInInspector]
    public float timerGettingFood = 1.5f;
    [HideInInspector]
    public bool insideWarehouse;

    int flyLeft; //нужна для высчитывания направления полета от жертвы, чтоб оказаться на нужном расстоянии
    int huntingStage;

    int ponyI, hunterI, uniI;


    void CheckVictim()
    {
        if (Vector2.Distance(Player.transform.position, transform.position) < distanceSeeing) //проверяем игрока
            if (Player.health > 0)
            {
                tempVictim = Player;
            }

        if (leftCamp.Ponies.Count > 0) //проверяем лагерь рядом
        {
            if (ponyI < leftCamp.Ponies.Count - 1) ponyI++;
            else ponyI = 0;
            if (Vector2.Distance(leftCamp.Ponies[ponyI].transform.position, transform.position) < distanceSeeing)
                if (tempVictim == null)
                    if (leftCamp.Ponies[ponyI].health > 0 && leftCamp.PoniesWalk[ponyI])
                    {
                        tempVictim = leftCamp.Ponies[ponyI];
                    }
        }
        if (mainCamp.Ponies.Count > 0) //проверяем лагерь игрока
        {
            if (ponyI < mainCamp.Ponies.Count - 1) ponyI++;
            else ponyI = 0;
            //проверяем обычных пней
            if (Vector2.Distance(mainCamp.Ponies[ponyI].transform.position, transform.position) < distanceSeeing)
                if (tempVictim == null)
                    if (mainCamp.Ponies[ponyI].health > 0 && mainCamp.PoniesWalk[ponyI])
                        tempVictim = mainCamp.Ponies[ponyI];
        }
        if (mainCamp.Hunters.Count > 0)
        {
            if (hunterI < mainCamp.Hunters.Count - 1) hunterI++;
            else hunterI = 0;
            //проверяем охотников отдельно от обычных пней
            if (Vector2.Distance(mainCamp.Hunters[hunterI].transform.position, transform.position) < distanceSeeing)
                if (tempVictim == null)
                    if (mainCamp.Hunters[hunterI].health > 0 && mainCamp.Hunters[hunterI])
                        tempVictim = mainCamp.Hunters[hunterI];
        }
        if (UniSpawn.unicorns.Count > 0)
        {
            if (uniI < UniSpawn.unicorns.Count - 1) uniI++;
            else uniI = 0;
            //проверяем юникорнов
            if (Vector2.Distance(UniSpawn.unicorns[uniI].transform.position, transform.position) < distanceSeeing)
                if (tempVictim == null)
                    if (UniSpawn.unicorns[uniI].health > 0 && UniSpawn.unicorns[uniI])
                        tempVictim = UniSpawn.unicorns[uniI];
        }
        if (ursa.gameObject.activeSelf && ursa.health > 0 && Vector2.Distance(ursa.transform.position, transform.position) < distanceSeeing)
        { //проверяем урсу
            tempVictim = ursa;
        }
        if (leftTower.myHunter != null)
        { //проверяем пня на левой башне
            if (Vector2.Distance(leftTower.myHunter.transform.position, transform.position) < distanceSeeing)
            {
                tempVictim = leftTower.myHunter;
            }
        }
        if (rightTower.myHunter != null)
        {  //проверяем пня на правой башне
            if (Vector2.Distance(rightTower.myHunter.transform.position, transform.position) < distanceSeeing)
            {
                tempVictim = rightTower.myHunter;
            }
        }
    }

    public override void Dead()
    {
        insideWarehouse = false;
        if (transform.position.y > -1)
        {
            speedY = -12;
            anim.SetBool("dead", true);
        }
        else
        {
            speedY = 0;
            if (foodCount > 0)
            {
                Item newWood = PoolManager.getGameObjectFromPool(FoodPrefab).GetComponent<Item>();
                newWood.GetComponent<SpriteRenderer>().sprite = newWood.foodSprite;
                newWood.player = Player.transform;
                newWood.transform.position = new Vector2(transform.position.x, transform.position.y - 0.2f);
                newWood.foodCount = foodCount;
                warehouse.resourses.SaveItems.Add(newWood);
                foodCount = 0;
            }

            if (deadTimer > 0)
            {
                anim.SetBool("dead", true);
                deadTimer -= Time.deltaTime;
            }
            else
            {
                myHome.pegasus.Remove(this);
                GameObject deadParticles = PoolManager.getGameObjectFromPool(DeadParticles);
                deadParticles.transform.position = transform.position;
                deadParticles.GetComponent<DeadParticles>().gameManager = gameManager;
                deadParticles.GetComponent<DeadParticles>().timerDissapear = 1f;
                PoolManager.putGameObjectToPool(gameObject);
            }
        }
    }

    bool FlyTo(Vector2 destination, float distance)
    {
        if (transform.position.y > positionY + 0.2f) speedY = -maxSpeed / 1.5f;
        else if (transform.position.y < positionY - 0.2f) speedY = +maxSpeed / 1.5f;
        else speedY = 0;

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

    bool FlyFrom(Vector2 destination, float distance)
    {
        if (transform.position.y > positionY + 0.2f) speedY = -maxSpeed / 2f;
        else if (transform.position.y < positionY - 0.2f) speedY = +maxSpeed / 2f;
        else speedY = 0;

        if (flyLeft == 0)
        {
            if (transform.position.x < destination.x) flyLeft = -1;
            else flyLeft = 1;
        }
        else
        {
            if (flyLeft == -1)
            {
                if (transform.position.x > destination.x - distance)
                {
                    speedX = -maxSpeed;
                    if (!SR.flipX) SR.flipX = true;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (transform.position.x < destination.x + distance)
                {
                    speedX = maxSpeed;
                    if (SR.flipX) SR.flipX = false;
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }

    void Hit()
    {
        _audi.PlayOneShot(HitSound);
        tempVictim.Hitted(damage, !SR.flipX);
    }

    bool FlyTo(Vector2 destination, float distance, float distanceLanding)
    {
        if (transform.position.x > destination.x - distanceLanding && transform.position.x < destination.x + distanceLanding)
        {
            if (transform.position.y > destination.y + 0.2f) speedY = -maxSpeed / 1.5f;
            else if (transform.position.y < destination.y - 0.2f) speedY = +maxSpeed / 1.5f;
            else speedY = 0;
        }
        else
        {
            if (transform.position.y > positionY + 0.2f) speedY = -maxSpeed / 1.5f;
            else if (transform.position.y < positionY - 0.2f) speedY = +maxSpeed / 1.5f;
            else speedY = 0;
        }

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

    public override bool isHitted
    {
        get
        {
            if (flyTime > 0.25f)
            {
                if (!onetimeHitted)
                {
                    tempPosY = transform.position.y;
                    onetimeHitted = true;
                }
                speedY = 6f;
            }
            else if (flyTime > 0)
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
                if (onetimeHitted)
                {
                    transform.position = new Vector3(transform.position.x, tempPosY, transform.position.z);
                    onetimeHitted = false;
                    flyTime = 0;
                    speedX = speedY = 0f;
                }
                return false;
            }
        }
    }

    void SoundWing()
    {
        _audi.volume = gameManager.soundVolume;
        if (wingSoundTimer > 0)
        {
            wingSoundTimer -= Time.deltaTime;
        }
        else
        {
            _audi.PlayOneShot(WingSound);
            wingSoundTimer = WingSoundCooldown;
        }
    }

    private void Start()
    {
        InitiateCreature();
        positionY = transform.position.y;
        timerGettingFood = 1.5f;
    }

    private void Update()
    {
        anim.SetBool("pegasus", true);
        rigbody.velocity = new Vector2(speedX, speedY);
        anim.SetFloat("speed", Mathf.Abs(speedX));

        if (health > 0)
        {
            SoundWing();
            if (!isHitted)
            {
                CheckVictim();

                if (tempVictim == null)
                {
                    if (haveFood)
                    {
                        insideWarehouse = false;
                        maxSpeed = 12f;
                        if (FlyTo(myHome.transform.position, 2f, 999f))
                        {
                            myHome.pegasus.Remove(this);
                            PoolManager.putGameObjectToPool(gameObject);
                        }
                    }
                    else
                    {
                        maxSpeed = 8f;
                        if (FlyTo(warehouse.transform.position, 2f, 8f))
                        {
                            if (timerGettingFood > 0)
                            {
                                insideWarehouse = true;
                                timerGettingFood -= Time.deltaTime;
                            }
                            else
                            {
                                if (warehouse.resourses.Food > 10)
                                {
                                    foodCount = 10;
                                    warehouse.resourses.AddResourses(0, -10);
                                }
                                else
                                {
                                    foodCount = warehouse.resourses.Food;
                                    warehouse.resourses.Food = 0;
                                    warehouse.resourses.UpdateResourses();
                                }
                                haveFood = true;
                                insideWarehouse = false;
                            }
                        }
                    }
                }
                else
                {
                    insideWarehouse = false;
                    //hunting
                    if (tempVictim.gameObject.activeSelf && Vector2.Distance(transform.position, tempVictim.transform.position) < 16f
                    && tempVictim.health > 0)
                    {
                        if (huntingStage == 0)
                        {
                            maxSpeed = 12f;

                            if (FlyFrom(tempVictim.transform.position, 12f))
                            {
                                huntingStage++;
                            }
                        }
                        if (huntingStage == 1)
                        {
                            maxSpeed = 19f;
                            if (FlyTo(tempVictim.transform.position, 0.5f, 25f))
                            {
                                Hit();
                                if (flyLeft == -1) flyLeft = 1;
                                else flyLeft = -1;
                                huntingStage++;
                            }
                        }
                        if (huntingStage == 2)
                        {
                            maxSpeed = 12f;
                            if (FlyFrom(tempVictim.transform.position, 12f))
                            {
                                flyLeft = 0;
                                huntingStage = 0;
                            }
                        }
                    }
                    else tempVictim = null;
                }
            }
        }
        else
        {
            redTime = 0;
            speedX = 0;
            Dead();
            //dead
        }
    }
}
