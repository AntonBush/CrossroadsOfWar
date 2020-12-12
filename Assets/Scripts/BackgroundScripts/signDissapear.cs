using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class signDissapear : MonoBehaviour
{
    public Transform player;
    public GameObject SignText;
    bool onetime;

    void Update()
    {
        if (Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(player.position.x)) < 5f)
        {
            SignText.gameObject.SetActive(true);
            onetime = true;
        }
        else
        {
            SignText.gameObject.SetActive(false);
            if (onetime)
            {
                enabled = false;
            }
        }
    }
}
