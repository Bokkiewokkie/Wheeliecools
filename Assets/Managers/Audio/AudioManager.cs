using UnityEngine;
using System.Collections;
using System.Linq.Expressions;

public class AudioManager : MonoBehaviour
{

    public float MusicVolume = 0.5f;
    public float SFXVolume = 0.5f;
    [SerializeField] private AudioClip[] Songs;
    /*
    0 - Menu theme
    1 - First gameplay song
    2 - Second gameplay song
    3 - Shop theme
    4 - Third gameplay song
    5 - Ending theme
    */
    private AudioSource MusicPlayer;
    private int QueueTime = 0;
    private bool FirstTrack = false;
    private bool SecondTrack = false;

    private AudioClip Interrupted;
    private int InterruptResumeTime;

    // Singleton design pattern, only 1 AudioManager can exist at a time.
    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            Debug.Log("AudioManager already exists");
        }
        else
        {
            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
        }
    }
    
    void Start()
    {
        MusicPlayer = GetComponent<AudioSource>();
        PlayMusic();
    }

    void Update()
    {
        double time = AudioSettings.dspTime;

        if (time + 1.0f > QueueTime)
        {
            HandleGameplayTracks();
        }
    }

    private void PlayMusic()
    {
        //Always start playing the menu theme
        MusicPlayer.clip = Songs[0];
        MusicPlayer.loop = true; //Music always loops
        MusicPlayer.volume = MusicVolume;
        MusicPlayer.Play();
    }

    public void SetMusicVolume(float input)
    {
        MusicVolume = input;
        MusicPlayer.volume = MusicVolume;
    }
    public void SetSFXVolume(float input)
    {
        SFXVolume = input;
    }

    public void SwitchMusic(int track)
    {
        MusicPlayer.clip = Songs[track]; //Change to selected
        if(track == 1 || track == 2 || track == 4)
        {
            MusicPlayer.loop = false;
            QueueTime = (int)AudioSettings.dspTime + (int)Songs[track].length; //When the next song should play    
        }
        else{
            MusicPlayer.loop = true;
        }
        MusicPlayer.Play();
    }

    public void HandleGameplayTracks()
    {
        if (FirstTrack)
        {
            SecondTrack = true;
            FirstTrack = false;
            SwitchMusic(2);
        }
        else if(SecondTrack)
        {
            SecondTrack = false;
            SwitchMusic(1);
        }
        else
        {
            FirstTrack = true;
            SwitchMusic(4);
        }
    }

    //This is basically only used so we can switch to the shop music and then back to the game
    public void InterruptWith(int track, bool looping)
    {
        Interrupted = MusicPlayer.clip;
        InterruptResumeTime = MusicPlayer.timeSamples;
        MusicPlayer.loop = looping;
        SwitchMusic(track);
    }

    public void ResumeTrack(bool looping)
    {
        MusicPlayer.loop = looping;
        MusicPlayer.clip = Interrupted;
        MusicPlayer.timeSamples = InterruptResumeTime;
        MusicPlayer.Play();
    }

}