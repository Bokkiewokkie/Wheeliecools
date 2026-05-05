using UnityEngine;
public class AudioManager : MonoBehaviour
{

    public float MusicVolume = 0.5f;
    public float SFXVolume = 0.5f;
    [SerializeField] private AudioClip[] Songs;
    private AudioSource MusicPlayer;

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
        MusicPlayer.Play();
    }
}