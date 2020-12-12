using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class WaterAnim : MonoBehaviour
{

    public Fisheye fisheye;
    bool oneside;

    void Update()
    {
        if (oneside)
        {
            if (fisheye.strengthX < 0.25f)
            {
                fisheye.strengthX += Time.deltaTime / 15f;

            }
            else
            {
                oneside = false;
            }
        }
        else
        {
            if (fisheye.strengthX > 0.12f)
            {
                fisheye.strengthX -= Time.deltaTime / 15f;

            }
            else
            {
                oneside = true;
            }
        }
    }
}
