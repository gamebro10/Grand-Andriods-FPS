using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    Dictionary<AudioSource, float> SFXAudios;
    Dictionary<AudioSource, float> MusicAudios;

    int level;
    float checkNullTime = 30f;

    static bool isQuitting;
    static AudioManager mInstance;

    /// <summary>
    /// do a null reference check --if(AudioManager.Instance != null)-- when calling Instance in OnDestroy() function
    /// </summary>
    public static AudioManager Instance
    {
        get
        {
            if (isQuitting)
            {
                return null;
            }
            if (mInstance == null)
            {
                GameObject go = Instantiate(new GameObject("Audio Manager"), null);
                mInstance = go.AddComponent<AudioManager>();
                DontDestroyOnLoad(mInstance);
            }
            return mInstance;
        }
    }

    private void Awake()
    {
        InitVars();
        SceneManager.sceneLoaded += delegate { InitVars(); };

        InvokeRepeating("CheckNullAudios", checkNullTime, checkNullTime);
    }

    public void RegisterSFX(AudioSource audio)
    {
        if (SFXAudios.TryAdd(audio, audio.volume) && GameManager.Instance != null)
        {
            audio.volume *= (float)GameManager.Instance.optionsvalues.SFXValue / 100;
        }
    }
    public void RegisterMusic(AudioSource audio)
    {
        if (MusicAudios.TryAdd(audio, audio.volume) && GameManager.Instance != null)
        {
            audio.volume *= (float)GameManager.Instance.optionsvalues.MusicValue / 100;
        }
    }
    public void UnregisterSFX(AudioSource audio)
    {
        SFXAudios.Remove(audio);
    }
    public void UnregisterMusic(AudioSource audio)
    {
        MusicAudios.Remove(audio);
    }

    void CheckNullAudios()
    {
        foreach (KeyValuePair<AudioSource, float> audio in SFXAudios)
        {
            if (audio.Key == null)
            {
                UnregisterSFX(audio.Key);
            }
        }
        foreach (KeyValuePair<AudioSource, float> audio in MusicAudios)
        {
            if (audio.Key == null)
            {
                UnregisterMusic(audio.Key);
            }
        }
    }

    void OnSFXChanged()
    {
        foreach (KeyValuePair<AudioSource, float> audio in SFXAudios)
        {
            if (audio.Key != null && GameManager.Instance != null)
            {
                audio.Key.volume = audio.Value * (float)GameManager.Instance.optionsvalues.SFXValue / 100;
            }
            else
            {
                UnregisterSFX(audio.Key);
            }
        }
    }

    void OnMusicChanged()
    {
        foreach (KeyValuePair<AudioSource, float> audio in MusicAudios)
        {
            if (audio.Key != null && GameManager.Instance != null)
            {
                audio.Key.volume = audio.Value * (float)GameManager.Instance.optionsvalues.MusicValue / 100;
            }
            else
            {
                UnregisterMusic(audio.Key);
            }
        }
    }

    void InitVars()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SFXSlider.onValueChanged.AddListener(delegate { OnSFXChanged(); });
            GameManager.Instance.MusicSlider.onValueChanged.AddListener(delegate { OnMusicChanged(); });
        }
        SFXAudios = new Dictionary<AudioSource, float>();
        MusicAudios = new Dictionary<AudioSource, float>();
        isQuitting = false;
        level = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnDestroy()
    {
        isQuitting = true;
    }
}
