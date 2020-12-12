using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyColor : MonoBehaviour
{

    public SpriteRenderer originSR;
    SpriteRenderer SR;
    float timer = 0.1f;

    [SerializeField]
    bool alwaysCheck;

    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            SR.color = originSR.color;
            if(!alwaysCheck)
			enabled = false;
        }
    }
}
