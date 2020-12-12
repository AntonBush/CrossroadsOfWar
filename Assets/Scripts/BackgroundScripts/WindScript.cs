using UnityEngine;

public class WindScript : MonoBehaviour {

    public float myMaxVolume;

    public WeatherControl weather;

    [SerializeField]
    GameManager gameManager;

    float maxVolume;

    AudioSource _audi;

    private void Start()
    {
        _audi = GetComponent<AudioSource>();
    }

    private void Update()
    {
        maxVolume = myMaxVolume * gameManager.soundVolume;

       if(weather.timeCount.hours > 5 && weather.timeCount.hours < 22)
        {
            if (_audi.volume < maxVolume) _audi.volume += Time.deltaTime / 2f;
            if (_audi.volume > maxVolume) _audi.volume -= Time.deltaTime / 2f;
        }
       else
        {
            if (_audi.volume > 0.01) _audi.volume -= Time.deltaTime / 2f;
        }
    }
}
