using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flag : Building {

	public MainFire fire;
	public Transform myWhite;
	public Image Header;
	public Image Border;
	SpriteRenderer SR;
	float timer;


    public AudioClip startCamp;
    public float timerstartCamp;
    bool onetimeS;

    bool downHeader;

    private void Start()
    {
		SR = myWhite.GetComponent<SpriteRenderer>();
        _audi = GetComponent<AudioSource>();
		timer = 0.6f;

        if (fire.buildingLevel > 0)
        {
            transform.localEulerAngles = Vector3.zero;
            downHeader = true;
            myWhite.localScale = new Vector3(1, 1, 1);
            Color newColor = SR.color;
            Header.color = new Color(newColor.r - 0.2f, newColor.g - 0.2f, newColor.b - 0.2f, Header.color.a);
            Border.color = new Color(newColor.r - 0.2f, newColor.g - 0.2f, newColor.b - 0.2f, Border.color.a);
        }
	}

	private void Update()
    {
		if(fire.buildingLevel > 0)
		{
            if(!downHeader) {
                if (Up())
                {
                    if (timer > 0)
                    {
                        timer -= Time.deltaTime;
                        Header.rectTransform.Translate(new Vector2(0, -Time.deltaTime));
                    }
                    if (myWhite.localScale.y < 1)
                    {
                        myWhite.localScale = new Vector3(1, myWhite.localScale.y + Time.deltaTime * 2f, 1);
                    }
                    else
                    {
                        Color newColor = SR.color;
                        Header.color = new Color(newColor.r - 0.2f, newColor.g - 0.2f, newColor.b - 0.2f, Header.color.a);
                        Border.color = new Color(newColor.r - 0.2f, newColor.g - 0.2f, newColor.b - 0.2f, Border.color.a);
                        if (timerstartCamp > 0)
                        {
                            timerstartCamp -= Time.deltaTime;
                        }
                        else
                        {
                            if (!onetimeS)
                            {
                                _audi.PlayOneShot(startCamp);
                                onetimeS = true;
                            }
                            else
                            {
                                if (!_audi.isPlaying)
                                {
                                    enabled = false;
                                }
                            }

                        }
                    }
                }
			}
            else
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                    Header.rectTransform.Translate(new Vector2(0, -Time.deltaTime));
                }
                else enabled = false;
            }
		}
	}
}
