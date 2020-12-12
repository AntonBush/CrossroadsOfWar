using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTest : MonoBehaviour
{

    public Manticore Timberwolf1;
    public Manticore Timberwolf2;
    public UrsaMinor Ursa;
    public UnicornsSpawn UniSpawn;
    public PegasusSpawn PegaSpawn;
    [SerializeField]
    Transform ArrowStartPosition;
    [SerializeField]
    MovingController controller;
    bool shoot;
    public float waiting { get; private set; }

    [SerializeField]
    AudioClip BowShoot;

    [SerializeField]
    GameObject ArrowPrefab;
    [SerializeField]
    float speed = 40;

    Vector3 tempDestination;

    AudioSource _audi;

    public static Vector3 DirectionOfLaunchForArc(Vector3 targetPos, Vector3 launcherPos, float startSpeed, bool mounted, out bool inRange)
    {
        Vector3 targetDirection = targetPos - launcherPos;
        targetDirection.y = 0f;
        Quaternion targetDirRot = Quaternion.LookRotation(targetDirection);
        Vector3 targetLocalPos = Quaternion.Inverse(targetDirRot) * (targetPos - launcherPos);
        targetLocalPos.z = Mathf.Abs(targetLocalPos.z);

        float x = targetLocalPos.z;
        float y = targetLocalPos.y;

        float v = startSpeed;

        const float g = 9.81f;

        float ang;

        float root = Mathf.Sqrt(v * v * v * v - g * (g * (x * x) + 2 * y * (v * v)));
        if (root > 0)
        {
            float upP;
            if (mounted)
            {
                upP = v * v + root;
            }
            else
            {
                upP = v * v - root;
            }
            float dnP = g * x;
            float divRes = upP / dnP;
            float theta = Mathf.Atan(divRes);
            ang = theta * Mathf.Rad2Deg;
            inRange = true;
        }
        else
        {
            ang = 45f;
            inRange = false;
        }

        Vector3 laubchDir = targetDirRot * Quaternion.Euler(-ang, 0, 0) * Vector3.forward;
        return laubchDir;
    }

    void Flip(bool flip)
    {
        if (controller.SR.flipX != flip)
        {
            controller.SR.flipX = flip;
            for (int i = 0; i < controller.partsOfBody.Length; i++)
            {
                if (controller.partsOfBody[i].gameObject.activeSelf)
                    controller.partsOfBody[i].flipX = flip;
            }
        }
    }

    void CheckFlip(float destinX)
    {
        if (destinX > transform.position.x)
        {
            Flip(false);
        }
        else
        {
            Flip(true);
        }
    }

    private void Start()
    {
        _audi = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && controller.hasBow && !controller.gameManager.GamePaused)
        {
            shoot = true;
            waiting = 0.5f;
            controller.SetAllAnims("shoot");
            tempDestination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CheckFlip(tempDestination.x);
        }

        if (shoot)
        {
            if (waiting > 0)
            {
                waiting -= Time.deltaTime;
            }
            else
            {
                _audi.PlayOneShot(BowShoot);

                Vector3 mouseWorldPos = tempDestination;

                bool inrange;
                Vector3 tempStartPos;
                if (controller.partsOfBody[0].flipX)
                {
                    tempStartPos = new Vector3(ArrowStartPosition.position.x - 2.8f, ArrowStartPosition.position.y, ArrowStartPosition.position.z);
                }
                else
                {
                    tempStartPos = ArrowStartPosition.position;
                }
                Vector3 direction = DirectionOfLaunchForArc(mouseWorldPos, tempStartPos, speed, false, out inrange);


                Rigidbody2D newarrow = PoolManager.getGameObjectFromPool(ArrowPrefab).GetComponent<Rigidbody2D>();
                newarrow.GetComponent<SpriteRenderer>().flipX = controller.SR.flipX;
                newarrow.transform.position = tempStartPos;
                newarrow.velocity = transform.TransformDirection(direction * speed * 1.7f);
                ArrowScript newArrow = newarrow.GetComponent<ArrowScript>();
                newArrow.SetNewArrow();
                newArrow.Hunter = controller;
                newArrow.timer = 5f;
                newArrow.Timberwolf1 = Timberwolf1;
                newArrow.Timberwolf2 = Timberwolf2;
                newArrow.Ursa = Ursa;
                newArrow.unicornSpawn = UniSpawn;
                newArrow.pegasusSpawn = PegaSpawn;
                shoot = false;
            }
        }
    }
}
