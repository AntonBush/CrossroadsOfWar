using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    private static Dictionary<string, LinkedList<GameObject>> poolsDictionary;
    public static Transform deactivatedObjectsParent;

    public static void init(Transform pooledObjectsContainer)
    {
        deactivatedObjectsParent = pooledObjectsContainer;
        poolsDictionary = new Dictionary<string, LinkedList<GameObject>>();
    }

    public static GameObject getGameObjectFromPool(GameObject prefab)
    {
        if (poolsDictionary == null)
        {
            init(GameObject.FindGameObjectWithTag("GameManager").transform);
        }

        if (!poolsDictionary.ContainsKey(prefab.name))
        {
            poolsDictionary[prefab.name] = new LinkedList<GameObject>();
        }

        GameObject result;

        if (poolsDictionary[prefab.name].Count > 0)
        {
            result = poolsDictionary[prefab.name].First.Value;
            poolsDictionary[prefab.name].RemoveFirst();
            if(result == null)
            { //каким-то образом это говно умудряется вытаскивать из словаря пустые значения
                result = GameObject.Instantiate(prefab);
                result.name = prefab.name;
            }
            result.SetActive(true);
            return result;
        }

        result = GameObject.Instantiate(prefab);
        result.name = prefab.name;

        return result;
    }

    public static void ClearPools()
    {
        poolsDictionary.Clear();
    }

    public static void putGameObjectToPool(GameObject target)
    {
        if (target != null)
        {
            poolsDictionary[target.name].AddFirst(target);
            target.transform.parent = deactivatedObjectsParent;
            target.SetActive(false);
        }
    }
}
