using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadParticles : MonoBehaviour
{
    public GameManager gameManager;
    public float timerDissapear = 1f;

    AudioSource _audi;

    private void Start()
    {
        _audi = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (gameManager == null) gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        _audi.volume = gameManager.soundVolume * 0.6f;

        if (timerDissapear > 0)
        {
            timerDissapear -= Time.deltaTime;
        }
        else
        {
			PoolManager.putGameObjectToPool(gameObject);
        }
    }
}
