using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    [Header("Clips")]
    public AudioClip bgm;
    public AudioClip clickSFX;

    [Header("Base Volume")]
    public float bgmBaseVolume = 0.5f; // default lebih kecil

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        PlayBGM();
        LoadVolume();
    }

    void LoadVolume()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    void PlayBGM()
{
    if (!musicSource.isPlaying)
    {
        musicSource.clip = bgm;
        musicSource.loop = true;
        musicSource.volume = musicVolume * bgmBaseVolume;
        musicSource.Play();
    }
}

    public void SetMusicVolume(float value)
    {
        musicVolume = value;

        // 🔥 gabungkan base volume + slider
        musicSource.volume = value * bgmBaseVolume;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        sfxSource.volume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void PlayClick()
    {
        PlaySFX(clickSFX);
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlayMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.Play();
    }
}