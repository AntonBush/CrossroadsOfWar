using UnityEngine;

public class SoundVolumeObject : MonoBehaviour {

    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    float myMaxVolume;

    AudioSource _audi;

    private void Start()
    {
        _audi = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _audi.volume = myMaxVolume * gameManager.soundVolume;
    }
}
