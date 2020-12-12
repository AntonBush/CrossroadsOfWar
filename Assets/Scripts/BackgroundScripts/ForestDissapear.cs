using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestDissapear : MonoBehaviour
{

    public int myTrees;
    public SpriteRenderer[] SRs;
    int i;

    float timer = 10f;

    private void Update()
    {
        if (myTrees == 0)
        {
            if (timer > 0)
            {
				timer -= Time.deltaTime;
                if (SRs[i].color.a > 0)
                {
                    SRs[i].color = new Color(1, 1, 1, SRs[i].color.a - Time.deltaTime);
                }
				if(i < SRs.Length - 1) i++;
				else i = 0;
            }
			else gameObject.SetActive(false);
        }
    }
}
