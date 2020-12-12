using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMoving : MonoBehaviour
{
    public Transform sun;
    public TimeCount time;
    public Light sunLight;
	public SpriteRenderer nightStars;

    float speedX, speedY;

    [HideInInspector]
    public Color SkyColor, SunColor;
    Color tempSkyColor, tempSunColor;


    void UpdateStars() {
		 if (time.hours >= 4) {
			 if(nightStars.color.a > 0) {
				 nightStars.color = new Color {
					 r = nightStars.color.r,
					 g = nightStars.color.g,
					 b = nightStars.color.b,
					 a = nightStars.color.a - Time.deltaTime / 5f
				 };
			 }
		 }
		 if (time.hours >= 22 || time.hours < 4) {
			 if(nightStars.color.a < 1) {
				 nightStars.color = new Color {
					 r = nightStars.color.r,
					 g = nightStars.color.g,
					 b = nightStars.color.b,
					 a = nightStars.color.a + Time.deltaTime / 5f
				 };
			 }
		 }
	}

    void SetSkySunColor()
    {
        if(time.hours <= 3)
        {
            SkyColor = new Color(0.32f, 0.22f, 0.32f);
            SunColor = new Color(0.7f, 0.7f, 0.7f);
        }
        else if (time.hours < 5)
        {
            SkyColor = new Color(0.412f, 0.303f, 0.496f);
        }
        else if (time.hours < 7)
        {
            SkyColor = new Color(0.873f, 0.475f, 0.410f);
            SunColor = new Color(1f, 0.322f, 0f);
        }
        else if (time.hours < 11)
        {
            SkyColor = new Color(0.732f, 0.667f, 0.657f);
            SunColor = new Color(0.944f, 0.971f, 0.499f);
        }
        else if(time.hours < 17)
        {
            SkyColor = new Color(0.999f, 0.999f, 0.999f);
            SunColor = new Color(1f, 1f, 1f);
        }
        else if(time.hours < 20)
        {
            SkyColor = new Color(0.864f, 0.529f, 0.529f);
            SunColor = new Color(1f, 0.439f, 0.4f);
        }
        else if(time.hours < 21)
        {
            SkyColor = new Color(0.864f, 0.529f, 0.529f);
            SunColor = new Color(0.5f, 0.3f, 0.3f);
        }
        else if(time.hours < 23)
        {
            SkyColor = new Color(0.254f, 0.207f, 0.420f);
            SunColor = new Color(0.8f, 0.8f, 0.8f);
        }
    }

    void SetMovingSpeed()
    {
        if (time.hours == 4) //готовим солнце к рассвету
        {
           // SkyColor = new Color(0.412f, 0.303f, 0.496f);
            sun.transform.localPosition = new Vector2(-9f, 2.1f);
        }
        if (time.hours == 5) //рассвет
        {
            //SkyColor = new Color(0.873f, 0.475f, 0.410f);
           // SunColor = new Color(1f, 0.322f, 0f);
            speedX = 0.0022f;
            speedY = 0.0005f;
        }
      //  if (time.hours == 7)
       // {
          //  SkyColor = new Color(0.732f, 0.667f, 0.657f);
           // SunColor = new Color(0.944f, 0.971f, 0.499f);
       // }
        if (time.hours == 11) //подлень
        {
           // SkyColor = new Color(0.999f, 0.999f, 0.999f);
          //  SunColor = new Color(1f, 1f, 1f);
            speedY = 0f;
        }
        if (time.hours == 17) //вечер
        {
          //  SkyColor = new Color(0.864f, 0.529f, 0.529f);
          //  SunColor = new Color(1f, 0.439f, 0.4f);
            speedY = -0.0025f;
        }
        if (time.hours == 21) //солнце исчезает с неба
        {
           // SkyColor = new Color(0.864f, 0.529f, 0.529f);
           // SunColor = new Color(0.5f, 0.3f, 0.3f);
            sun.transform.localPosition = new Vector2(9f, 2.1f);
        }
        if (time.hours == 22)  //солнце превращается в луну
        {
           // SkyColor = new Color(0.254f, 0.207f, 0.420f);
           // SunColor = new Color(0.8f, 0.8f, 0.8f);
            speedX = -0.001f; 
			speedY =  0.0005f;
        }
        if (time.hours == 24) //луна стоит на месте всю ночь
        {
            speedX = speedY = 0;
        }
        if (time.hours == 3) //луна скрывается с неба и превращается в солнце
        {
           // SkyColor = new Color(0.32f, 0.22f, 0.32f);
           // SunColor = new Color(0.7f, 0.7f, 0.7f);
            speedX = 0.0015f;
            speedY = -0.000542f;
        }
    }

    void ChangeSkyColor()
    {
        if (tempSkyColor.r < SkyColor.r - 0.05f) tempSkyColor.r += Time.deltaTime / 8f;
        if (tempSkyColor.r > SkyColor.r + 0.05f) tempSkyColor.r -= Time.deltaTime / 8f;

        if (tempSkyColor.g < SkyColor.g - 0.05f) tempSkyColor.g += Time.deltaTime / 8f;
        if (tempSkyColor.g > SkyColor.g + 0.05f) tempSkyColor.g -= Time.deltaTime / 8f;

        if (tempSkyColor.b < SkyColor.b - 0.05f) tempSkyColor.b += Time.deltaTime / 8f;
        if (tempSkyColor.b > SkyColor.b + 0.05f) tempSkyColor.b -= Time.deltaTime / 8f;

        RenderSettings.ambientLight = tempSkyColor;
    }

    void ChangeSunColor()
    {
        if (tempSunColor.r < SunColor.r - 0.05f) tempSunColor.r += Time.deltaTime / 8f;
        if (tempSunColor.r > SunColor.r + 0.05f) tempSunColor.r -= Time.deltaTime / 8f;

        if (tempSunColor.g < SunColor.g - 0.05f) tempSunColor.g += Time.deltaTime / 8f;
        if (tempSunColor.g > SunColor.g + 0.05f) tempSunColor.g -= Time.deltaTime / 8f;

        if (tempSunColor.b < SunColor.b - 0.05f) tempSunColor.b += Time.deltaTime / 8f;
        if (tempSunColor.b > SunColor.b + 0.05f) tempSunColor.b -= Time.deltaTime / 8f;

        sunLight.color = tempSunColor;
    }

    private void Update()
    {
		UpdateStars();
        SetMovingSpeed();
        SetSkySunColor();
        ChangeSkyColor();
        ChangeSunColor();
        if(!time.gameManager.GamePaused)
        sun.transform.Translate(speedX, speedY, 0);
    }
}
