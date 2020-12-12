using UnityEngine;

public class MusicScript : MonoBehaviour {

    public bool MusicPlay;
    public float maxVolume;

    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    MainFire mainFire;
    [SerializeField]
    TimeCount timeCount;
    [SerializeField]
    MovingController Player;
    [SerializeField]
    float deepForestBegin;

    [SerializeField]
    AudioClip Lv0Track;
    float Lv0timerBegin;
    [HideInInspector]
    public bool Lv0played;

    [SerializeField]
    AudioClip Lv2Track;
    float Lv2timerBegin;
    [HideInInspector]
    public bool Lv2played;

    [SerializeField]
    AudioClip Lv3Track;
    float Lv3timerBegin;
    [HideInInspector]
    public bool Lv3played;

    [SerializeField]
    AudioClip FightTrack;
    [SerializeField]
    int FightTrackDay;
    [SerializeField]
    int FightTrackHour;
    [HideInInspector]
    public bool FightTrackPlayed;

    [SerializeField]
    AudioClip SadTrack;
    [SerializeField]
    int SadTrackDay;
    [SerializeField]
    int SadTrachHour;
    [HideInInspector]
    public bool SadTrackPlayed;


    [SerializeField]
    Manticore Timberwolf1;
    [SerializeField]
    Manticore Timberwolf2;

    [SerializeField]
    UrsaMinor Ursa;
    [SerializeField]
    AudioClip UrsaTrack;
    [SerializeField]
    int UrsaDayBegin;
    [SerializeField]
    int UrsaHourBegin;
    bool UrsaTrackPlayed;

    AudioSource _audi;

    bool wolfsAreAlive
    {
        get
        {
            return Timberwolf1.health > 0 || Timberwolf2.health > 0;
        }
    }

    void SoundMusic()
    {
        if (!Lv0played)
        {
            if (timeCount.hours >= 5 && timeCount.hours <= 11 && !_audi.isPlaying)
            {
                if (Lv0timerBegin > 0)
                {
                    Lv0timerBegin -= Time.deltaTime;
                }
                else
                {
                    if (_audi.clip != Lv0Track)
                    {
                        _audi.clip = Lv0Track;
                        _audi.Play();
                        Lv0played = true;
                    }
                }
            }
        }
        else if (!Lv2played && !_audi.isPlaying)
        {
            if (timeCount.hours >= 5 && timeCount.hours <= 7)
            {
                if (Lv2timerBegin > 0)
                {
                    Lv2timerBegin -= Time.deltaTime;
                }
                else
                {
                    if (_audi.clip != Lv2Track)
                    {
                        _audi.clip = Lv2Track;
                        _audi.Play();
                        Lv2played = true;
                    }
                }
            }
            // else if (warehouse.buildingLevel > 2) Lv2played = true;
        }
        else if (!Lv3played && !_audi.isPlaying)
        {
            if (timeCount.hours >= 6 && timeCount.hours <= 11 && timeCount.days >= 4)
            {
                if (Lv3timerBegin > 0)
                {
                    Lv3timerBegin -= Time.deltaTime;
                }
                else
                {
                    if (_audi.clip != Lv3Track)
                    {
                        _audi.clip = Lv3Track;
                        _audi.Play();
                        Lv3played = true;
                    }
                }
            }
        }
        else if (!FightTrackPlayed && !_audi.isPlaying)
        {
            if (timeCount.days == FightTrackDay)
                if (timeCount.hours == FightTrackHour)
                {
                    if (_audi.clip != FightTrack)
                    {
                        _audi.clip = FightTrack;
                        _audi.Play();
                        FightTrackPlayed = true;
                    }
                }
        }
        else if(!SadTrackPlayed && !_audi.isPlaying)
        {
            if(timeCount.days == SadTrackDay)
            {
                if(timeCount.hours == SadTrachHour)
                {
                    if(_audi.clip != SadTrack)
                    {
                        _audi.clip = SadTrack;
                        _audi.Play();
                        SadTrackPlayed = true;
                    }
                }
            }
        }
        else if (!UrsaTrackPlayed)
        {
            if (timeCount.days == UrsaDayBegin)
                if (timeCount.hours == UrsaHourBegin)
                {
                    if (_audi.clip != UrsaTrack)
                    {
                        _audi.clip = UrsaTrack;
                        _audi.Play();
                        UrsaTrackPlayed = true;
                    }
                }

        }
    }

    void UpdateMusic()
    {
        if (_audi.isPlaying)
        {
            MusicPlay = true;
            if (!_audi.clip != UrsaTrack && !_audi.clip != FightTrack) //если играют обычные треки
            {
                if ((Player.transform.position.x < deepForestBegin && wolfsAreAlive) || Player.health <= 0 || !_audi.isPlaying)
                {
                    if (_audi.volume > 0) _audi.volume -= Time.deltaTime / 3f;
                }
                else
                {
                    if (_audi.volume < maxVolume * 0.7f - 0.02f) _audi.volume += Time.deltaTime / 10f;
                    if (_audi.volume > maxVolume * 0.7f + 0.02f) _audi.volume -= Time.deltaTime / 10f;
                }
            }
        }
        else MusicPlay = false;
    }

    private void Start()
    {
        Lv0timerBegin = 3f;
        Lv2timerBegin = 2f;
        Lv3timerBegin = 3f;
        _audi = GetComponent<AudioSource>();
    }

    private void Update()
    {
        maxVolume = gameManager.musicVolume;
        UpdateMusic();
        SoundMusic();
    }
}
