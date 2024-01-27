﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioGameSetting audioSetting;
    public AudioGameSetting AudioSetting
    {
        get
        {
            return audioSetting;
        }
        set
        {
            audioSetting = value;
            ApplyAudioSettings();
        }
    }
    [SerializeField] List<AudioSource> audioSources;
    [SerializeField] AudioSource looperAudioSource;

    [SerializeField] AudioSource bgmIntroAudioSource;
    [SerializeField] AudioSource bgmAudioSource;


    public enum AudioType
    {
        BGM,
        SFX,
        UI_SFX
    }

    [System.Serializable]
    public class BGM_SpecialHandler
    {
        public AudioClip BGM;
        public float endFadingTime = 5f;
    }

    [SerializeField] List<AudioClip> BGMs;
    [SerializeField] List<BGM_SpecialHandler> BGM_specialHandler;
    [SerializeField] List<AudioClip> sfxs;
    [SerializeField] List<AudioClip> UI_sfxs;

    //private parameters
    private Coroutine bgmTransitionCoroutine = null;

    //private void OnValidate()
    //{
    //    Button[] fooGroup = Resources.FindObjectsOfTypeAll<Button>();
    //print(fooGroup.Length);
    //    foreach (Button b in fooGroup)
    //    {
    //        if (b.GetComponent<MedalGameAudioPlayer>() == null)
    //        {
    //            b.gameObject.AddComponent<MedalGameAudioPlayer>();
    //        }
    //    }
    //}

#if UNITY_EDITOR
    private void OnValidate()
    {
        sfxs = ExtraFunction.FindAssetsByType<AudioClip>("Assets/SFX").ToList();
        //UI_sfxs = ExtraFunction.FindAssetsByType<AudioClip>("Assets/1-BinaryTree/Audio/UI_SoundEffect").ToList();
    }
