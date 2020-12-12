using UnityEngine;

public class NightSoundScript : MonoBehaviour {

    public float myMaxVolume;

    public TimeCount timeCount;
    private AudioSource _audi;

    [SerializeField]
    GameManager gameManager;

    float maxVolume;


    private void Start()
    {
        _audi = GetComponent<AudioSource>();
    }

    private void Update()
    {
        maxVolume = myMaxVolume * gameManager.soundVolume;

        if(timeCount.hours > 21 || timeCount.hours < 4)
        {
            if (_audi.volume < maxVolume) _audi.volume += Time.deltaTime / 2f;
        }
        else
        {
            if (_audi.volume > 0) _audi.volume -= Time.deltaTime / 2f;
        }
    }
}
