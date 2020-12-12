using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSex : MonoBehaviour
{
    public bool Player;
    public bool newPony;
    public Animator[] anims;

    public RuntimeAnimatorController[] animsColt;
    public RuntimeAnimatorController[] animsMare;

    public Animator HairAnim;
    public RuntimeAnimatorController[] mareHair;
    public RuntimeAnimatorController coltHair;

    public GameObject Beard;

    public bool playerMare;

    [HideInInspector]
    public int randomHair;

    public void SetNewValues()
    {
        if (playerMare)
        {
            for (int i = 0; i < anims.Length; i++)
            {
                anims[i].runtimeAnimatorController = animsMare[i];
            }
            HairAnim.runtimeAnimatorController = mareHair[randomHair];
        }
        else
        {
            for (int i = 0; i < anims.Length; i++)
            {
                anims[i].runtimeAnimatorController = animsColt[i];
            }
            HairAnim.runtimeAnimatorController = coltHair;
        }
    }

    void SetGender()
    {
        if (Random.value > 0.5f)
        {
            playerMare = true;
            Beard.SetActive(false);
            for (int i = 0; i < anims.Length; i++)
            {
                anims[i].runtimeAnimatorController = animsMare[i];
            }
            randomHair = Random.Range(0, mareHair.Length);

            if(HairAnim != null)
            HairAnim.runtimeAnimatorController = mareHair[randomHair];
        }
        else
        {
            playerMare = false;
            for (int i = 0; i < anims.Length; i++)
            {
                anims[i].runtimeAnimatorController = animsColt[i];
            }

            if (Random.value > 0.5f)
            {
                Beard.SetActive(true);
                Beard.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX; //чот сложна
            }
            else
            {
                Beard.SetActive(false);
            }

            if(HairAnim != null)
            HairAnim.runtimeAnimatorController = coltHair;
        }
    }

    void Start()
    {
        if(!Player)
        SetGender();
    }

    private void Update()
    {
        if (newPony)
        {
            SetGender();
            newPony = false;
        }
    }
}
