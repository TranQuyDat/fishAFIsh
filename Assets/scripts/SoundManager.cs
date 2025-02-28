using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public enum BGMType
{
    UpSurFaceWater,
    UnderSurFaceWater,
}

public enum SFXType
{
    Click,
    Eat,
    Jump,
    LvUp
}

[System.Serializable]
public class BGMEntry
{
    public BGMType type;
    public AudioClip clip;
}

[System.Serializable]
public class SFXEntry
{
    public SFXType type;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public Transform fxActive;
    public Transform fxDeActive;
    public AudioMixer audioMixer;
    public AudioSource bgmSource;
    public AudioSource sfxSourcePrefab;
    private Queue<AudioSource> sfxPool = new Queue<AudioSource>();
    private int poolSize = 10;

    public List<BGMEntry> bgmEntries;
    public List<SFXEntry> sfxEntries;
    private Dictionary<BGMType, AudioClip> bgmClips;
    private Dictionary<SFXType, AudioClip> sfxClips;
    GameManager gameManager;
    DataGame datagame;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDictionaries();
            InitializeSFXPool();
        }
        else if(Instance != null && Instance != this) 
        {
            Destroy(this);
        }
    }
    private void Update()
    {
        if(datagame==null || gameManager == null)
        {
            datagame = GameManager.instance.dataGame;
            gameManager = GameManager.instance;
            initSoundValues();
        }
        update_BGM_SFX();
    }



    private void InitializeDictionaries()
    {
        bgmClips = new Dictionary<BGMType, AudioClip>();
        foreach (var entry in bgmEntries)
        {
            bgmClips[entry.type] = entry.clip;
        }

        sfxClips = new Dictionary<SFXType, AudioClip>();
        foreach (var entry in sfxEntries)
        {
            sfxClips[entry.type] = entry.clip;
        }
    }
    private void InitializeSFXPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource sfxSource = Instantiate(sfxSourcePrefab, fxDeActive);
            sfxSource.gameObject.SetActive(false);
            sfxPool.Enqueue(sfxSource);
        }
    }
    private void initSoundValues()
    {
        datagame.load();
        gameManager.uiGame.slider_Music.value = datagame.musicBGVolume;
        gameManager.uiGame.slider_Sound.value = datagame.soundFXVolume;

        SetSFXVolume(datagame.soundFXVolume);
        SetBGMVolume(datagame.soundFXVolume);
    }
    public void update_BGM_SFX()
    {
        if (gameManager.uiGame.slider_Sound.value != datagame.soundFXVolume)
        {
            datagame.soundFXVolume = gameManager.uiGame.slider_Sound.value;
            SetSFXVolume(gameManager.uiGame.slider_Sound.value);
        }

        if (gameManager.uiGame.slider_Music.value != datagame.musicBGVolume)
        {
            datagame.musicBGVolume = gameManager.uiGame.slider_Music.value;
            SetBGMVolume(gameManager.uiGame.slider_Music.value);
        }
        datagame.save();
    }

    public void PlayBGM(BGMType type)
    {
        if (bgmClips.TryGetValue(type, out AudioClip clip))
        {
            if (bgmSource.clip == clip) return;
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    public void PlaySFX(SFXType type)
    {
        if (sfxClips.TryGetValue(type, out AudioClip clip))
        {
            if (sfxPool.Count > 0)
            {
                AudioSource sfxSource = sfxPool.Dequeue();
                sfxSource.clip = clip;
                sfxSource.transform.parent = fxActive;
                sfxSource.gameObject.SetActive(true);
                sfxSource.Play();
                Invoke(nameof(ReturnToPool), clip.length);
            }
        }
    }

    private void ReturnToPool()
    {
        AudioSource sfxSource = fxActive.GetChild(0).GetComponent<AudioSource>();
        sfxSource.transform.parent = fxDeActive;
        sfxSource.gameObject.SetActive(false);
        sfxPool.Enqueue(sfxSource);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }
}
