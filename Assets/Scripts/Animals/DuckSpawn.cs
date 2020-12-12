using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckSpawn : MonoBehaviour
{
    public GameManager gameManager;
    public RuntimeAnimatorController[] birdAnims;
    public AudioClip[] birdSinging;
    public GameObject duckPrefab;
    public TimeCount time;
    public WeatherControl weather;
    public float minX, maxX, minY, maxY;
    int ducksCount;
    public int tempDuckCount;
    bool randomSide;

    IEnumerator spawnDuck;

    private void Start()
    {
        spawnDuck = duckSpawn();
        StartCoroutine(spawnDuck);
    }

    void SpawnDuck()
    {
        GameObject newDuck = PoolManager.getGameObjectFromPool(duckPrefab);
        newDuck.transform.parent = this.transform;
        Duck tempDuck = newDuck.GetComponent<Duck>();
        tempDuck.gameManager = gameManager;
		tempDuck.myParent = this;
		tempDuck.maxSpeed = Random.Range(4f,5f);
        int randomBird = Random.Range(0, birdAnims.Length);
        tempDuck.myAnim = birdAnims[randomBird];
        AudioSource singing = tempDuck.GetComponent<AudioSource>();
        singing.clip = birdSinging[randomBird];
        singing.Play();
        if (randomSide)
        {
            tempDuck.startPosition = new Vector2(minX, Random.Range(minY, maxY));
            tempDuck.endPosition = new Vector2(maxX, Random.Range(minY, maxY));
        }
        else
        {
            tempDuck.startPosition = new Vector2(minX, Random.Range(minY, maxY));
            tempDuck.endPosition = new Vector2(maxX, Random.Range(minY, maxY));
        }
        tempDuckCount++;
    }

    IEnumerator duckSpawn()
    {
        for (; ; )
        {
            if (tempDuckCount == 0)
            {
                if (time == null || weather == null || (time.hours > 5 && time.hours < 22 && weather.weatherNumber < 8))
                {
                    if (Random.value > 0.5f) randomSide = true;
                    else randomSide = false;
                    ducksCount = Random.Range(0, 3);
                    while (tempDuckCount < ducksCount)
                    {
                        SpawnDuck();
                    }
                }
            }
            yield return new WaitForSeconds(Random.Range(15f, 60f));
        }
    }
}
