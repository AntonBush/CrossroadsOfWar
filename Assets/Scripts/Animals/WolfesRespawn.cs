using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfesRespawn : MonoBehaviour {

    [SerializeField]
    ForestDissapear myForest;

    [SerializeField]
     TimeCount timeCount;

    [SerializeField]
    Transform Player;

    [SerializeField]
    Manticore Timberwolf1;
    [SerializeField]
    Manticore Timberwolf2;

    int tempDay;
    bool onetime;

    void RespawnWolf(Manticore wolf)
    {
        wolf.deadTimer = 2f;
        wolf.health = 160;
        if (wolf != null)
        {
            wolf.gameObject.SetActive(true);
            wolf.GetComponent<Animator>().SetBool("dead", false);
            wolf.EyesGlowAnimator.SetBool("dead", false);
        }
        else
        {
            print("волк исчез! чо за херня?");
        }
    }

    void CheckWolfSpawn()
    {
        if (myForest.myTrees < 10) enabled = false;

        if (!onetime)
        {
            if (Timberwolf1.health <= 0 && Timberwolf2.health <= 0)
            {
                tempDay = timeCount.days + 2;
                onetime = true;
            }
        }
        else
        {
            if(timeCount.days == tempDay)
            {
                if(Vector2.Distance(Timberwolf1.transform.position,Player.transform.position) > 11f &&
                    Vector2.Distance(Timberwolf2.transform.position, Player.transform.position) > 11f)
                {
                    RespawnWolf(Timberwolf1);
                    RespawnWolf(Timberwolf2);
                    onetime = false;
                }
            }
        }
    }

    private void Update()
    {
        CheckWolfSpawn();
    }
}