#endif

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            this.gameObject.SetActive(false);
    }
    public void ApplyAudioSettings()
    {
        BgmVolumeAdjust(audioSetting.BGMVolume);
        print("Apply audio settings");
    }

    private void Start()
    {
        ApplyAudioSettings();
    }

    public int GetAudioIDByName(AudioType type, string name)
    {
        switch (type)
        {
            case AudioType.BGM:
                for (int i = 0; i < BGMs.Count; i++)
                {
                    if (BGMs[i].name.Equals(name))
                    {
                        return i;
                    }
                }
                break;
            case AudioType.SFX:
                for (int i = 0; i < sfxs.Count; i++)
                {
                    if (sfxs[i].name.Equals(name))
                    {
                        return i;
                    }
                }
                break;
            case AudioType.UI_SFX:
                for (int i = 0; i < UI_sfxs.Count; i++)
                {
                    if (UI_sfxs[i].name.Equals(name))
                    {
                        return i;
                    }
                }
                break;
        }
        Debug.LogError("Put the correct sound effect under the correct audio folder please");
        return -1;
    }

    public void Play(string audioName, float volume = 1)
    {
        audioSources[0].volume = volume * audioSetting.sfxVolume;
        audioSources[0].clip = (sfxs.Find((s) => s.name == audioName));
        audioSources[0].Play();
    }


    //public void PlayNoOverlap(int audioIndex, int channel = 1, float volume = 1)
    //{
    //    if (!audioSources[channel].isPlaying)
    //    {
    //        audioSources[channel].volume = volume * audioSetting.sfxVolume;
    //        audioSources[channel].clip = sfxs[audioIndex];
    //        audioSources[channel].Play();
    //    }
    //}
    //public void PlayNoOverlap(string audioName, int channel = 1, float volume = 1)
    //{
    //    if (!audioSources[channel].isPlaying)
    //    {
    //        audioSources[channel].volume = volume * audioSetting.sfxVolume;
    //        audioSources[channel].clip = sfxs.Find((s) => s.name == audioName);
    //        audioSources[channel].Play();
    //    }
    //}

    public void PlayOnUnusedTrack(int audioIndex, float volume = 1)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].clip == sfxs[audioIndex] && !audioSources[i].isPlaying)
            {
                audioSources[i].volume = volume * audioSetting.sfxVolume;
                audioSources[i].pitch = Time.timeScale;
                audioSources[i].Play();
                return;
            }
        }

        for (int i = 0; i < audioSources.Count; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                audioSources[i].volume = volume * audioSetting.sfxVolume;
                audioSources[i].pitch = Time.timeScale;
                audioSources[i].clip = sfxs[audioIndex];
                audioSources[i].Play();
                return;
            }

        }
    }
    public void PlayOnUnusedTrack(string audioName, float volume = 1)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].clip == sfxs.Find((s) => s.name == audioName) && !audioSources[i].isPlaying)
            {
                audioSources[i].volume = volume * audioSetting.sfxVolume;
                audioSources[i].pitch = Time.timeScale;
                audioSources[i].Play();
                return;
            }
        }

        for (int i = 0; i < audioSources.Count; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                audioSources[i].volume = volume * audioSetting.sfxVolume;
                audioSources[i].clip = sfxs.Find((s) => s.name == audioName);
                audioSources[i].pitch = Time.timeScale;
                audioSources[i].Play();
                return;
            }

        }
    }

    public void PlayOnUnusedTrack_UI(int audioIndex, float volume = 1)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].clip == UI_sfxs[audioIndex] && !audioSources[i].isPlaying)
            {
                audioSources[i].volume = volume * audioSetting.UIsfxVolume;
                audioSources[i].pitch = Time.timeScale;
                audioSources[i].Play();
                return;
            }
        }

        for (int i = 0; i < audioSources.Count; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                audioSources[i].volume = volume * audioSetting.UIsfxVolume;
                audioSources[i].clip = UI_sfxs[audioIndex];
                audioSources[i].pitch = Time.timeScale;
                audioSources[i].Play();
                return;
            }

        }

    }

    public void PlayOnUnusedTrack_UI(string audioName, float volume = 1)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].clip == UI_sfxs.Find((s) => s.name == audioName) && !audioSources[i].isPlaying)
            {
                audioSources[i].volume = volume * audioSetting.UIsfxVolume;
                audioSources[i].pitch = Time.timeScale;
                audioSources[i].Play();
                return;
            }
        }

        for (int i = 0; i < audioSources.Count; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                audioSources[i].volume = volume * audioSetting.UIsfxVolume;
                audioSources[i].clip = UI_sfxs.Find((s) => s.name == audioName);
                audioSources[i].pitch = Time.timeScale;
                audioSources[i].Play();
                return;
            }

        }

    }
    public void LoopSFX(int audioIndex, float volume = 1)
    {
        looperAudioSource.volume = volume * audioSetting.sfxVolume;
        looperAudioSource.loop = true;
        looperAudioSource.clip = sfxs[audioIndex];
        looperAudioSource.Play();
    }
    public void LoopSFX(string audioName, float volume = 1)
    {
        looperAudioSource.volume = volume * audioSetting.sfxVolume;
        looperAudioSource.loop = true;
        looperAudioSource.clip = sfxs.Find((s) => s.name == audioName);
        looperAudioSource.Play();
    }

    public void BgmVolumeAdjust(float volume)
    {
        bgmAudioSource.volume = audioSetting.BGMVolume;
        bgmIntroAudioSource.volume = audioSetting.BGMVolume;
    }

    //public int GetCurrentBGMIndex()
    //{

    //    return BGMs.FindIndex((b) => b.name.Equals(bgmAudioSource.clip?.name));
    //}

    public void PlayConnectedBGM(int audioIndex, int introAudioIndex)
    {
        print("play connected bgm");
        bgmIntroAudioSource.clip = BGMs[introAudioIndex];
        bgmIntroAudioSource.volume = audioSetting.BGMVolume;
        bgmIntroAudioSource.Play();
        bgmAudioSource.clip = BGMs[audioIndex];
        bgmAudioSource.PlayDelayed(bgmIntroAudioSource.clip.length);
    }

    public void ChangeBGM(int audioIndex)
    {
        if (bgmAudioSource.clip == BGMs[audioIndex] && bgmAudioSource.isPlaying) // if the same bgm is playing
        {
            print("Same bgm is playing already");
            return;
        }
        //else if (bgmAudioSource_Track1.clip == BGMs[audioIndex])                        // if the same bgm is playing but not in its volume
        //{
        //    print("same bgm is playing but not in its volume");
        //    StopCoroutine(bgmTransitionCoroutine);
        //    StartCoroutine( FadeInBGM(audioIndex,0.2f));
        //    bgmTransitionCoroutine = null;
        //}
        else
        {
            if (bgmIntroAudioSource.isPlaying)
            {
                StartCoroutine(FadeOutBGMIntro(0.5f));
            }
            if (bgmAudioSource.isPlaying && bgmTransitionCoroutine == null)     // if other bgm is playing
            {
                print("Change BGM to " + BGMs[audioIndex].name.ToString());
                bgmTransitionCoroutine = StartCoroutine(BGMTransition(audioIndex));
            }
            else if (bgmTransitionCoroutine != null)                            // if the last bgm transition running
            {
                print("BGM is changing too quickly, please check your bgm playing method");
                return;
            }
            else                                                                // if no bgm is playing
            {
                print("Player BGM " + BGMs[audioIndex].name.ToString());
                bgmAudioSource.volume = audioSetting.BGMVolume;
                bgmAudioSource.loop = true;
                bgmAudioSource.clip = BGMs[audioIndex];
                bgmAudioSource.Play();
            }
        }
    }
    public void ForcePlayLastSecondOfBGM(int audioIndex)
    {
        bgmAudioSource.clip = BGMs[audioIndex];
        bgmAudioSource.time = BGMs[audioIndex].length - 0.01f;
        bgmAudioSource.Play();
        StartCoroutine(FadeOutBGMIntro(0.2f));
    }

    private IEnumerator BGMTransition(int audioIndex, float fadeTime = 0.75f)
    {
        bgmTransitionCoroutine = StartCoroutine(FadeOutBGM(fadeTime));

        yield return bgmTransitionCoroutine;

        bgmTransitionCoroutine = StartCoroutine(FadeInBGM(audioIndex, fadeTime));
        yield return bgmTransitionCoroutine;
        bgmTransitionCoroutine = null;
    }


    public void StopBGM(float fadeTime = 1.5f)
    {
        StartCoroutine(FadeOutBGM(fadeTime));
    }

    private IEnumerator FadeInBGM(int audioIndex, float FadeTime)
    {
        float targetVolume = audioSetting.BGMVolume;
        bgmAudioSource.volume = 0;
        bgmAudioSource.loop = true;
        bgmAudioSource.clip = BGMs[audioIndex];
        bgmAudioSource.Play();
        while (bgmAudioSource.volume < targetVolume)
        {
            bgmAudioSource.volume += targetVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        bgmAudioSource.volume = targetVolume;
    }
    private IEnumerator FadeOutBGM(float FadeTime)
    {
        float startVolume = bgmAudioSource.volume;

        while (bgmAudioSource.volume > 0)
        {
            bgmAudioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        bgmAudioSource.Stop();
        bgmAudioSource.volume = startVolume;
    }
    private IEnumerator FadeOutBGMIntro(float FadeTime)
    {
        float startVolume = bgmIntroAudioSource.volume;

        while (bgmIntroAudioSource.volume > 0)
        {
            bgmIntroAudioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        bgmIntroAudioSource.Stop();
        bgmIntroAudioSource.volume = startVolume;
    }


    public void StopLooperSFX()
    {
        looperAudioSource.Pause();
        looperAudioSource.loop = false;


    }

}
[System.Serializable]
public class AudioGameSetting
{
    public float masterVolume;
    [SerializeField] float _BGMVolume;
    public float BGMVolume { get { return _BGMVolume * masterVolume; } set { _BGMVolume = value; } }
    [SerializeField] float _sfxVolume;
    public float sfxVolume { get { return _sfxVolume * masterVolume; } set { _sfxVolume = value; } }
    [SerializeField] float _UIsfxVolume;
    public float UIsfxVolume { get { return _UIsfxVolume * masterVolume; } set { _UIsfxVolume = value; } }

    public float GetBGMValue()
    {
        return _BGMVolume;
    }

    public float GetSFXValue()
    {
        return _sfxVolume;
    }

}