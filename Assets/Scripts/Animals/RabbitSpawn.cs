using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitSpawn : MonoBehaviour
{
    public GameManager gameManager;
    public bool LeftWall;
    public bool RightWall;
    public GameObject RabbitPrefab;
    public Transform player;
    public Manticore Timberwolf1;
    public Manticore Timberwolf2;
    public List<Rabbit> rabbits = new List<Rabbit>();

    IEnumerator SpawnRabbit;

    private void Start()
    {
        SpawnRabbit = spawnRabbit();
        StartCoroutine(SpawnRabbit);
    }

    IEnumerator spawnRabbit()
    {
        for (; ; )
        {
            if (rabbits.Count < 4)
            {
                float minX, maxX;
                if (Random.value > 0.5f) //если кролик спавнится слева на карте
                {
                    minX = -213f;
                    if (!LeftWall) //если слева стоит стена
                    {
                        maxX = -124f; //граница - чуть левее этой стены
                    }
                    else  //если слева стены нет
                    {
                        if (player.transform.position.x > -150f) //если игрок далеко 
                            maxX = player.transform.position.x - 15f;
                        else
                            maxX = player.transform.position.x + 30f;
                    }
                }
                else
                {
                    maxX = 60f;
                    if (!RightWall) //если справа стоит стена
                    {
                        minX = -22f; //граница - чуть правее этой стены
                    }
                    else  //если справа стены нет
                    {
                        if (player.transform.position.x < 60) //если игрок далеко 
                            minX = player.transform.position.x + 15f;
                        else
                            minX = player.transform.position.x - 30f;
                    }
                }

                float randomX = Random.Range(minX, maxX);

                GameObject tempRabbit = PoolManager.getGameObjectFromPool(RabbitPrefab);
                tempRabbit.transform.position = new Vector2(randomX, -2f);
                Rabbit tempRab = tempRabbit.GetComponent<Rabbit>();
                tempRab.gameManager = gameManager;
                tempRab.minX = minX;
                tempRab.maxX = maxX;
                tempRab.health = 20;
                tempRab.deadTimer = 2f;
                tempRab.myParent = this;
                tempRab.Timberwolf1 = Timberwolf1;
                tempRab.Timberwolf2 = Timberwolf2;
                rabbits.Add(tempRab);
            }
            yield return new WaitForSeconds(10f);
        }
    }
}
