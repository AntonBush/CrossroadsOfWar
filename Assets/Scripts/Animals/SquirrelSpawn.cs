using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelSpawn : MonoBehaviour
{
    public GameManager gameManager;
    public bool LeftWall;
    public bool RightWall;
    public Transform player;
    public GameObject SquirrelPrefab;
    public List<Transform> LeftTrees;
    public List<Transform> RightTrees;

    public List<Squirrel> Squirrels = new List<Squirrel>();

    IEnumerator Spawner;

    private void Start()
    {
        Spawner = spawner();
        StartCoroutine(Spawner);
    }

    public Squirrel FindSquirrelByTree(Transform tree)
    {
        if (Squirrels.Count > 0)
        {
            for (int i = 0; i < Squirrels.Count; i++)
            {
                if (Squirrels[i] != null && Squirrels[i].newtree != null)
                {
                    if (Squirrels[i].newtree.transform.position == tree.position)
                    {
                        return Squirrels[i];
                    }
                }
            }
            return null;
        }
        else
        {
            return null;
        }
    }

    void SpawnSquirrel()
    {
        if (LeftTrees.Count > 2 || RightTrees.Count > 2)
        {
            if (!LeftWall && !RightWall)
            {
                GameObject newSq = PoolManager.getGameObjectFromPool(SquirrelPrefab);
                newSq.transform.parent = null;
                Squirrel newSquirrel = newSq.GetComponent<Squirrel>();
                newSquirrel.player = player;
                newSquirrel.myParent = this;
                newSquirrel.gameManager = gameManager;

                if (Random.value > 0.5f) // выбираем первое дерево для белки
                {
                    int randomTree1 = Random.Range(0, LeftTrees.Count);
                    newSq.transform.position = LeftTrees[randomTree1].position;
                    newSquirrel.tree = LeftTrees[randomTree1];
                }
                else
                {
                    int randomTree1 = Random.Range(0, RightTrees.Count);
                    newSq.transform.position = RightTrees[randomTree1].position;
                    newSquirrel.tree = RightTrees[randomTree1];
                }

                if (Random.value > 0.5f) //выбираем второе дерево для белки
                {
                    int randomTree2 = Random.Range(0, LeftTrees.Count);
                    newSquirrel.newtree = LeftTrees[randomTree2];
                }
                else
                {
                    int randomTree2 = Random.Range(0, RightTrees.Count);
                    newSquirrel.newtree = RightTrees[randomTree2];
                }

                Squirrels.Add(newSquirrel);
            }
            else //если построены стены
            {
                GameObject newSq = PoolManager.getGameObjectFromPool(SquirrelPrefab);
                newSq.transform.parent = null;
                Squirrel newSquirrel = newSq.GetComponent<Squirrel>();
                newSquirrel.player = player;
                newSquirrel.myParent = this;

                if (Random.value > 0.5f) //выбираем сразу оба дерева для белки, т.к. теперь перебегать на другую сторону они не могут
                {
                    int randomTree1 = Random.Range(0, LeftTrees.Count);
                    newSq.transform.position = LeftTrees[randomTree1].position;
                    newSquirrel.tree = LeftTrees[randomTree1];
                    int randomTree2 = Random.Range(0, LeftTrees.Count);
                    newSquirrel.newtree = LeftTrees[randomTree2];
                    newSquirrel.maxX = -96f;
                }
                else
                {
                    int randomTree1 = Random.Range(0, RightTrees.Count);
                    newSq.transform.position = RightTrees[randomTree1].position;
                    newSquirrel.tree = RightTrees[randomTree1];
                    int randomTree2 = Random.Range(0, RightTrees.Count);
                    newSquirrel.newtree = RightTrees[randomTree2];
                    newSquirrel.minX = -22f;
                }

                Squirrels.Add(newSquirrel);
            }
        }
        else enabled = false;
    }

    IEnumerator spawner()
    {
        for (; ; )
        {
            SpawnSquirrel();
            yield return new WaitForSeconds(Random.Range(15f, 60f));
        }
    }
}
