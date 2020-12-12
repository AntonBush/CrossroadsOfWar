using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [HideInInspector]
    public Creature Hunter;
    [HideInInspector]
    public Manticore Timberwolf1;
    [HideInInspector]
    public Manticore Timberwolf2;
    [HideInInspector]
    public UrsaMinor Ursa;
    [HideInInspector]
    public UnicornsSpawn unicornSpawn;
    [HideInInspector]
    public PegasusSpawn pegasusSpawn;
    public float timer = 5f;

    [SerializeField]
    AudioClip ArrowHit;

    AudioSource _audi;
    Rigidbody2D rigbody;
    bool leftSide;

    int uniI, pegI;

    bool soundHit, onetimeSound;

    Transform tempVictim;

    GameManager gameManager;

    private void Start()
    {
        rigbody = GetComponent<Rigidbody2D>();
        _audi = GetComponent<AudioSource>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void HitEnemy(Creature Enemy)
    {
        timer *= 0.5f;
        if (Enemy.transform.position.x < transform.position.x) leftSide = false;
        else leftSide = true;

        Enemy.Hitted(15, leftSide);
        if (Hunter.name == "Player" && Enemy.health <= 0)
        {
            gameManager.killsCount++;
            if (Enemy.name.Contains("Unicorn")) gameManager.unikillsCount++;
            if (Enemy.name.Contains("Pegasus")) gameManager.pegakillsCount++;
        }
        tempVictim = Enemy.transform;
        soundHit = true;
        //PoolManager.putGameObjectToPool(gameObject);
    }

    void HitTimber(Manticore timberWolf)
    {
        timer *= 0.5f;
        if (timberWolf.transform.position.x < transform.position.x) leftSide = false;
        else leftSide = true;

        tempVictim = timberWolf.transform;
        timberWolf.Hitted(15, leftSide, Hunter);
        if (Hunter.name == "Player" && timberWolf.health <= 0)
        {
            gameManager.killsCount++;
            gameManager.timberkillsCount++;
        }
        soundHit = true;
        //PoolManager.putGameObjectToPool(gameObject);
    }

    void HitUniSpawner()
    {
        timer *= 0.5f;
        leftSide = true;
        unicornSpawn.HitSpawner(15);
        tempVictim = unicornSpawn.transform;
        soundHit = true;
    }

    void CheckCloseEnemy()
    {
        if (Timberwolf1.health > 0 && Vector2.Distance(transform.position, Timberwolf1.transform.position) < 2f)
        {
            HitTimber(Timberwolf1);
        }
        if (Timberwolf2.health > 0 && Vector2.Distance(transform.position, Timberwolf2.transform.position) < 2f)
        {
            HitTimber(Timberwolf2);
        }
        if(Ursa.gameObject.activeSelf && Ursa.health > 0 && Vector2.Distance(transform.position, Ursa.transform.position) < 2f)
        {
            HitEnemy(Ursa);
        }
        if (unicornSpawn.unicorns.Count > 0)
        {
            if (uniI < unicornSpawn.unicorns.Count)
            {
                if (unicornSpawn.unicorns[uniI].health > 0 && Vector2.Distance(transform.position, unicornSpawn.unicorns[uniI].transform.position) < 2f)
                {
                    HitEnemy(unicornSpawn.unicorns[uniI]);
                }
            }
            else uniI = 0;
        }
        if (pegasusSpawn.pegasus.Count > 0)
        {
            if (pegI < pegasusSpawn.pegasus.Count)
            {
                if (pegasusSpawn.pegasus[pegI].health > 0 && Vector2.Distance(transform.position, pegasusSpawn.pegasus[pegI].transform.position) < 2f)
                {
                    HitEnemy(pegasusSpawn.pegasus[pegI]);
                }
            }
            else pegI = 0;
        }
        if (unicornSpawn.health > 0 && Vector2.Distance(transform.position, unicornSpawn.transform.position) < 2f)
        {
            HitUniSpawner();
        }

        if (uniI < unicornSpawn.unicorns.Count - 1) uniI++;
        else uniI = 0;

        if (pegI < pegasusSpawn.pegasus.Count - 1) pegI++;
        else pegI = 0;
    }

    public void SetNewArrow()
    {
        transform.parent = null;
        onetimeSound = false;
        soundHit = false;
        if (rigbody == null) rigbody = GetComponent<Rigidbody2D>();
        rigbody.isKinematic = false;
    }

    void Update()
    {
        if (!soundHit)
        {
            CheckCloseEnemy();
        }
        else
        {
            _audi.volume = Timberwolf1.gameManager.soundVolume; //волк нужен, только чтобы вытащить глобальную громкость
            if (!onetimeSound)
            {
                _audi.PlayOneShot(ArrowHit);
                onetimeSound = true;
            }
            rigbody.velocity = Vector2.zero;
            rigbody.isKinematic = true;
            transform.parent = tempVictim;
        }

        if (timer > 0)
        {
            if (rigbody.velocity.x > 0)
            {
                transform.localEulerAngles = new Vector3(0, 0, rigbody.velocity.y * 2);
            }
            else
            {
                transform.localEulerAngles = new Vector3(0, 0, -rigbody.velocity.y * 2);
            }
            timer -= Time.deltaTime;
        }
        else
        {
            PoolManager.putGameObjectToPool(gameObject);
        }
    }
}
