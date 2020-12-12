using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manticore : Animal
{
    [Header("Это тимерфульв, если что с:")]
    public float myMaxVolume;

    public bool destroyWall;
    public WallBuild LeftWall;

    public Creature Player;
    public SpriteRenderer EyesGlowSR;
    public Animator EyesGlowAnimator;
    public RabbitSpawn rabbitSpawn;
    public EarthponiesCamp earthponiesSpawn;
    public EarthponiesCamp mainSpawn;
    public TimeCount time;
    public int damage;

    public AudioClip[] wolfWalk;
    float walkTimer;
    int walkI;

    public AudioClip wolfHit;
    public AudioClip wolfHitJump;
    public AudioClip wolfHitted;

    public Creature tempVictim;

    float cooldown;
    float timerJump = 0.6f;
    float timerHit = 0.7f;
    bool hit;
    int sideJump = 0;
    bool jump;

    int rabbitI, ponyI, hunterI;


    private void Start()
    {
        InitiateAnimal();
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
            anim.SetTrigger("hit");
            EyesGlowAnimator.SetTrigger("hit");
            tempVictim.Hitted(damage, !SR.flipX);
        }
    }

    void CheckVictim()
    {
        if (transform.position.x < maxX + 15f)
        {
            if (Vector2.Distance(Player.transform.position, transform.position) < 12f) //проверяем игрока
                if (Player.health > 0)
                {
                    tempVictim = Player;
                }

            if (rabbitSpawn.rabbits.Count > 0) //проверяем кроликов
            {
                if (rabbitI < rabbitSpawn.rabbits.Count - 1) rabbitI++;
                else rabbitI = 0;
                if (Vector2.Distance(rabbitSpawn.rabbits[rabbitI].transform.position, transform.position) < 13f)
                    if (tempVictim == null)
                        if (rabbitSpawn.rabbits[rabbitI].health > 0)
                            tempVictim = rabbitSpawn.rabbits[rabbitI];
            }
            if (earthponiesSpawn.Ponies.Count > 0) //проверяем лагерь рядом с лесом
            {
                if (ponyI < earthponiesSpawn.Ponies.Count - 1) ponyI++;
                else ponyI = 0;
                if (Vector2.Distance(earthponiesSpawn.Ponies[ponyI].transform.position, transform.position) < 13f)
                    if (tempVictim == null)
                        if (earthponiesSpawn.Ponies[ponyI].health > 0 && earthponiesSpawn.PoniesWalk[ponyI])
                            tempVictim = earthponiesSpawn.Ponies[ponyI];
            }
            if (mainSpawn.Ponies.Count > 0) //проверяем лагерь игрока
            {
                if (ponyI < mainSpawn.Ponies.Count - 1) ponyI++;
                else ponyI = 0;
                //проверяем обычных пней
                if (Vector2.Distance(mainSpawn.Ponies[ponyI].transform.position, transform.position) < 13f)
                    if (tempVictim == null)
                        if (mainSpawn.Ponies[ponyI].health > 0 && mainSpawn.PoniesWalk[ponyI])
                            tempVictim = mainSpawn.Ponies[ponyI];
            }
            if (mainSpawn.Hunters.Count > 0)
            {
                if (hunterI < mainSpawn.Hunters.Count - 1) hunterI++;
                else hunterI = 0;
                //проверяем охотников отдельно от обычных пней
                if (Vector2.Distance(mainSpawn.Hunters[hunterI].transform.position, transform.position) < 13f)
                    if (tempVictim == null)
                        if (mainSpawn.Hunters[hunterI].health > 0 && !mainSpawn.Hunters[hunterI].onTower)
                            tempVictim = mainSpawn.Hunters[hunterI];

            }
        }
    }

    void JumpTo(Vector2 target)
    {
        if (sideJump == 0)
        {
            if (transform.position.x < target.x)
            {
                sideJump = 1;
                if (SR.flipX) SR.flipX = false;
            }
            else
            {
                sideJump = -1;
                if (!SR.flipX) SR.flipX = true;
            }
        }
        if (timerJump > 0)
        {
            speedX = maxSpeed * sideJump;
            timerJump -= Time.deltaTime;
        }
        else
        {
            speedX = 0f;
            timerJump = 0.6f;
            sideJump = 0;
            jump = false;
        }
    }

    public override void Dead()
    {
        if (deadTimer > 0)
        {
            anim.SetBool("dead", true);
            EyesGlowAnimator.SetBool("dead", true);
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

    public override void Hitted(int damage, bool leftSide)
    {
        _audi.PlayOneShot(wolfHitted);
        health -= damage;
        flyTime = 0.4f;
        if (leftSide)
        {
            speedX = 3f;
        }
        else
        {
            speedX = -3f;
        }
        redTime = 0.2f;
        EyesGlowAnimator.SetTrigger("hit");
        anim.SetTrigger("hit");
        stanned = 0.6f;
    }

    public void Hitted(int damage, bool leftSide, Creature hunter)
    {
        Hitted(damage,leftSide);
        tempVictim = hunter;
    }

    void SoundSteps()
    {
        _audi.volume = myMaxVolume * gameManager.soundVolume;

        float spd = Mathf.Abs(speedX);
        if (spd > 0 && spd < 4)
        {
            //играем звуки шагов
            if (walkTimer > 0)
            {
                walkTimer -= Time.deltaTime;
            }
            else
            {

                _audi.PlayOneShot(wolfWalk[walkI]);
                walkTimer = 0.4f;
                if (walkI < wolfWalk.Length - 1) walkI++;
                else walkI = 0;

            }
        }
        else if (spd > 4)
        {
            //играем звуки бега
            if (walkTimer > 0)
            {
                walkTimer -= Time.deltaTime;
            }
            else
            {

                _audi.PlayOneShot(wolfWalk[walkI]);
                walkTimer = 0.25f;
                if (walkI < wolfWalk.Length - 1) walkI++;
                else walkI = 0;

            }
        }
    }


    private void Update()
    {
        CheckVictim();
        rigbody.velocity = new Vector2(speedX, speedY);
        anim.SetFloat("speed", Mathf.Abs(speedX));
        EyesGlowAnimator.SetFloat("speed", Mathf.Abs(speedX));
        EyesGlowSR.flipX = SR.flipX;
        SoundSteps();


        if (cooldown > 0) //высчитываем отдельно кулдаут
        {
            cooldown -= Time.deltaTime;
        }

        if (time.hours > 22 || time.hours < 5) //высчитываем отдельно максимум, где может гулять тимберфульф
        {
            maxX = -130f;
        }
        else
        {
            maxX = -179.3f;
        }

        if (hit) //задержка для укусов
        {
            if (timerHit > 0)
            {
                timerHit -= Time.deltaTime;
            }
            else
            {
                _audi.PlayOneShot(wolfHit);
                Hit();
                timerHit = 0.7f;
                hit = false;
            }
        }

        if (health > 0)
        {
            if (!isHitted)
            {
                if (tempVictim == null) //если жертв нет, он гуляет
                {
                    maxSpeed = 4f;
                    if (transform.position.x > maxX + 5f) //если волк зашел далеко от леса
                    {
                        Running(-maxSpeed);
                    }
                    else //если он в лесу
                    {
                        GoSomewhere();
                    }
                }
                else //если жертва есть
                {
                    if (jump) //если волк решает прыгнуть
                    {
                        JumpTo(tempVictim.transform.position);
                    }
                    else //если волк преследует жертву
                    {
                        if (destroyWall) //если волк бежал за игроком и встретил стену
                        {
                            if (maxX == -130f) //если сейчас ночь
                            {
                                if (LeftWall.health > 0)
                                {
                                    float dist = Vector2.Distance(transform.position, LeftWall.transform.position);
                                    if (dist < 5f && dist > 3f && cooldown <= 0)
                                    {
                                        SR.flipX = false;
                                        cooldown = 1.5f;
                                        anim.SetTrigger("hit");
                                        EyesGlowAnimator.SetTrigger("hit");
                                        LeftWall.health -= damage;
                                    }
                                    else if (dist < 3f)
                                    {
                                        Running(-4);
                                    }
                                    else speedX = speedY = 0;
                                }
                                else destroyWall = false;
                            }
                            else destroyWall = false;
                            if (tempVictim != Player || Player.transform.position.x < LeftWall.transform.position.x) destroyWall = false;
                        }
                        else
                        {
                            float dist = Vector2.Distance(transform.position, tempVictim.transform.position);

                            if (LeftWall.health > 0 && Vector2.Distance(transform.position, LeftWall.transform.position) < 5f)
                                destroyWall = true;

                            if (dist >= 7f && cooldown < 0.6f) //бежим за жервой, если она далеко
                            {
                                maxSpeed = 9f;
                                RunTo(tempVictim.transform.position);
                            }
                            else if (dist >= 6f && dist < 7f && cooldown < 0.1f) //прыгаем, если она на нужном расстоянии
                            {
                                maxSpeed = 10f;
                                Hit();
                                _audi.PlayOneShot(wolfHitJump);
                                jump = true;
                            }
                            else if (dist < 5.5f && cooldown < 0.1f) //бьем, если она стоит вплотную
                            {
                                speedX = 0;
                                hit = true;
                               
                            }
                            else if (dist < 5.5f && cooldown < 0.6f) //идем, если она близко
                            {
                                if (Mathf.Abs(maxSpeed) != 4.5f)
                                {
                                    if (tempVictim.transform.position.x > transform.position.x)
                                        maxSpeed = 4.5f;
                                    else
                                        maxSpeed = -4.5f;
                                }
                                Running(maxSpeed);
                            }
                        }
                        if (Vector2.Distance(transform.position, tempVictim.transform.position) > 16f ||
                            transform.position.x > maxX + 42f)
                        {
                            tempVictim = null;
                            return; //нужно, чтоб он дальше не проверял у пустой жертвы уровень здоровья
                        }
                        if (tempVictim.health <= 0) tempVictim = null;
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